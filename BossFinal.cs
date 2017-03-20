using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Final_Project
{
    class BossFinal
    {
        public Texture2D bossTexture;
        public Texture2D enemyBulletTexture;
        public Vector2 bossPosition;
        float bossSpeed;
        public List<EnemyBullet> listBullet = new List<EnemyBullet>();
        public EnemyBullet enemyBullet;
        int shotTimer = 0;
        int timeBetweenShots = 600;
        private int bossLife;
        public Texture2D lifeBarTexture;
        public Rectangle lifeBarRectangle;
        public Vector2 lifeBarPosition;
        public int EnemyLife
        {
            get { return bossLife; }
            set { bossLife = value; }
        }
        public SpriteManager spriteManager;
        public Vector2 getEnemyPosition { get { return bossPosition; } }
        public BossFinal(Texture2D bossTexture, Vector2 bossPosition, Texture2D enemyBulletTexture, float bossSpeed, int bossLife, SpriteManager sp, Texture2D lifeBarTexture, Vector2 lifeBarPosition)
        {
            this.bossTexture = bossTexture;
            this.bossPosition = bossPosition;
            this.enemyBulletTexture = enemyBulletTexture;
            this.bossSpeed = bossSpeed;
            this.bossLife = bossLife;
            this.spriteManager = sp;
            this.lifeBarTexture = lifeBarTexture;
            this.lifeBarPosition = lifeBarPosition;
        }
        bool b = true;
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {

            Vector2 player = spriteManager.GetPlayerPosition();

            float speedVal = Math.Max(Math.Abs(bossSpeed), Math.Abs(bossSpeed));

            if(b)
            { 
            if (player.X - 100< bossPosition.X)
                bossPosition.X -= speedVal;
            else if (player.X - 125 > bossPosition.X)
                bossPosition.X += speedVal;
            }
            lifeBarRectangle = new Rectangle((int)lifeBarPosition.X, ((int)lifeBarPosition.Y), bossLife, 30);
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
                enemyBullet.Position = new Vector2((bossPosition.X - 35) + bossTexture.Width / 2 - enemyBullet.DrawTexture.Width / 2, this.bossPosition.Y + 100);
                listBullet.Add(enemyBullet);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossTexture, bossPosition, null, Color.White, 0, Vector2.Zero, .7f, SpriteEffects.None, .5f);

            foreach (EnemyBullet eb in listBullet)  
            {
                eb.Draw(spriteBatch);
            }
            spriteBatch.Draw(lifeBarTexture, lifeBarRectangle, Color.White);
        }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)bossPosition.X + 100,
                    (int)bossPosition.Y + 200,
                    (int)((bossTexture.Width - (130 * 2) * 1f)),
                    (int)((bossTexture.Height - (200 * 2) * 1f)));
            }
        }

    }
}
