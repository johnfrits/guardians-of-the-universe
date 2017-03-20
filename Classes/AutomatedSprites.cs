using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    class AutomatedSprites : SpriteConstructor
    {
        public AutomatedSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue, float scale, string collisionName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, collisionCueName, scoreValue, scale, 20,collisionName)
        {
        }
        public AutomatedSprites(Texture2D textureImage, Vector2 position, Point frameSize,
                    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName, int scoreValue, string collisionName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame, collisionCueName, scoreValue, collisionName)
        {
        }
        public override Vector2 direction { get { return speed; } }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;
            base.Update(gameTime, clientBounds);
        }


    }
}
