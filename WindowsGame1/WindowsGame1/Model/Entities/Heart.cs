using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using WindowsGame1.Views.Assets;
using Morningstar.Views.Assets;

namespace Morningstar.Model.Entities
{
    public class Heart : Entity
    {

        const int playerRadius = 16;
        const int basicVelocity = 0;

        public static int count = 0;

        public Heart(Vector2 position, World w) : base(position, playerRadius, basicVelocity, w)
        {
            type = "Heart";
            world = w;
            count++;
        }

        public override Entity checkCollision(Entity excluded)
        {

            foreach (var entity in world.entities)
            {
                if (entity == this)
                    continue;
                if (!entity.type.Contains("Player")) continue;

                Vector2 center_1 = this.returnCenter();
                Vector2 center_2 = entity.returnCenter();

                //czy odleglosc miedzy srodkami jest dluzsza niz suma ich promieni ///// krótsza???
                double distance = Math.Sqrt(Math.Pow((center_2.X - center_1.X), 2) + Math.Pow((center_2.Y - center_1.Y), 2));
                float radiusSum = entity.returnRadius() + this.returnRadius();
                if (distance < radiusSum) return entity;

            }



            return null;
        }


        public override bool getHit()
        {

            throw new NotImplementedException();
        }

        public override void update()
        {
            Entity entity = checkCollision(this);
            if (entity == null)
                return;
            (entity as Player).addLife();
            world.removeEntity(this);
            count--;
        }
    }
}
