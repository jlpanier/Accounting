/// <summary>
/// Gestion des paramètres Android
/// </summary>
public partial class DownloadFolderService : IDownloadFolderService
{
    /// <summary>
    /// Obtenir le répertoire de téléchargement
    /// </summary>
    public string GetDownloadFolder() => Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)!.AbsolutePath;

}
