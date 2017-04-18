using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Morningstar
{
    public abstract class Asset
    {
        public string header;
    

        public Asset(Asset other)
        {
            header = other.header;
        }

        public Asset(string header)
        {
            this.header = header;   
        }

        public abstract void draw(ContentManager c, SpriteBatch s);
    }
}
