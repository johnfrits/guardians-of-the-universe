using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    public class Explosion
    {
        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public Vector2 direction { get; set; }
        public float scale { get; set; }
        public int milliSecondsPerFrame { get; set; }
        Point framesize = new Point(96, 96);
        Point currentFrame = new Point(0, 0);
        Point sheeetSize = new Point(5, 4);

        int timeSinceLastFrame = 0;


        public Explosion(Texture2D texture, Vector2 position, Vector2 direction, float scale, int milliSecondsPerFrame)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.scale = scale;
            this.milliSecondsPerFrame = milliSecondsPerFrame;
        }
        public virtual void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > milliSecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X > sheeetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                             position,
                             new Rectangle(currentFrame.X * framesize.X, currentFrame.Y * framesize.Y,
                                           framesize.X, framesize.Y),
                                           Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

    }
}
