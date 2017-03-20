using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    class EvadingSprites : SpriteConstructor
    {
        SpriteManager spriteManager;
        float evasionSpeedModifier; 
        int evasionRange; 
        bool evade = false; 
        public EvadingSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, SpriteManager spriteManager, float evasionSpeedModifier,int evasionRange, int scoreValue, float scale,string collisionName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, collisionCueName, scoreValue, scale, 20, collisionName)
        {
            this.spriteManager = spriteManager;
            this.evasionSpeedModifier = evasionSpeedModifier; 
            this.evasionRange = evasionRange; 
        }
        public EvadingSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName, SpriteManager spriteManager, float evasionSpeedModifier, int evasionRange, int scoreValue, float scale, string collisionName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, collisionCueName, scoreValue, scale, 20, collisionName)
        {
            this.spriteManager = spriteManager;
            this.evasionSpeedModifier = evasionSpeedModifier;
            this.evasionRange = evasionRange;
        }
        public override Vector2 direction
        {
            get { return speed; }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += speed;

            Vector2 player = spriteManager.GetPlayerPosition();
            if (evade)
            {
                if (player.X < position.X)
                    position.X += Math.Abs(speed.Y);
                else if (player.X > position.X)
                    position.X -= Math.Abs(speed.Y);


                if (player.Y < position.Y)
                    position.Y += Math.Abs(speed.X);
                else if (player.Y > position.Y)
                    position.Y -= Math.Abs(speed.X);
            }
            else
            {
                if (Vector2.Distance(position, player) < evasionRange)
                {
                    // Player is within evasion range,   
                    // reverse direction and modify speed        
                    speed *= -evasionSpeedModifier;
                    evade = true;
                }
            }


            base.Update(gameTime, clientBounds);
        }
    }
}
