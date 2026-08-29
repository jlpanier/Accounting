namespace Main.Services
{
    public interface IAlertService
    {
        /// <summary>
        /// Affichage d'une boite de dialogues de style "standard"
        /// </summary>
        Task ShowAlertAsync(string title, string message, string cancel);

        /// <summary>
        /// Affichage d'une boite de dialogues de style "error"
        /// </summary>
        Task ShowAlertAsync(Exception ex);

        /// <summary>
        /// Affichage d'une boite de dialogues de style "Info"
        /// </summary>
        Task ShowAlertAsync(string info);

        /// <summary>
        /// Affichage d'une boite de dialogues de style avec deux boutons
        /// </summary>
        Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel);
    }
}
