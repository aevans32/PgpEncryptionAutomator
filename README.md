# 🔐 PGP Automator / PgpEncryptionAutomator

## 🇪🇸 Descripción en Español

**PGP Automator** es una API desarrollada en .NET que automatiza el proceso de encriptación de archivos utilizando una llave pública. Utiliza el paquete `PgpCore`, que a su vez es un wrapper del proyecto BouncyCastle para cifrado PGP.

### 🚀 Funcionalidades:
- Encripta archivos usando PGP.
- Permite subir y gestionar llaves públicas.
- Los archivos encriptados pueden ser desencriptados con herramientas externas como Kleopatra.
- Soporte para probar la API mediante Swagger si se usa Visual Studio.

### 📄 Requisitos:
- .NET 6.0 o superior
- Paquete NuGet `PgpCore`

### 🧩 ¿Qué se necesita para encriptar?
La API requiere dos archivos:
1. El archivo que se desea encriptar (de cualquier extensión).
2. La llave pública (formato `.asc`).

El resultado será un archivo `.pgp` encriptado.

### 🛠️ Instalación:
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/yourusername/PGPAutomator.git
   cd PGPAutomator
   ```

2. Restaurar los paquetes necesarios:
   ```bash
   dotnet restore
   ```

3. Compilar y ejecutar el proyecto:
   ```bash
   dotnet build
   dotnet run
   ```

### 📦 Paquetes NuGet utilizados:
- **PgpCore** (versión 6.5.1): facilita el uso de cifrado y descifrado PGP en proyectos .NET.

Instalación manual:
```bash
dotnet add package PgpCore
```

### 📬 Uso de la API:
- **Encriptar archivos:**  
  Enviar una petición POST a `/api/pgpencryption/encrypt` con:
  - Un archivo `.txt` o cualquier tipo de archivo
  - Una llave pública `.asc`  
  Se devolverá un archivo `.pgp`.

- **Desencriptar archivos:**  
  Usar una herramienta externa como Kleopatra junto con tu llave privada.

---

## 🇬🇧 English Description

**PGP Automator** is a .NET-based API that automates the encryption of files using a public key. It uses the `PgpCore` NuGet package, which wraps the BouncyCastle cryptography library.

### 🚀 Features:
- Encrypt files using PGP.
- Upload and manage public keys.
- Decrypt .pgp files using tools like Kleopatra.
- Swagger support available when running in Visual Studio.

### 📄 Requirements:
- .NET 6.0 (or later)
- `PgpCore` NuGet package

### 🧩 What is needed to encrypt?
The API requires two files:
1. The file to be encrypted (any extension).
2. The public key file (`.asc` format).

The result will be an encrypted `.pgp` file.

### 🛠️ Installation:
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/PGPAutomator.git
   cd PGPAutomator
   ```

2. Install NuGet packages:
   ```bash
   dotnet restore
   ```

3. Build and run:
   ```bash
   dotnet build
   dotnet run
   ```

### 📦 NuGet Packages Used:
- **PgpCore** (version 6.5.1): a simple PGP encryption/decryption wrapper for .NET.

Install manually:
```bash
dotnet add package PgpCore
```

### 📬 API Usage:
- **Encrypting files:**  
  Send a POST request to `/api/pgpencryption/encrypt` with:
  - A `.txt` or any file
  - A `.asc` public key  
  The API returns a `.pgp` encrypted file.

- **Decrypting files:**  
  Use an external tool like Kleopatra with your private key.

---

📅 **Autor / Author:** Andrés Evans  
🗓️ **Fecha / Date:** 10/10/2024
