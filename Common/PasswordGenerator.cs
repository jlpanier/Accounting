using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// Génération de mot de passe
    /// </summary>
    public static class PasswordGenerator
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}";

        /// <summary>
        /// Génération de mot de passe
        /// </summary>
        public static string GeneratePassword(int length = 6)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);

            var sb = new StringBuilder(length);

            foreach (var b in bytes)
            {
                sb.Append(AllowedChars[b % AllowedChars.Length]);
            }

            return sb.ToString();
        }
    }
}
