using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Views.Addons
{
    class Label
    {

        protected bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        private Dictionary<int, bool> views; //lista wszystkich widokow
        protected ContentManager content;

        protected Texture2D textureCurrent, textureNormal;

        protected Rectangle position;

        public bool isActive { get; set; }

        public Label(ContentManager newContent, Vector2 windowScreen, String name, int number)
        {
            content = newContent;

            textureNormal = content.Load<Texture2D>("Menu/" + name);
            textureCurrent = textureNormal;

            position = new Rectangle((int)windowScreen.X / 2 - textureNormal.Width / 2, (int)(windowScreen.Y * (0.2 + 0.2 * number)),
                    textureNormal.Width, textureNormal.Height);

            isVisible = true;
            isActive = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureCurrent, position, Color.White);

        }

    }
}
