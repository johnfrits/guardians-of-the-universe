using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    class ChasingSprites : SpriteConstructor
    {
        SpriteManager spriteManager;
        public ChasingSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, SpriteManager spriteManager, int scoreValue, float scale,string collisionName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, collisionCueName, scoreValue, scale, 20, collisionName)
        {
            this.spriteManager = spriteManager;
        }
        public ChasingSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName, SpriteManager spriteManager, int scoreValue, string collisionName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame,collisionCueName, scoreValue, collisionName)
        {
            this.spriteManager = spriteManager;
        }
        public override Vector2 direction
        {
            get { return speed; }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Vector2 player = spriteManager.GetPlayerPosition();

            float speedVal = Math.Max(Math.Abs(speed.X), Math.Abs(speed.Y));

            if (player.X < position.X)
                position.X -= speedVal;
            else if (player.X > position.X)
                position.X += speedVal;

            if (player.Y < position.Y)
                position.Y -= speedVal;
            else if (player.Y > position.Y)
                position.Y += speedVal; 
            base.Update(gameTime, clientBounds);
        }
    }
}
