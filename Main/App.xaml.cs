using FFImageLoading.Helpers;
using Repository.Dbo;

namespace Main
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected async Task InitialiseAsync()
        {
            try
            {
                AppPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                DbPath = Path.Combine(AppPath, "db");
                ImagePath = Path.Combine(AppPath, "images");
                FilePath = Path.Combine(AppPath, "file");
                TmpPath = Path.Combine(AppPath, "tmp");
                MapPath = Path.Combine(AppPath, "maps");

                Directory.CreateDirectory(AppPath);
                Directory.CreateDirectory(DbPath);
                Directory.CreateDirectory(FilePath);
                Directory.CreateDirectory(TmpPath);
                Directory.CreateDirectory(ImagePath);
                Directory.CreateDirectory(MapPath);

                DatabaseAccess.Instance.Init(Path.Combine(DbPath, BaseDbo.DatabaseName));
            }
            catch (FileNotFoundException)
            {
                var asset = ServiceHelper.GetService<IAssetService>();
                await asset.CopyAssetAsync(BaseDbo.DatabaseName, DbPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur d'initialisation de la base de données : " + ex.Message);
                if (!DatabaseAccess.Instance.IsReady())
                {
                    var asset = ServiceHelper.GetService<IAssetService>();
                    await asset.CopyAssetAsync(BaseDbo.DatabaseName, DbPath);
                }
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());
            _ = InitialiseAsync();
            return window;
        }

        /// <summary>
        /// Répertoire interne de l'application
        /// </summary>
        public string AppPath { get; private set; } = "";

        /// <summary>
        /// Répertoire de stockage des cartes
        /// </summary>
        public string MapPath { get; private set; } = "";

        /// <summary>
        /// Chemin des images 
        /// </summary>
        public string ImagePath { get; private set; } = "";

        /// <summary>
        /// Chemin de la base de données
        /// </summary>
        public string DbPath { get; private set; } = "";

        /// <summary>
        /// Chemin complet de la base de données
        /// </summary>
        public string DbFilePath { get; private set; } = "";

        /// <summary>
        /// Chemin de partage des fichiers
        /// </summary>
        public string FilePath { get; private set; } = "";

        /// <summary>
        /// Chemin temporaire des fichiers
        /// </summary>
        public string TmpPath { get; private set; } = "";
    }
}
