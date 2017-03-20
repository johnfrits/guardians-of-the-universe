using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    class BouncingSprites : SpriteConstructor
    {
         public BouncingSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, float scale, string collisionName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, collisionCueName, 0, scale, 20, collisionName)
        {
        }
        public BouncingSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName, string collisionName)
             : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame, collisionCueName, 0, collisionName)
        {
        }
        public override Vector2 direction { get { return speed; } }


        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;

            if (position.Y > clientBounds.Height - frameSize.Y || position.Y < 0)
                speed.Y *= -1;
            if (position.X > clientBounds.Width - frameSize.X || position.X < 0)
                speed.X *= -1;

            base.Update(gameTime, clientBounds);
        }
    }
}
