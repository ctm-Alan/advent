using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace advent
{
    public class CtmLogoScene : ISpecialScene
    {
        private Point topLeft = new Point(0, 0);
        public bool IsActive { get; private set; }

        public bool HidesTime { get; private set; }

        public bool RainbowSnow => false;
        public string Name => "CTM Logo";

        private TimeSpan elapsedThisScene;
        private Image<Rgba32> ctmLogo;
        private bool foundImage;

        private static TimeSpan sceneDuration = TimeSpan.FromSeconds(6);
        private static TimeSpan fadeDuration = TimeSpan.FromSeconds(0.5);

        public CtmLogoScene()
        {
            IsActive = false;
            HidesTime = false;
            foundImage = false;

            ctmLogo = Image.Load<Rgba32>("CtMLogo32x32.png");
            foundImage = true;
        }

        public void Activate()
        {
            if (foundImage)
            {
                elapsedThisScene = TimeSpan.Zero;
                IsActive = true;
                HidesTime = true;
            }
        }

        public void Elapsed(TimeSpan timeSpan)
        {
            elapsedThisScene += timeSpan;
            if (elapsedThisScene > sceneDuration)
            {
                IsActive = false;
                HidesTime = false;
            }
        }

        public void Draw(Image<Rgba32> img)
        {
            double fraction;
            if (elapsedThisScene < fadeDuration)
            {
                fraction = elapsedThisScene.TotalMilliseconds / fadeDuration.TotalMilliseconds;
            }
            else if (elapsedThisScene < sceneDuration - fadeDuration)
            {
                fraction = 1.0;
            }
            else
            {
                fraction = (sceneDuration - elapsedThisScene).TotalMilliseconds / fadeDuration.TotalMilliseconds;
            }

            fraction = Math.Min(1.0, Math.Max(0.0, fraction));
            if (IsActive)
            {
                img.Mutate(x => x.DrawImage(ctmLogo, topLeft, (float)fraction));
            }
        }
    }
}
