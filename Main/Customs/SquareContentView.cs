
namespace Main.Customs
{
    /// <summary>
    /// Imposser une taille carrée pour le contrôle, avec une taille minimale de 250x250 pixels - tableau
    /// </summary>
    public class SquareContentView : ContentView
    {
        protected override void OnSizeAllocated(double width, double height)
        {
            const int minsize = 370;
            base.OnSizeAllocated(width, height);

            if (width <= 0 || height <= 0)
                return;

            double size = Math.Min(width, height);
            if (size < minsize) size = minsize;

            // On force le contrôle à être carré
            WidthRequest = size;
            HeightRequest = size;
        }
    }
}
