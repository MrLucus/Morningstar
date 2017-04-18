using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Morningstar.Model.Entities;
using Morningstar.Views;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using static Morningstar.Views.BasicView;
using System.Linq;
using System.Text;
using Morningstar.Views.Assets;

namespace Morningstar.Model
{
    public class World
    {
        private const int frequencyRandomizedActions = 400;
        private const int maxHealth = 3;
        public List<Entity> entities = new List<Entity>();
        public List<Asset> assets = new List<Asset>();

        public int playerCount { get; set; }

        Vector2 Size = new Vector2(520, 470);
        public void update()
        {
            //zamiana 'Entities' na 'Assets' by mozna je bylo przeslac przez server
            assets = new List<Asset>();
            int countPlayers=0;
            foreach (var entity in entities)
            {
                Asset temporary = entity.toAsset();
                if(temporary!=null)
                assets.Add(temporary);

                if (entity is Player)
                {
                    temporary = (entity as Player).healthToAsset();
                    if(temporary!=null)
                    assets.Add(temporary);
                    assets.Add((entity as Player).toScoreAsset(countPlayers));
                    countPlayers++;
                }
                else if(entity is Bullet)
                {
                    if ((entity as Bullet).wasSended == true)
                        continue;
                    assets.Add((entity as Bullet).shootToAsset());
                    (entity as Bullet).wasSended = true;

                }

            }
            randomizedActions();
            for (int i = 0; i < entities.Count(); i++) entities[i].update();
        }


        public void shot(Player source, Vector2 target)
        {
            Bullet star = new Entities.Bullet(source, target, this);
                entities.Add(star);
        }

        public void randomizedActions()
        {
            Random generator = new Random();
            if (generator.Next(0, frequencyRandomizedActions) == 0)
                addHeart();
        }
        
        public void respPlayer(Player player)
        {
            Random generator = new Random();
            do
            {
                Vector2 position = new Vector2(generator.Next(0, (int)Size.X), generator.Next(0, (int)Size.Y));
                player.position = position;
            }
            while (!player.isFree());
        }
        public void addHeart()
        {
            if (Heart.count > maxHealth)
                return;
            Random generator = new Random();
            Vector2 position = new Vector2(generator.Next(0, (int)Size.X), generator.Next(0, (int)Size.Y));
            Heart heart = new Heart(position, this);
            do
            {
               position = new Vector2(generator.Next(0, (int)Size.X), generator.Next(0, (int)Size.Y));
                heart.position = position;
            } while (!heart.isFree());
            entities.Add(heart);
        }

        public void removeEntity(Entity e)
        {
            entities.Remove(e);
        }

        public Player newPlayer()
        {
            playerCount++;
            Player newPlayer = new Player(Vector2.Zero, this, playerCount);
            respPlayer(newPlayer);
            entities.Add(newPlayer);
            return newPlayer;
        }

        public void addEvent(string type, Vector2 position)
        {
            assets.Add(new DrawableAsset("EVENT", type, position));
        }


    }
}
