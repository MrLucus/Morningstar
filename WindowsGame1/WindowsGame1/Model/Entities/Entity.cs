using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Morningstar.Views;
using Morningstar.Views.Assets;
using System;
using System.Linq;

namespace Morningstar.Model.Entities
{
    public abstract class Entity
    {
        public enum EntityType {Player, Bullet}

        int radius;
        protected int respingCount = 0;
        public Vector2 position;
        public String type;
        public int velocity { get; set; }

        protected World world;

        protected Entity(Vector2 newPosition, int r, int newVelocity, World w)
        {
            radius = r;
            velocity = newVelocity;
            position = newPosition;
            world = w;
        }
        public bool isShowing()
        {
            if (((int)respingCount / 10) % 2 == 0)
                return false;
            return true;
        }
        public bool isResping()
        {
            if (respingCount > 0)
                return true;
            return false;
        }

        public Vector2 returnCenter()
        {
            return new Vector2(position.X + radius, position.Y + radius);
        }

        public int returnRadius() { return radius; }

        public virtual DrawableAsset toAsset()
        {
            if (!isShowing())
                return new DrawableAsset("ENTITY", type, position);
            else
                return null;
        }

        public virtual void update() { }

        public virtual Entity checkCollision(Entity excluded)
        {
            for (int i = 0; i < world.entities.Count(); i++)
            {
                if (!world.entities[i].type.Contains("Player")) continue;
                if (world.entities[i] == excluded) continue;
                Vector2 center_1 = this.returnCenter();
                Vector2 center_2 = world.entities[i].returnCenter();

                //czy odleglosc miedzy srodkami jest dluzsza niz suma ich promieni ///// krótsza???
                double distance = Math.Sqrt(Math.Pow((center_2.X - center_1.X), 2) + Math.Pow((center_2.Y - center_1.Y), 2));
                float radiusSum = world.entities[i].returnRadius() + this.returnRadius();
                if (distance < radiusSum) return world.entities[i];

            }

            return null;
        }
        public bool isFree()
        {
            for (int i = 0; i < world.entities.Count(); i++)
            {
                if (world.entities[i] == this) continue;
                Vector2 center_1 = this.returnCenter();
                Vector2 center_2 = world.entities[i].returnCenter();

                //czy odleglosc miedzy srodkami jest dluzsza niz suma ich promieni ///// krótsza???
                double distance = Math.Sqrt(Math.Pow((center_2.X - center_1.X), 2) + Math.Pow((center_2.Y - center_1.Y), 2));
                float radiusSum = world.entities[i].returnRadius() + this.returnRadius();
                if (distance < radiusSum) return false;

            }
            return true;
        }

        public abstract bool getHit();
    }
}
