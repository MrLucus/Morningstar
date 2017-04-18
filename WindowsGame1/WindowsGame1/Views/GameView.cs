using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Morningstar.Views.Assets;
using Morningstar.Views.Animations;
using System;
using WindowsGame1.Views.Assets;

namespace Morningstar.Views
{
    public class GameView : BasicView
    {
        public List<Asset> assets { get; set; }
        public List<AnimatedEffect> animations;

        float timeSinceUpdate;
        public bool showScores {get;set; }

        public GameView(SpriteBatch s, ContentManager c, Controller cl) : base(s, c, cl)
        {
            backgroundImage = c.Load<Texture2D>("gameviewBackground");
            assets = new List<Asset>();
            animations = new List<AnimatedEffect>();
            controller = cl;
        }

        public override void update(GameTime time) { }

        public override void draw()
        {
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height), Color.White);
            List<ScoreAsset> scoreAssets = new List<ScoreAsset>();
            foreach (var asset in assets)
            {
                if (asset is ScoreAsset)
                {
                    if (showScores == true)
                        scoreAssets.Add(asset as ScoreAsset);
                    continue;
                }
                asset.draw(content, spriteBatch);
            }
            foreach (var asset in scoreAssets)
            {
                asset.draw(content, spriteBatch);
            }
          
            AnimatedEffect[] anims = animations.ToArray();
            for (int i = 0; i < anims.Length; i++) anims[i].draw(content, spriteBatch);
        }

        public void stopAnimation(AnimatedEffect e)
        {
            animations.Remove(e);
        }

        public void addAnimation(DrawableAsset asset)
        {
            List<AnimatedEffect> addedAnimations = new List<AnimatedEffect>();


            switch(asset.type)
            {
                case "HIT":
                    addedAnimations.Add(new BloodAnimation(asset.position, content, this)) ;
                    addedAnimations.Add(new MessageAnimation("HIT_MESSAGE", asset.position, content, this));                    
                    break;

                case "DEATH":
                    addedAnimations.Add(new DeathAnimation(asset.position, content, this));
                    addedAnimations.Add(new MessageAnimation("DEATH_MESSAGE", asset.position, content, this));
                    break;
            }

            animations.AddRange(addedAnimations);         
        }

        public override void setActivityButtons(bool activity)
        {
            throw new NotImplementedException();
        }
    }
}
