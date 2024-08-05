using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;


    public EncryptionService(IConfiguration configuration)
    {

        _key = Convert.FromBase64String(configuration.GetSection("EncryptionSettings")["Key"]);
        _iv = Convert.FromBase64String(configuration.GetSection("EncryptionSettings")["IV"]);

    }

    public string Encrypt(string plainText)
    {
        using (AesGcm aesGcm = new AesGcm(_key))
        {
            byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize]; // 12 bytes for GCM
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] ciphertext = new byte[plaintextBytes.Length];
            byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize]; // 16 bytes for GCM

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(nonce);
            }

            aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(nonce, 0, nonce.Length);
                ms.Write(ciphertext, 0, ciphertext.Length);
                ms.Write(tag, 0, tag.Length);

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];
        byte[] ciphertext = new byte[cipherTextBytes.Length - nonce.Length - tag.Length];

        Array.Copy(cipherTextBytes, 0, nonce, 0, nonce.Length);
        Array.Copy(cipherTextBytes, nonce.Length, ciphertext, 0, ciphertext.Length);
        Array.Copy(cipherTextBytes, nonce.Length + ciphertext.Length, tag, 0, tag.Length);

        using (AesGcm aesGcm = new AesGcm(_key))
        {
            byte[] plaintextBytes = new byte[ciphertext.Length];
            aesGcm.Decrypt(nonce, ciphertext, tag, plaintextBytes);

            return Encoding.UTF8.GetString(plaintextBytes);
        }
    }
}