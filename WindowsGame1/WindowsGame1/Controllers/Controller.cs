using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Morningstar.Views;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using System;
using Morningstar.Views.Assets;
using Microsoft.Xna.Framework;
using WindowsGame1.Views.Assets;

namespace Morningstar
{
    public class Controller
    {
        private Dictionary<viewKeys, BasicView> views; //lista wszystkich widokow
        private Game morningstar; //referencja do gry

        private Server server; //server, jeden na gre
        private Client player; //instancja klienta, jeden na gracza

        //obiekty koniecznie do wczytywania/rysowania zasobow
        private SpriteBatch spriteBatch;
        private ContentManager contentManager;

        //zmienna informujaca czy gra sie rozpoczela
        public bool isGameOn { get; set; }

        enum viewKeys { MENU, GAME , BAD_JOIN};

        private MouseState old;
        bool addBadJoin = false;

        //konstruktor przyjmuje jako parametr referencje do wywolujacej go gry
        public Controller(Game g, SpriteBatch s, ContentManager c)
        {
            spriteBatch = s;
            contentManager = c;

            morningstar = g;

            //stworzenie listy widokow - domyslnie jej pierwszym elementem jest menuView
            views = new Dictionary<viewKeys, BasicView>();

            MenuView menu = new MenuView(spriteBatch, contentManager, this);
            BadLoginView badLogView = new BadLoginView(spriteBatch, contentManager, this);
            GameView gameView = new GameView(spriteBatch, contentManager, this);

            menu.isActive = true;

            views.Add(viewKeys.GAME, gameView);
            views.Add(viewKeys.MENU, menu);
            views.Add(viewKeys.BAD_JOIN, badLogView);
        }

        //metoda zwracajaca aktualnie aktywny widok

        public void returnMenu()
        {
            foreach (var view in views)
                view.Value.isActive = false;

            views[viewKeys.MENU].isActive = true;
            views[viewKeys.MENU].setActivityButtons(true);

        }

        public void drawActiveViews()
        {
            foreach (var view in views)
                if (view.Value.isActive)
                    view.Value.draw();
        }

        public void updateActiveViews(GameTime gameTime )
        {

            foreach (var view in views)
                if (view.Value.isActive)
                    view.Value.update(gameTime);
            if(addBadJoin==true)
            {
                views[viewKeys.BAD_JOIN].isActive = true;
                views[viewKeys.MENU].setActivityButtons(false);
                addBadJoin = false;
            }


        }

        //zamkniecie gry
        public void quitGame()
        {
            morningstar.Exit();
        }

        //uruchomienie servera, rozpoczecie nasluchiwania
        public void newGame()
        {
            server = new Server();
            new Thread(server.waitForPlayers).Start();
        }

        //dolaczenie do gry jako nowy gracz (klient)
        public void joinGame()
        {
         //   if(Client.checkServer()==false)
         //       return;
            player = new Client(this);
            if (player.connectToServer() == false)
            {
                addBadJoin = true;
                return;
            }

            //'zamkniecie' menu
            views[viewKeys.MENU].isActive = false;

            //otworzenie widoku gry
            views[viewKeys.GAME].isActive = true;
            isGameOn = true;
        }

        //przetwarza zadanie z gameView, wola metode odbierajaca dane
        public void updateAssets(List<Asset> assets)
        {
            if (isGameOn)
            {
                List<Asset> Assets = new List<Asset>();

                foreach (var asset in assets)
                {
                    switch (asset.header)
                    {
                        case "EVENT":
                            (views[viewKeys.GAME] as GameView).addAnimation(asset as DrawableAsset);
                            break;
                        case "ENTITY":
                            Assets.Add(new DrawableAsset(asset as DrawableAsset));
                            break;
                        case "SCORE":
                            Assets.Add(new ScoreAsset(asset as ScoreAsset));
                            break;
                        case "SOUNDS":

                            Assets.Add(new ListenableAsset(asset as ListenableAsset));
                            break;
                    }
                }

            (views[viewKeys.GAME] as GameView).assets = Assets;
            }
        }

        //przetwarza wyslanie stanu klawiatury
        public void sendAction(KeyboardState keyboardState)
        {
            if (isGameOn)
            {
                player.sendAction(keyboardState);
                (views[viewKeys.GAME] as GameView).showScores = false;
                if (keyboardState.IsKeyDown(Keys.Tab))
                    (views[viewKeys.GAME] as GameView).showScores = true;
            }
        }

        public void sendAction(MouseState mouse)
        {
            if ((mouse.LeftButton == ButtonState.Pressed) && (old.LeftButton == ButtonState.Released))
            {
                player.sendAction(mouse);
            }

            old = mouse;

        }

        public void backToMenu()
        {
            isGameOn = false;
            views[viewKeys.MENU].isActive = true;
        }
    }
}


