using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Morningstar.Views.Addons;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Morningstar.Views
{
    class MenuView : BasicView
    {
        private List<Button> buttons = new List<Button>();
        Button bNew, bJoin, bQuit;


        public MenuView(SpriteBatch s, ContentManager c, Controller cl):base(s, c, cl)
        {           

            backgroundImage = content.Load<Texture2D>("backgroundMenu2");

            bNew = new Button(content, new Vector2(backgroundImage.Width, backgroundImage.Height), "game", 0);
            buttons.Add(bNew);
            bJoin = new Button(content, new Vector2(backgroundImage.Width, backgroundImage.Height), "lobby", 1);
            buttons.Add(bJoin);
            bQuit = new Button(content, new Vector2(backgroundImage.Width, backgroundImage.Height), "exit", 2);
            buttons.Add(bQuit);
        }


        public override void draw()
        {
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height), Color.White);

            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }

        public override void update(GameTime time)
        {
            foreach (Button button in buttons)
            {
                button.Update(Mouse.GetState());
            }

            if (bNew.isLeftClicked)
            {
                controller.newGame();
            }

            if (bJoin.isLeftClicked)
            {
                controller.joinGame();
            }

            if (bQuit.isLeftClicked)
            {
                controller.quitGame();
            }
        }

        public override void setActivityButtons(bool activity)
        {
            foreach (Button button in buttons)
            {
                button.isActive = activity;
            }
        }


    }
}
