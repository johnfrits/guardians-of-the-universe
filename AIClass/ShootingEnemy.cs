using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Final_Project.AIClass
{
    class ShootingEnemy
    {
        public Texture2D enemyTexture;
        public Texture2D enemyBulletTexture;
        public Vector2 enemyPosition;
        float enemySpeed;

        private int enemyLife;
        public int EnemyLife
        {
            get { return enemyLife; }
            set { enemyLife = value; }
        }
        public Vector2 getEnemyPosition { get { return enemyPosition; } }
        public ShootingEnemy(Texture2D enemyTexture, Vector2 enemyPosition, Texture2D enemyBulletTexture, float enemySpeed, int enemyLife)
        {
            this.enemyTexture = enemyTexture;
            this.enemyPosition = enemyPosition;
            this.enemyBulletTexture = enemyBulletTexture;
            this.enemySpeed = enemySpeed;
            this.enemyLife = enemyLife;
        }
        public void Update(GameTime gameTime)
        {
            enemyPosition.Y += enemySpeed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, enemyPosition, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
        }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)enemyPosition.X + 10,
                    (int)enemyPosition.Y + 10,
                    (int)((enemyTexture.Width - (10 * 2) * 1f)),
                    (int)((enemyTexture.Height - (10 * 2) * 1f)));
            }
        }
        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (enemyPosition.X < -enemyTexture.Width ||
                enemyPosition.X > clientRect.Width ||
                enemyPosition.Y < -enemyTexture.Height ||
                enemyPosition.Y > clientRect.Height)
            {
                return true;
            }
            return false;
        }

    }
}
