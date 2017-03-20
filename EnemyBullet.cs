using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Final_Project
{
    class EnemyBullet
    {
        public Texture2D DrawTexture;
        public Vector2 Position;
        public float Speed;

        public Vector2 getCurrentBulletPosition { get { return Position; } }
        public EnemyBullet(Texture2D texture)
        {
            this.DrawTexture = texture;
            this.Speed = 10;
        }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)Position.X + 10,
                    (int)Position.Y + 10,
                    (int)((DrawTexture.Width - (10 * 2) * 1f)),
                    (int)((DrawTexture.Height - (10 * 2) * 1f)));
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
