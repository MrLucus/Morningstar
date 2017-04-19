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
    class TextView
    {
        public string text { get; set; }

        private MouseState presentMouse, pastMouse;
        const int A = 65;
        const int Z = 90;
        const int ENTER = 13;
        const int BACKSPACE = 8;
        private ContentManager content;
        private bool isActive { get; set; }
        private Rectangle position;
        private Dictionary<Keys, bool> keysPressed; // czy guzik był wciśnięty
        public TextView(ContentManager newContent, Rectangle newPosition, String newText)
        {
            content = newContent;
            position = newPosition;
            text = newText;
            isActive = false;
            keysPressed = new Dictionary<Keys, bool>();
            for(int i=A;i<=Z;i++)// od A do Z
            {
                keysPressed.Add((Keys)i, false);
            }
            keysPressed.Add((Keys)BACKSPACE, false); //przycisk backspace
            keysPressed.Add((Keys)ENTER, false); //przycisk backspace
        }
            



        public bool isMouseOver
        {
            get
            {
                return  position.Contains(new Point(presentMouse.X, presentMouse.Y));
            }
        }

        public bool clickedEverywhere
        {
            get { return (presentMouse.LeftButton == ButtonState.Released) && pastMouse.LeftButton == ButtonState.Pressed; }
        }
        public bool isLeftClicked
        {
            get { return isMouseOver && clickedEverywhere; }
        }


        public void Update(MouseState mouse, KeyboardState keyboardState)
        {
            pastMouse = presentMouse;
            presentMouse = mouse;
            Console.WriteLine(mouse.X +"  "+mouse.Y);
            if (isLeftClicked) isActive = true;
            else if (clickedEverywhere) isActive = false;
            if (isActive == false)
                return;
            for(int i=A;i<=Z;i++)
            {
                if (keyboardState.IsKeyDown((Keys)i) && keysPressed[(Keys)i] == false)
                {
                    if(text.Length<12)
                    text += (char)i;
                    keysPressed[(Keys)i] = true;

                }
                else if(keyboardState.IsKeyUp((Keys)i))
                    keysPressed[(Keys)i] = false;
            }
            if (keyboardState.IsKeyDown((Keys)BACKSPACE) && keysPressed[(Keys)BACKSPACE] == false)
            {
                if(text.Length>0)
                text=text.Remove(text.Length - 1, 1);
                keysPressed[(Keys)BACKSPACE] = true;

            }
            else if (keyboardState.IsKeyUp((Keys)BACKSPACE))
                keysPressed[(Keys)BACKSPACE] = false;
            if (keyboardState.IsKeyDown((Keys)ENTER) && keysPressed[(Keys)ENTER] == false)
            {
                keysPressed[(Keys)ENTER] = true;
                isActive = false;

            }
            else if (keyboardState.IsKeyUp((Keys)ENTER))
                keysPressed[(Keys)ENTER] = false;

        }

        public void draw(ContentManager content, SpriteBatch s)
        {
            if(isActive)
                 s.DrawString(content.Load<SpriteFont>("normal"), text, new Vector2(position.X,position.Y), Color.Red);
            else
                s.DrawString(content.Load<SpriteFont>("scoreboard"), text, new Vector2(position.X, position.Y), Color.Red);
        }


    }
}
