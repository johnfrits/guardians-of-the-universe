using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Final_Project
{
    class EvadingShootingEnemy
    {
        public Texture2D enemyTexture;
        public Texture2D enemyBulletTexture;
        public Vector2 enemyPosition;
        float enemySpeed;
        public List<EnemyBullet> listBullet = new List<EnemyBullet>();
        public EnemyBullet enemyBullet;
        private int enemyLife;
        SpriteManager playerPosition;
        float evasionSpeedModifier;
        int evasionRange;
        bool evade = false;

        int shotTimer = 0;
        int timeBetweenShots = 400;
        public int EnemyLife
        {
            get { return enemyLife; }
            set { enemyLife = value; }
        }
        public Vector2 getEnemyPosition { get { return enemyPosition; } }
        public EvadingShootingEnemy(Texture2D enemyTexture, Vector2 enemyPosition, Texture2D enemyBulletTexture, float enemySpeed, int enemyLife, SpriteManager spriteManager, float evasionSpeedModifier, int evasionRange)
        {
            this.enemyTexture = enemyTexture;
            this.enemyPosition = enemyPosition;
            this.enemyBulletTexture = enemyBulletTexture;
            this.enemySpeed = enemySpeed;
            this.enemyLife = enemyLife;
            this.playerPosition = spriteManager;
            this.evasionSpeedModifier = evasionSpeedModifier;
            this.evasionRange = evasionRange;
        }
        public void Update(GameTime gameTime)
        {
            Vector2 player = playerPosition.GetPlayerPosition();

           if(evade)
           {
               if (player.X < enemyPosition.X)
                   enemyPosition.X += Math.Abs(enemySpeed);
               else if (player.X > enemyPosition.X)
                   enemyPosition.X -= Math.Abs(enemySpeed);
           }
           else
           {
               if(Vector2.Distance(enemyPosition, player) < evasionRange)
               {
                   enemySpeed *= -evasionSpeedModifier;
                   evade = true;
               }
           }
            enemyPosition.Y += enemySpeed;

            EnemyShoot(gameTime);

            foreach (EnemyBullet eb in listBullet)
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
                enemyBullet.Position = new Vector2((enemyPosition.X + 18) + enemyTexture.Width / 2 - enemyBullet.DrawTexture.Width / 2, this.enemyPosition.Y + 45);
                listBullet.Add(enemyBullet);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, enemyPosition, null, Color.White, 0, Vector2.Zero, .8f, SpriteEffects.None, .7f);

            foreach (EnemyBullet eb in listBullet)
            {
                eb.Draw(spriteBatch);
            }
        }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)enemyPosition.X + 50,
                    (int)enemyPosition.Y + 50,
                    (int)((enemyTexture.Width - (50 * 2) * .2f)),
                    (int)((enemyTexture.Height - (50 * 2) * .2f)));
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
