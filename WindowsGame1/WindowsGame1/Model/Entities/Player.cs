using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using WindowsGame1.Views.Assets;
using Morningstar.Views.Assets;

namespace Morningstar.Model.Entities
{
    public class Player : Entity
    {
        public bool isAlive = true;

        const int playerRadius = 16;
        const int basicVelocity = 3;

        private int healthBars = 6;

        public void addLife()
        {
            healthBars++;
            if (healthBars > 6)
                healthBars = 6;
        }
        private int kills;
        public void addKill()
        {
            kills++;
        }
        private int deads;
        public void addDead()
        {
            deads++;
            healthBars = 6;
            respingCount = 200;
        }

        private const int bulletsMax = 4;
        private int bulletsLeft;

        private float shootCooldown;
        private float reloadTime = 0.3f;

        private string nick;

        public string Nick()
        {
            return nick;
        }
        private int points { get; set; }

        public void addPoints(int value)
        {
            points += value;
        }
        public int Points()
        {
            return points;
        }
       
        public ScoreAsset toScoreAsset(int id)
        {
            return new ScoreAsset("SCORE", nick, points, id, kills, deads);
        }



        public Player(Vector2 position, World w, int id) : base(position, playerRadius, basicVelocity, w)
        {
            bulletsLeft = bulletsMax;
            type = $"Player{id}";
            nick = "rozpierdalacz";
            world = w;
            points = 0;
        }


        public void move(Vector2 potentialMov)
        {
            Vector2 potentialMovement = potentialMov;
            potentialMovement *= velocity;
            position += potentialMovement;
            Entity other = checkCollision(this);
            if (other == null )// jeśli nie ma nikogo
                return;
            position -= potentialMovement;
            potentialMovement = potentialMov;
            Vector2 relativePosition = checkPositions(other);
            potentialMovement += relativePosition;
            potentialMovement *= velocity;
            position += potentialMovement;
            if (checkCollision(this) != null)
                position -= potentialMovement;
        }

        private Vector2 checkPositions(Entity other)
        {
            Vector2 vector = Vector2.Zero;
            if (this.returnCenter().X > other.returnCenter().X)
                vector.X = 1;
            else if (this.returnCenter().X < other.returnCenter().X)
                vector.X = -1;
            if (this.returnCenter().Y > other.returnCenter().Y)
                vector.Y = 1;
            else if (this.returnCenter().Y < other.returnCenter().Y)
                vector.Y = -1;

            return vector;
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

        public override void update()
        {
            if (respingCount == 0)
                return;
            respingCount--;
        }


        public override bool getHit()
        {
            
            world.addEvent("HIT", position);
            healthBars--;

            if (healthBars == 0)
            {
                die();
                return true; //Gracz zabity
            }
            return false; 
        }

        public void shoot(Vector2 target)
        {

            if (bulletsLeft > 0)
            {
                world.shot(this, target);
                bulletsLeft--;
                if (bulletsLeft == 0) shootCooldown = reloadTime;
            }
            else
            {
                shootCooldown -= reloadTime;
                if (shootCooldown < 0) bulletsLeft = bulletsMax;
            }


        }

        private void die()
        {
            world.addEvent("DEATH", position);
            addDead();
            world.respPlayer(this);
        }

        public DrawableAsset healthToAsset()
        {
            string name = $"hp{healthBars}";
            Vector2 pos = position - new Vector2(playerRadius-10, playerRadius*0.65f);
            if (!isShowing())
                return new DrawableAsset("ENTITY", name, pos);
            else
                return null;
        }
    }
}
