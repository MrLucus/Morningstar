using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Morningstar.Views.Addons;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Morningstar.Views
{
    class BadLoginView : BasicView
    {
        private List<Button> buttons = new List<Button>();
        Button bOk;


        public BadLoginView(SpriteBatch s, ContentManager c, Controller cl) : base(s, c, cl)
        {

            backgroundImage = content.Load<Texture2D>("Menu/notServer");
            bOk= new Button(content, new Vector2(backgroundImage.Width, backgroundImage.Height), "exit", 0);
        }


        public override void draw()
        {
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 150, backgroundImage.Width, backgroundImage.Height), Color.White);
            bOk.Draw(spriteBatch);

        }

        public override void update(GameTime time)
        {
            bOk.Update(Mouse.GetState());

           if (bOk.isLeftClicked)
            {
                controller.returnMenu();
            }

        }

        public override void setActivityButtons(bool activity)
        {
            throw new NotImplementedException();
        }
    }
}
