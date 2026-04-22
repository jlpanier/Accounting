namespace Main.Services
{
    public class AssetService : IAssetService
    {
        /// <summary>
        /// Copie des fichiers de l'asset (cf. base de données) à l'initialisation
        /// </summary>
        public async Task<string> CopyAssetAsync(string assetName, string destPath)
        {
            string dbPathName = string.Empty;
            if (Directory.Exists(destPath))
            {
                using (var input = await FileSystem.OpenAppPackageFileAsync(assetName))
                {
                    dbPathName = Path.Combine(destPath, Path.GetFileName(assetName));
                    using (var output = File.Create(dbPathName))
                    {
                        await input.CopyToAsync(output);
                    }
                }
            }
            return dbPathName;
        }
    }
}
