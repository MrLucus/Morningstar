using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Morningstar.Views.Animations
{
    public abstract class AnimatedEffect
    {
        protected Texture2D texture;

        protected int framesCount;
        protected int frameCurrent;

        protected int frameHeight;
        protected int frameWidth;

        protected double delay;
        protected double delayStep;

        protected Vector2 position;
        protected Vector2 size;

        protected GameView display;

        public AnimatedEffect(String name, Vector2 position, ContentManager c, GameView gV)
        {
            display = gV;
            texture = (c.Load<Texture2D>("Animations/" + name));
            size = new Vector2(texture.Width, texture.Height);

            this.position = position;

            delay = 0;
            delayStep = 0.3;
            //mozna wyszukiwac gracza po pozycji i przypiac do niego animacje (TO DO)
        }

        protected virtual void update() {

            delay += delayStep;
            if (delay >= 1)
            {
                frameCurrent += 1;
                delay = 0.0;
            }

            if (frameCurrent == framesCount)
            {
                frameCurrent = 0;
                display.stopAnimation(this);
            }

        }

        public virtual void draw(ContentManager content, SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 32, 32),
                             new Rectangle(frameWidth * frameCurrent, 0, frameWidth, frameHeight),
                             Color.White);
            update();

        }

    }
}
