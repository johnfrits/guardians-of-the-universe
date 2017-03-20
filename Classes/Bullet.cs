using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    public class Bullet
    {
        public Texture2D DrawTexture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public int ActiveTime { get; set; }
        public int TotalActiveTime { get; set; }

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, float speed, int activeTime)
        {
            this.DrawTexture = texture;
            this.Position = position;
            this.Direction = direction;
            this.Speed = speed;
            this.ActiveTime = activeTime;

            this.TotalActiveTime = 0;
        }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)Position.X + 10,
                    (int)Position.Y + 10,
                    (int)((DrawTexture.Width - (10 * 2) * 1f)),
                    (int)((DrawTexture.Height - (10 * 2) *1f)));
            }
        }
        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (Position.X < -DrawTexture.Width ||
                Position.X > clientRect.Width ||
                Position.Y < -DrawTexture.Height||
                Position.Y > clientRect.Height)
            {
                return true;
            }
            return false;
        }
        public void Update(GameTime gameTime)
        {
            this.Position += Direction * Speed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                 DrawTexture,
                 Position,
                 null,
                 Color.White,
                 0f,
                 new Vector2(
                      DrawTexture.Width / 2,
                      DrawTexture.Height / 2),
                 .5f,
                 SpriteEffects.None,
                 0f);
        }
    }
}
