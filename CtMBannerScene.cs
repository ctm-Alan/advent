using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace advent
{
    public class CtmBannerScene : ISpecialScene
    {
        public bool IsActive { get; private set; }

        public bool HidesTime { get; private set; }

        public bool RainbowSnow => false;
        public string Name => "CTM Banner";

        private TimeSpan elapsedThisScene;
        private Image<Rgba32> ctmBanner;
        private bool foundImage;

        private static TimeSpan sceneDuration = TimeSpan.FromSeconds(15);
        private const int StartXOffset = 62;
        private const int TotalMovementDistance = 243;
        private static TimeSpan timeToMove = TimeSpan.FromSeconds(15);

        public CtmBannerScene()
        {
            IsActive = false;
            HidesTime = false;
            foundImage = false;

            ctmBanner = Image.Load<Rgba32>("CtMBanner32x180.png");
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
            var x = (elapsedThisScene.TotalMilliseconds / timeToMove.TotalMilliseconds) * TotalMovementDistance;
            var position = new Point(StartXOffset - (int)x, 0);
            if (IsActive)
            {
                img.Mutate(x => x.DrawImage(ctmBanner, position, 1f));
                if (position.X <= -180)
                    IsActive = false;
            }
        }
    }
}
