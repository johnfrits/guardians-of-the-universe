using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final_Project
{
    class UserControlledSprite : SpriteConstructor
    {
        Vector2 inputDirection;

        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize,
                   int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, float scale, Texture2D bulletTexture, float rotatiozn)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, null, 0, scale, 0, null)
        {
            position = Vector2.Zero;
        }
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame, null, 0, null)
        {
        }

      
        public override Vector2 direction
        {
            get
            {
                inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    inputDirection.X += -1;
                 if (Keyboard.GetState().IsKeyDown(Keys.D))
                    inputDirection.X += 1;
                 if (Keyboard.GetState().IsKeyDown(Keys.W))
                     inputDirection.Y += -1;
                 if (Keyboard.GetState().IsKeyDown(Keys.S))
                     inputDirection.Y += 1;
             
                return inputDirection * speed;
            }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;


            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - (frameSize.X + 25))
                position.X = clientBounds.Width - (frameSize.X + 25);
            if (position.Y > clientBounds.Height - (frameSize.Y + 25))
                position.Y = clientBounds.Height - (frameSize.Y + 25);
            base.Update(gameTime, clientBounds);
        }
    }
}
