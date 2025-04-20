<<<<<<< HEAD
PgpEncryptionAutomator

Primera Version de una API que automatiza el proceso de encryptacion de un archivo segun la llave publica que se indique.

La API requiere dos archivos para su proceso: el archivo a ser encryptado (cualquier extension) y la llave publica (extesnion .asc)
El resultado es el archivo encryptado (extension .pgp)

Si se cuenta con Visual Studio, el controlador se puede probar mediante Swagger.

Realizado por: Andres Evans - 10/10/2024
=======
# PGP Automator

## Overview
**PGP Automator** is a .NET-based API that provides PGP encryption functionality. It allows users to encrypt files using a public key, leveraging the `PgpCore` NuGet package.

### Features:
- Encrypt files using PGP.
- Upload and manage public keys.
- Easily decrypt PGP-encrypted files with external tools like Kleopatra.

## Requirements
- .NET 6.0 (or later)
- `PgpCore` NuGet package

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/PGPAutomator.git
   cd PGPAutomator

2. Install the necessary NuGet packages, including PgpCore:
   dotnet restore

3. Build and run the project:
   dotnet build
   dotnet run

## NuGet Packages
The following NuGet package is required for PGP encryption functionality:

PgpCore
Version: 6.5.1

Description: A wrapper around the BouncyCastle library, making PGP encryption and decryption easy in .NET projects.

Installation:
dotnet add package PgpCore

## How to Use
Encrypting Files:

Send a POST request with a .txt file and a public key to the API endpoint /api/pgpencryption/encrypt.
The API returns an encrypted .pgp file.
Decrypting Files:

Use an external PGP tool (like Kleopatra) with your private key to decrypt the encrypted .pgp files.

>>>>>>> Result_title
