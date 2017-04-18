using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Morningstar.Views.Assets
{
    public class DrawableAsset:Asset
    {
        public string type;
        public Vector2 position;
        public DrawableAsset(string header, string type, Vector2 position) : base(header) {
            this.type = type;
            this.position = position;
        }

        public DrawableAsset(DrawableAsset asset) : base(asset)
        {
            this.type = asset.type;
            this.position = asset.position;
        }
        override
        public void draw(ContentManager content, SpriteBatch s)
        {
            s.Draw(content.Load<Texture2D>(type), position, Color.White);           
        }

    }
}
