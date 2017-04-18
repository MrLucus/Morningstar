using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Morningstar.Views
{
    public abstract class BasicView
    {
        public bool isActive { get; set; }

        protected Texture2D backgroundImage;

        protected ContentManager content;
        protected SpriteBatch spriteBatch;
        protected Controller controller;

        protected BasicView(SpriteBatch s, ContentManager c, Controller cl)
        {
            content = c;
            spriteBatch = s;
            controller = cl;
        }

        public abstract void update(GameTime time);

        public abstract void draw();

        public abstract void setActivityButtons(bool activity);
    }
}
