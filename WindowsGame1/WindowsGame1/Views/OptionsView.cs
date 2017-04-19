using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Morningstar.Views.Addons;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using WindowsGame1.Views.Addons;

namespace Morningstar.Views
{
    class OptionsView : BasicView
    {
        private List<Button> buttons = new List<Button>();
        Label lNick;
        Button bBack, bSound;
        TextView tNick;


        public OptionsView(SpriteBatch s, ContentManager c, Controller cl) : base(s, c, cl)
        {

            backgroundImage = content.Load<Texture2D>("backgroundMenu2");

            bBack= new Button(content, new Vector2(backgroundImage.Width, backgroundImage.Height), "exit", 2);
            buttons.Add(bBack);
            bSound = new Button(content, new Vector2(backgroundImage.Width, backgroundImage.Height), "lobby", 0);
            buttons.Add(bSound);
            tNick = new TextView(content, new Rectangle(300, 300, 100, 50), "xDDDD");
        }


        public override void draw()
        {
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height), Color.White);

            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
            tNick.draw(content, spriteBatch);
        }

        public override void update(GameTime time)
        {
            foreach (Button button in buttons)
            {
                button.Update(Mouse.GetState());
            }

            if (bBack.isLeftClicked)
            {
                if (!(tNick.text == ""))
                    controller.nickname = tNick.text;
                else
                    tNick.text = controller.nickname;
                controller.backToMenu();
                return;

            }
            tNick.Update(Mouse.GetState(), Keyboard.GetState());

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
