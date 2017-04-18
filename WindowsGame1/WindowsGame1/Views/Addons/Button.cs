using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Morningstar.Views.Addons
{
    class Button
    {
        private MouseState presentMouse, pastMouse;

        private ContentManager content;

        private Texture2D textureNormal, textureHighlighted, textureCurrent;

        private Rectangle position;

        public bool isActive { get; set; }

        public Button(ContentManager newContent, Vector2 windowScreen, String name, int number)
        {
            content = newContent;

            textureNormal = content.Load<Texture2D>("Menu/" + name);
            textureHighlighted = content.Load<Texture2D>("Menu/" + name + "Highlighted");
            textureCurrent = textureNormal;

            position = new Rectangle((int)windowScreen.X / 2 - textureNormal.Width / 2, (int)(windowScreen.Y * (0.2 + 0.3 * number)),
                    textureNormal.Width, textureNormal.Height);

            isVisible = true;
            isActive = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureCurrent, position, Color.White);

        }


        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public bool isMouseOver
        {
            get
            {
                return isActive && isVisible && position.Contains(new Point(presentMouse.X, presentMouse.Y));
            }
        }

        public bool isLeftClicked
        {
            get { return isMouseOver && (presentMouse.LeftButton == ButtonState.Released) && pastMouse.LeftButton == ButtonState.Pressed; }
        }


        public void Update(MouseState mouse)
        {
            pastMouse = presentMouse;
            presentMouse = mouse;
            if (isMouseOver) textureCurrent = textureHighlighted;
            else textureCurrent = textureNormal;
        }

    }
}
