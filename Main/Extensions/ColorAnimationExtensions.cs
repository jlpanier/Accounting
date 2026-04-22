namespace Main.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Microsoft.Maui.Graphics.Color"/> animations
    /// </summary>
    public static partial class ColorAnimationExtensions
    {
        /// <summary>
        /// Animates the BackgroundColor of a VisualElement to the given color
        /// </summary>
        /// <param name="element"></param>
        /// <param name="color">The target color to _animate the VisualElement's BackgroundColor to</param>
        /// <param name="rate">The time, in milliseconds, between the frames of the animation</param>
        /// <param name="length">The _duration, in milliseconds, of the animation</param>
        /// <param name="easing">The easing function to be used in the animation</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Value indicating if the animation completed successfully or not</returns>
        public static Task<bool> BackgroundColorTo(this VisualElement element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(color);

            // Although BackgroundColor is defined as not-nullable, it CAN be null
            // If null => set it to Transparent as Animation will crash on null BackgroundColor
            element.BackgroundColor ??= Colors.Transparent;

            var animationCompletionSource = new TaskCompletionSource<bool>();

            try
            {
                new Animation
            {
                { 0, 1, GetRedTransformAnimation(element, color.Red) },
                { 0, 1, GetGreenTransformAnimation(element, color.Green) },
                { 0, 1, GetBlueTransformAnimation(element, color.Blue) },
                { 0, 1, GetAlphaTransformAnimation(element, color.Alpha) },
            }
                .Commit(element, nameof(BackgroundColorTo), rate, length, easing, (d, b) => animationCompletionSource.SetResult(true));
            }
            catch (ArgumentException aex)
            {
                //When creating an Animation too early in the lifecycle of the Page, i.e., in the OnAppearing method,
                //the Page might not have an 'IAnimationManager' yet, resulting in an ArgumentException.
                System.Diagnostics.Trace.WriteLine($"{aex.GetType().Name} thrown in {typeof(ColorAnimationExtensions).FullName}: {aex.Message}");
                animationCompletionSource.SetResult(false);
            }

            return animationCompletionSource.Task.WaitAsync(token);


            static Animation GetRedTransformAnimation(VisualElement element, float targetRed) =>
                new(v => element.BackgroundColor = element.BackgroundColor.WithRed(v), element.BackgroundColor.Red, targetRed);

            static Animation GetGreenTransformAnimation(VisualElement element, float targetGreen) =>
                new(v => element.BackgroundColor = element.BackgroundColor.WithGreen(v), element.BackgroundColor.Green, targetGreen);

            static Animation GetBlueTransformAnimation(VisualElement element, float targetBlue) =>
                new(v => element.BackgroundColor = element.BackgroundColor.WithBlue(v), element.BackgroundColor.Blue, targetBlue);

            static Animation GetAlphaTransformAnimation(VisualElement element, float targetAlpha) =>
                new(v => element.BackgroundColor = element.BackgroundColor.WithAlpha((float)v), element.BackgroundColor.Alpha, targetAlpha);
        }

        /// <summary>
        /// Applies the supplied <paramref name="redComponent"/> to this <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to modify.</param>
        /// <param name="redComponent">The red component to apply to the existing <see cref="Color"/>. Note this value must be between 0 and 1.</param>
        /// <returns>A <see cref="Color"/> with the supplied <paramref name="redComponent"/> applied.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="color"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="redComponent"/> is <b>not</b> between 0 and 1.</exception>
        public static Color WithRed(this Color color, double redComponent)
        {
            ArgumentNullException.ThrowIfNull(color);
            return redComponent is < 0 or > 1
                    ? throw new ArgumentOutOfRangeException(nameof(redComponent))
                    : Color.FromRgba(redComponent, color.Green, color.Blue, color.Alpha);
        }

        /// <summary>
        /// Applies the supplied <paramref name="greenComponent"/> to this <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to modify.</param>
        /// <param name="greenComponent">The green component to apply to the existing <see cref="Color"/>. Note this value must be between 0 and 1.</param>
        /// <returns>A <see cref="Color"/> with the supplied <paramref name="greenComponent"/> applied.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="color"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="greenComponent"/> is <b>not</b> between 0 and 1.</exception>
        public static Color WithGreen(this Color color, double greenComponent)
        {
            ArgumentNullException.ThrowIfNull(color);
            return greenComponent is < 0 or > 1
                    ? throw new ArgumentOutOfRangeException(nameof(greenComponent))
                    : Color.FromRgba(color.Red, greenComponent, color.Blue, color.Alpha);
        }

        /// <summary>
        /// Applies the supplied <paramref name="blueComponent"/> to this <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to modify.</param>
        /// <param name="blueComponent">The blue component to apply to the existing <see cref="Color"/>. Note this value must be between 0 and 1.</param>
        /// <returns>A <see cref="Color"/> with the supplied <paramref name="blueComponent"/> applied.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="color"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="blueComponent"/> is <b>not</b> between 0 and 1.</exception>
        public static Color WithBlue(this Color color, double blueComponent)
        {
            ArgumentNullException.ThrowIfNull(color);

            return blueComponent is < 0 or > 1
                    ? throw new ArgumentOutOfRangeException(nameof(blueComponent))
                    : Color.FromRgba(color.Red, color.Green, blueComponent, color.Alpha);
        }
    }
}
