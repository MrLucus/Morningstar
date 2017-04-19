using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Morningstar.Views.Assets;
using System;

namespace Morningstar.Model.Entities
{
    class Bullet : Entity
    {
        const int bulletRadius = 10;

        const int basicVelocity = 3;
        const int range = 500;


        const int pointsForHit = 10;
        const int pointsForKill = 100;
        public bool wasSended;
        string name = "krotki";
        Vector2 direction;
        Player shooter;

        //Pocisk przekazuje do klasy bazowej wspolrzedne srodka gracza, ktory go wystrzelil
        //potem odejmujemy polowe wysokosci/szerokosci zeby srodek kuli pokrywal sie ze srodkiem gracza
        public ListenableAsset shootToAsset()
        {
            return new ListenableAsset("SOUNDS", name);
        }
        public Bullet(Player source, Vector2 target, World w) : base(source.returnCenter(), bulletRadius, basicVelocity, w)
        {
            shooter = source;
            type = "Bullet";
            wasSended = false;
            //odejmowane sa wektory: pozycja gracza ktory wystrzelil i myszki w chwili wystrzelenia
            //mamy roznice - czyli jezeli dodamy ja do pozycji gracza to otrzymamy pozycje myszki (...)
            //i teraz normalizujemy tj. skalujemy do 1
            //no i mamy ile musimy dodac w danej jednostce czasu do wektora poczatkowego zeby kiedys tam przeciac pozycje myszki (...)
            Vector2 dest = new Vector2(target.X, target.Y);
            Vector2 from = new Vector2((int)source.returnCenter().X, (int)source.returnCenter().Y);
            direction = (dest - from);
            direction.Normalize();

            //przesuniecie od srodka
            target += shooter.returnRadius() * direction;
            direction *= velocity;

            //rotationAngle = 0;
        }

        //podczas kazdego odswiezenia pocisk sie przesuwa po swojej krzywej
        //i sprawdza czy w nic nie uderzyl
        public override void update()
        {
            double distance = Vector2.Distance(position, shooter.position);
            if (distance > range)
            {
                world.removeEntity(this);
                return;
            }
            position += direction;
            base.update();
            checkCollision(shooter);
        }

        public override Entity checkCollision(Entity excluded)
        {
            Entity obstacle = base.checkCollision(excluded);
            if (obstacle != null)
            {
                if(obstacle.isResping())
                {
                    return null;
                }
                shooter.addPoints(pointsForHit);
                if (obstacle.getHit() == true)
                {
                    shooter.addPoints(pointsForKill);
                    shooter.addKill();
                }

                world.entities.Remove(this);

            }

            return null;
        }

        //klasa Bullet nie implementuje ciala metody getHit - pocisk nigdy nie bedzie uderzony.
        public override bool getHit()
        {
            throw new NotImplementedException();
        }

        /*
         * TODO: przeniesc po strone klienta, do assets
        public override void draw(SpriteBatch s)
        {
            Vector2 origin = new Vector2();
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            rotationAngle += 0.1f;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;

            s.Draw(texture, returnCenter(), null, Color.White, rotationAngle,
                   origin, 1f, SpriteEffects.None, 0f);
        }
        */

    }
}
