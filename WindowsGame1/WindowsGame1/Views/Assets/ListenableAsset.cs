using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Morningstar.Views.Assets
{
    public class ListenableAsset : Asset
    {
        public string type;

        private static int number = 0;
        public ListenableAsset(string header, string type) : base(header)
        {

            this.type = type;
        }

        public ListenableAsset(ListenableAsset asset) : base(asset)
        {
            this.type = asset.type;
        }
        override
        public void draw(ContentManager content, SpriteBatch s)
        {
            SoundEffect efekt = content.Load<SoundEffect>("sounds/" + type);
            efekt.Play();
            number++;
            Console.WriteLine(number);
        }

    }
}
