using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Final_Project
{
    class ShootingEnemy
    {
        public Texture2D enemyTexture;
        public Texture2D enemyBulletTexture;
        public Vector2 enemyPosition;
        float enemySpeed;
        public List<EnemyBullet> listBullet = new List<EnemyBullet>();
        public EnemyBullet enemyBullet;
        private int enemyLife;

        int shotTimer = 0;
        int timeBetweenShots = 500;
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
            EnemyShoot(gameTime);
            foreach(EnemyBullet eb in listBullet)
            {
                eb.Position.Y = eb.Position.Y + eb.Speed;
            }
        }
        private void EnemyShoot(GameTime gameTime)
        {
            shotTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (shotTimer > timeBetweenShots)
            {
                shotTimer = 0;
                enemyBullet = new EnemyBullet(this.enemyBulletTexture);
                enemyBullet.Position = new Vector2((enemyPosition.X + 20) + enemyTexture.Width / 2 - enemyBullet.DrawTexture.Width / 2, this.enemyPosition.Y + 50);
                listBullet.Add(enemyBullet);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, enemyPosition, null, Color.White, 0, Vector2.Zero,.8f, SpriteEffects.None, 1f);

            foreach(EnemyBullet eb in listBullet)
            {
                eb.Draw(spriteBatch);
            }
        }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)enemyPosition.X + 30,
                    (int)enemyPosition.Y + 30,
                    (int)((enemyTexture.Width - (30 * 2) * .5f)),
                    (int)((enemyTexture.Height - (30 * 2) * .5f)));
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
