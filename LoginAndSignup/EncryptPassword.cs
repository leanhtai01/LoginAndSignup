using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoginAndSignup
{
    public class EncryptPassword
    {
        private readonly int SALT_SIZE = 10;

        /// <summary>
        /// Create salt value
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte[] CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[size];

            rng.GetBytes(salt);

            return salt;
        }

        /// <summary>
        /// Concatenate password with salt value
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private byte[] ConcatPwAndSalt(string password, byte[] salt)
        {
            byte[] pwBytes = Encoding.UTF8.GetBytes(password);
            byte[] pwAndSalt = new byte[salt.Length + pwBytes.Length];

            // concatenate saltBytes and pwBytes
            Buffer.BlockCopy(salt, 0, pwAndSalt, 0, salt.Length);
            Buffer.BlockCopy(pwBytes, 0, pwAndSalt, salt.Length, pwBytes.Length);

            return pwAndSalt;
        }

        /// <summary>
        /// Compute hash (sha512) from (password and salt)
        /// </summary>
        /// <param name="pwAndSalt"></param>
        /// <returns></returns>
        private byte[] ComputePwAndSaltHash(byte[] pwAndSalt)
        {
            SHA512Managed sha512Managed = new SHA512Managed();

            return sha512Managed.ComputeHash(pwAndSalt);
        }

        /// <summary>
        /// Concatenate salt with (password and salt)'s hash
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="pwAndSaltHash"></param>
        /// <returns></returns>
        private byte[] ConcatSaltWithPwAndSaltHash(byte[] salt, byte[] pwAndSaltHash)
        {
            byte[] saltWithPwAndSaltHash = new byte[salt.Length + pwAndSaltHash.Length];

            Buffer.BlockCopy(salt, 0, saltWithPwAndSaltHash, 0, salt.Length);
            Buffer.BlockCopy(pwAndSaltHash, 0, saltWithPwAndSaltHash, salt.Length, pwAndSaltHash.Length);

            return saltWithPwAndSaltHash;
        }

        /// <summary>
        /// Get salted password in string
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string GetSaltedPassword(string password, byte[] salt)
        {
            byte[] pwAndSalt = ConcatPwAndSalt(password, salt);
            byte[] saltedPwHash = ComputePwAndSaltHash(pwAndSalt);
            byte[] saltWithSaltedPwHash = ConcatSaltWithPwAndSaltHash(salt, saltedPwHash);

            return Convert.ToBase64String(saltWithSaltedPwHash);
        }

        /// <summary>
        /// Get salted password in string
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string GetSaltedPassword(string password)
        {
            byte[] salt = CreateSalt(SALT_SIZE);
            byte[] pwAndSalt = ConcatPwAndSalt(password, salt);
            byte[] saltedPwHash = ComputePwAndSaltHash(pwAndSalt);
            byte[] saltWithSaltedPwHash = ConcatSaltWithPwAndSaltHash(salt, saltedPwHash);

            return Convert.ToBase64String(saltWithSaltedPwHash);
        }

        /// <summary>
        /// Check whether a password valid given valid password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltedPassword"></param>
        /// <returns></returns>
        public bool IsPasswordValid(string password, string saltedPassword)
        {
            byte[] validPasswordBytes = Convert.FromBase64String(saltedPassword);
            byte[] salt = new byte[SALT_SIZE];

            Buffer.BlockCopy(validPasswordBytes, 0, salt, 0, SALT_SIZE);

            return GetSaltedPassword(password, salt) == saltedPassword;
        }
    }
}
