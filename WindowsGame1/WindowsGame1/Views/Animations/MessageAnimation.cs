using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Morningstar.Views.Animations
{
    public class MessageAnimation:AnimatedEffect
    {
        Vector2 offset;
        private int lifetime;


        public MessageAnimation(String name, Vector2 position, ContentManager c, GameView gV) : base(name, position, c, gV) {

            if (name == "DEATH_MESSAGE")
            {
                lifetime = 20;
                offset = new Vector2(0, 0);
                framesCount = -1;
            }
            else if (name == "HIT_MESSAGE")
            {
                lifetime = 15;
                offset = new Vector2(15, -15);
                framesCount = -1;
            }


        }

        protected override void update()
        {
            position.Y -= 1;
            lifetime--;
            if (lifetime == 0) display.stopAnimation(this);
        }

        public override void draw(ContentManager content, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + offset, Color.White);
            update();
        }

    }
}
