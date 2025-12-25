using System;
using System.Security.Cryptography;
using System.Text;

namespace Mihaylov.Api.Site.Data.Helpers
{
    public class AesGcmHelper : IAesGcmHelper
    {
        // AES-256-GCM                             N + 28 bytes    12-byte nonce + 16-byte tag
        // (other option) AES-CTR + HMAC-256       N + 48 bytes    16-byte nonce + 32-byte HMAC

        private const int KEY_SIZE_BYTES = 32;     // 32*8 = 256 bits
        private const int NONCE_SIZE_BYTES = 12;   // 12*8 = 96 bits, recommended for GCM
        private const int TAG_SIZE_BYTES = 16;     // 16*8 = 128 bits, standard tag length

        private readonly byte[] _key;

        public AesGcmHelper(string key)
        {
            // var key = cypher.GenerateKey();             // KEY_SIZE_BYTES     // 12
            // var key64 = Convert.ToBase64String(key);

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _key = Convert.FromBase64String(key);

            if (key.Length != KEY_SIZE_BYTES)
            {
                throw new ArgumentException("Key must be 32 bytes (256 bits)", nameof(key));
            }
        }

        public void Test()
        {
            // Example
            string text = "Secret message";
            byte[] plaintext = Encoding.UTF8.GetBytes(text);

            // Encrypt (get combined bytes)
            byte[] encryptedCombined = Encrypt(plaintext);

            string b64 = Convert.ToBase64String(encryptedCombined);
            Console.WriteLine("Encrypted (base64): " + b64);

            // Decrypt
            byte[] decrypted = Decrypt(encryptedCombined);

            string decryptedText = Encoding.UTF8.GetString(decrypted);
            Console.WriteLine("Decrypted: " + decryptedText);
        }

        /// <summary>
        /// Encrypt: returns combined = nonce || ciphertext || tag
        /// </summary>
        /// <param name="plainData"></param>
        /// <param name="associatedData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] Encrypt(byte[] plainData, byte[] associatedData = null)
        {
            if (plainData == null)
            {
                throw new ArgumentNullException(nameof(plainData));
            }

            byte[] nonce = new byte[NONCE_SIZE_BYTES];
            RandomNumberGenerator.Fill(nonce);

            byte[] ciphertext = new byte[plainData.Length];
            byte[] tag = new byte[TAG_SIZE_BYTES];

            // AesGcm implements IDisposable
            using (var aes = new AesGcm(_key, TAG_SIZE_BYTES))
            {
                // encrypt in-place style
                aes.Encrypt(nonce, plainData, ciphertext, tag, associatedData);
            }

            // Combine: nonce || ciphertext || tag
            byte[] combined = new byte[nonce.Length + ciphertext.Length + tag.Length];
            Buffer.BlockCopy(nonce, 0, combined, 0, nonce.Length);
            Buffer.BlockCopy(ciphertext, 0, combined, nonce.Length, ciphertext.Length);
            Buffer.BlockCopy(tag, 0, combined, nonce.Length + ciphertext.Length, tag.Length);

            return combined;
        }

        /// <summary>
        /// Decrypt from combined = nonce || ciphertext || tag
        /// </summary>
        /// <param name="combined"></param>
        /// <param name="associatedData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="CryptographicException">auth failure</exception>
        public byte[] Decrypt(byte[] combined, byte[] associatedData = null)
        {
            if (combined == null)
            {
                throw new ArgumentNullException(nameof(combined));
            }

            if (combined.Length < NONCE_SIZE_BYTES + TAG_SIZE_BYTES)
            {
                throw new ArgumentException("Combined data is too short", nameof(combined));
            }

            byte[] nonce = new byte[NONCE_SIZE_BYTES];
            Buffer.BlockCopy(combined, 0, nonce, 0, NONCE_SIZE_BYTES);

            int cipherDataLen = combined.Length - NONCE_SIZE_BYTES - TAG_SIZE_BYTES;
            byte[] cipherData = new byte[cipherDataLen];
            Buffer.BlockCopy(combined, NONCE_SIZE_BYTES, cipherData, 0, cipherDataLen);

            byte[] tag = new byte[TAG_SIZE_BYTES];
            Buffer.BlockCopy(combined, NONCE_SIZE_BYTES + cipherDataLen, tag, 0, TAG_SIZE_BYTES);

            byte[] plainData = new byte[cipherDataLen];

            using (var aes = new AesGcm(_key, TAG_SIZE_BYTES))
            {
                // Will throw CryptographicException if tag verification fails
                aes.Decrypt(nonce, cipherData, tag, plainData, associatedData);
            }

            return plainData;
        }

        public byte[] GenerateKey()
        {
            byte[] key = new byte[KEY_SIZE_BYTES];
            RandomNumberGenerator.Fill(key);

            return key;
        }
    }
}
