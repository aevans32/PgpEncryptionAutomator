using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PGPAutomator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PgpEncryptionController : ControllerBase
    {
        [HttpPost("encrypt")]
        public async Task<IActionResult> EncryptFile([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.PublicKey == null)
                return BadRequest("Se necesitan un archivo y una llave publica.");

            // Crear un directorio temporal para cargar archivos
            var tempDirectory = Path.Combine(Path.GetTempPath(), "PgpEncryptionKeys");
            if (!Directory.Exists(tempDirectory))
                Directory.CreateDirectory(tempDirectory);

            // Guardar el archivo cargado y la llave publica temporalmente
            var inputFilePath = Path.Combine(tempDirectory, request.File.FileName);
            var publicKeyPath = Path.Combine(tempDirectory, "publicKey.asc");
            var outputFilePath = Path.Combine(tempDirectory, "encryptedFile.pgp");

            await using (var inputFileStream = new FileStream(inputFilePath, FileMode.Create))
            {
                await request.File.CopyToAsync(inputFileStream);
            }

            await using (var publicKeyFileStream = new FileStream(publicKeyPath, FileMode.Create))
            {
                await request.PublicKey.CopyToAsync(publicKeyFileStream);
            }

            try
            {
                // Cargar la llave publica
                PgpEncryptionKeys encryptionKeys = new PgpEncryptionKeys(publicKeyPath);

                // Instaurar la clase PgpEncrypt y realizar la encryptacion
                PgpEncrypt pgpEncrypt = new PgpEncrypt(encryptionKeys);
                using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
                {
                    pgpEncrypt.Encrypt(outputFileStream, new FileInfo(inputFilePath));
                }

                // retornar el archivo encryptado al usuario
                var encryptedBytes = await System.IO.File.ReadAllBytesAsync(outputFilePath);
                return File(encryptedBytes, "application/octet-stream", "encryptedFile.pgp");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error: {ex.Message}");
            }
        }
    }

    // Modelo para manejar las cargas de archivos
    public class FileUploadRequest
    {
        public IFormFile File { get; set; }
        public IFormFile PublicKey { get; set; }
    }

    // Clase PgpEncrypt
    public class PgpEncrypt
    {
        private PgpEncryptionKeys m_encryptionKeys;
        private const int BufferSize = 0x10000;

        public PgpEncrypt(PgpEncryptionKeys encryptionKeys)
        {
            if (encryptionKeys == null)
            {
                throw new ArgumentNullException("encryptionKeys", "encryptionKeys is null.");
            }
            m_encryptionKeys = encryptionKeys;
        }

        public void Encrypt(Stream outputStream, FileInfo unencryptedFileInfo)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream", "outputStream is null.");
            }
            if (unencryptedFileInfo == null)
            {
                throw new ArgumentNullException("unencryptedFileInfo", "unencryptedFileInfo is null.");
            }
            if (!File.Exists(unencryptedFileInfo.FullName))
            {
                throw new ArgumentException("No se encontro el archivo a encryptar.");
            }
            using (Stream encryptedOut = ChainEncryptedOut(outputStream))
            {
                using (Stream literalOut = ChainLiteralOut(encryptedOut, unencryptedFileInfo))
                using (FileStream inputFile = unencryptedFileInfo.OpenRead())
                {
                    WriteOutput(literalOut, inputFile);
                }
            }
        }

        private static void WriteOutput(Stream literalOut, FileStream inputFile)
        {
            int length = 0;
            byte[] buf = new byte[BufferSize];
            while ((length = inputFile.Read(buf, 0, buf.Length)) > 0)
            {
                literalOut.Write(buf, 0, length);
            }
        }

        private Stream ChainEncryptedOut(Stream outputStream)
        {
            PgpEncryptedDataGenerator encryptedDataGenerator;
            encryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.TripleDes, new SecureRandom());
            encryptedDataGenerator.AddMethod(m_encryptionKeys.PublicKey);
            return encryptedDataGenerator.Open(outputStream, new byte[BufferSize]);
        }

        private static Stream ChainLiteralOut(Stream encryptedOut, FileInfo file)
        {
            PgpLiteralDataGenerator pgpLiteralDataGenerator = new PgpLiteralDataGenerator();
            return pgpLiteralDataGenerator.Open(encryptedOut, PgpLiteralData.Binary, file);
        }
    }

    // Clase PgpEncryptionKeys para cargar la llave publica
    public class PgpEncryptionKeys
    {
        public PgpPublicKey PublicKey { get; private set; }

        public PgpEncryptionKeys(string publicKeyFilePath)
        {
            if (File.Exists(publicKeyFilePath))
            {
                using (var keyIn = File.OpenRead(publicKeyFilePath))
                {
                    PublicKey = ReadPublicKey(keyIn);
                }
            }
            else
            {
                throw new ArgumentException("LLave publica no encontrada.");
            }
        }

        private PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);
            PgpPublicKeyRingBundle pgpPub = new PgpPublicKeyRingBundle(inputStream);

            foreach (PgpPublicKeyRing keyRing in pgpPub.GetKeyRings())
            {
                foreach (PgpPublicKey key in keyRing.GetPublicKeys())
                {
                    if (key.IsEncryptionKey)
                    {
                        return key;
                    }
                }
            }

            throw new ArgumentException("No se encontro una llave de encriptacion en la coleccion de llaves publicas.");
        }
    }
}
