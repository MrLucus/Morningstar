using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace Morningstar
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Controller controller;
        List<Asset> assets;


        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 550;
            graphics.PreferredBackBufferWidth = 500;

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        //zainicjalizowanie kluczowych zasobow
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            assets = new List<Asset>();

            controller = new Controller(this, spriteBatch, Content);
            

            base.Initialize();
        }

        //zaladowanie ewentualnych zasobow - dzieje sie to jednak w odpowiednich klasach
        protected override void LoadContent() { }


        //Wszystkie aktualizacje tj. polozenia assets na mapie oraz ewentualnie przesylanie inputu
        protected override void Update(GameTime gameTime)
        {
            controller.updateActiveViews(gameTime);

            if (controller.isGameOn)
            {
                controller.sendAction(Keyboard.GetState());
                controller.sendAction(Mouse.GetState());
            }



            base.Update(gameTime);
        }


        //Rysowanie widokow
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            controller.drawActiveViews();

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
