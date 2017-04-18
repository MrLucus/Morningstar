using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Morningstar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Views.Assets
{
    public class ScoreAsset:Asset
    {

        public string nick;
        public int points;
        public int count;
        public int kills;
        public int deads;

        public ScoreAsset(string header, string nick, int points, int count, int kills, int deads):base(header)
        {
            this.nick = nick;
            this.points = points;
            this.count = count;
            this.kills = kills;
            this.deads = deads;
        }

        public ScoreAsset(ScoreAsset other) : base(other)
        {
            this.nick = other.nick;
            this.points = other.points;
            this.count = other.count;
            this.kills = other.kills;
            this.deads = other.deads;
        }
        override
        public void draw(ContentManager content, SpriteBatch s)
        {

            s.DrawString(content.Load<SpriteFont>("scoreboard"),"Score "+ nick +"  "+ points+"   "+kills +"   "+deads, new Vector2(20,10+20*count), Color.Red);
        }
    }
}
