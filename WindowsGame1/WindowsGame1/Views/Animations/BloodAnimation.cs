using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Morningstar.Views.Animations
{
    public class BloodAnimation:AnimatedEffect
    {
        private const int animationFrames = 6;

        private int sourceWidth = 32;
        private int sourceHeight = 32;

        public BloodAnimation(Vector2 position, ContentManager c, GameView gV):base("BLOOD", position, c, gV)
        {
            framesCount = animationFrames;
            frameCurrent = 0;

            frameHeight = (int)size.Y;
            frameWidth = (int)size.X / animationFrames;
        }
    
    }
}
