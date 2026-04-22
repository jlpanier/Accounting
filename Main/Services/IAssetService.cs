namespace Main.Services
{
    public interface IAssetService
    {
        /// <summary>
        /// Copie des fichiers de l'asset (cf. base de données) à l'initialisation
        /// </summary>
        Task<string> CopyAssetAsync(string assetName, string destPath);
    }
}
