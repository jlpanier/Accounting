using Common;

namespace Main.Services
{
    /// <summary>
    /// Gestion des boites de dialogues
    /// </summary>
    public class AlertService : IAlertService
    {
        /// <summary>
        /// Type de dialog
        /// </summary>
        public enum typeDialg
        {
            [StringValue("⚠️ Attention")]
            Warning,
            [StringValue("ℹ️ Info")]
            Info,
            [StringValue("✅ Succès")]
            Success,
            [StringValue("❌ Erreur")]
            Error,
            [StringValue("")]
            None
        }

        /// <summary>
        /// Standard bouton OK
        /// </summary>
        public const string OK = "Ok";

        /// <summary>
        /// Affichage d'une boite de dialogues de style "Info"
        /// </summary>
        public Task ShowAlertAsync(string info)
        {
            return Shell.Current.DisplayAlertAsync(typeDialg.Info.GetStringValue(), info, OK);
        }

        /// <summary>
        /// Affichage d'une boite de dialogues de style "error"
        /// </summary>
        public Task ShowAlertAsync(Exception ex)
        {
            return Shell.Current.DisplayAlertAsync(typeDialg.Error.GetStringValue(), ex.Message, OK);
        }

        /// <summary>
        /// Affichage d'une boite de dialogues de style "standard"
        /// </summary>
        public Task ShowAlertAsync(string title, string message, string cancel)
        {
            return Shell.Current.DisplayAlertAsync(title, message, cancel);
        }

        /// <summary>
        /// Affichage d'une boite de dialogues de style avec deux boutons
        /// </summary>
        public Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel)
        {
            return Shell.Current.DisplayAlertAsync(title, message, accept, cancel);
        }
    }
}
