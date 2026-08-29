using ICSharpCode.SharpZipLib.Zip;

namespace Common
{
    /// <summary>
    /// Gestion des fichiers zip
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>
        /// zipper un fichier avec password
        /// </summary>
        public static string Zip(string source, string zipPath, string password)
        {
            if (!File.Exists(source))
                throw new FileNotFoundException("Fichier introuvable", source);

            if (File.Exists(zipPath)) File.Delete(zipPath);

            using (var fs = File.Create(zipPath))
            {
                using (var zipStream = new ZipOutputStream(fs))
                {
                    zipStream.SetLevel(9);              // compression max
                    zipStream.Password = password;      // mot de passe

                    // Quand ZipOutputStream sera fermé, il fermera aussi le flux sous‑jacent.
                    //      IsStreamOwner = true → zipStream.Close() ferme aussi FileStream
                    //      IsStreamOwner = false → zipStream.Close() ne ferme pas FileStream
                    zipStream.IsStreamOwner = true;

                    // AES‑256
                    var entry = new ZipEntry(Path.GetFileName(source))
                    {
                        AESKeySize = 256,
                        DateTime = DateTime.Now
                    };

                    zipStream.PutNextEntry(entry);

                    using var fileStream = File.OpenRead(source);
                    {
                        fileStream.CopyTo(zipStream);
                    }

                    zipStream.CloseEntry();
                    zipStream.Finish();

                    return zipPath;
                }

            }
        }
    }

}
