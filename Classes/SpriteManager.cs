using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Final_Project.Classes;
using Final_Project.AIClass;



namespace Final_Project
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        Turret turret;
        Vector2 mousePosition;
        List<SpriteConstructor> spriteList = new List<SpriteConstructor>();
        List<AutomatedSprites> livesList = new List<AutomatedSprites>();
        List<Bullet> bullets = new List<Bullet>();
        Bullet b;
        SpriteConstructor s;
        int timeBetweenShots = 200; 
        int shotTimer = 0;
        Texture2D textureBullet;
        List<Explosion> explosionList = new List<Explosion>();
        Explosion ex;
        //
        List<ShootingEnemy> enemies = new List<ShootingEnemy>();
        Texture2D enemyTexture;
        ShootingEnemy enemy;

        int enemySpawnMinMilliseconds = 500,
            enemySpawnMaxMilliseconds = 1000,
            enemyMinSpeed = 2,
            enemyMaxSpeed = 6;
        int nextSpawnTime = 0,
        likelihoodAutomated = 75,
        likelihoodChasing = 5,
        likelihoodEvading = 20,
        automatedSpritePointValue = 10,
        chasingSpritePointValue = 20,
        evadingSpritePointValue = 0,
        nextSpawnTimeChange = 5000,
        timeSinceLastSpawnTimeChange = 0,
        powerUpExpiration = 0;

        int offset;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

        }
   
        protected void AdjustSpawnTimes(GameTime gameTime)
        {
            if (enemySpawnMaxMilliseconds > 500)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;

                    if (enemySpawnMaxMilliseconds > 1000)
                    {
                        enemySpawnMaxMilliseconds += -100;
                        enemySpawnMinMilliseconds += -100;
                    }
                    else
                    {
                        enemySpawnMaxMilliseconds += 10;
                        enemySpawnMinMilliseconds += 10;

                    }
                }
            }
        }
        protected void CheckPowerUpExpiration(GameTime gameTime)
        {
            if (powerUpExpiration > 0)
            {
                powerUpExpiration -= gameTime.ElapsedGameTime.Milliseconds;

                if (powerUpExpiration <= 0)
                {
                    powerUpExpiration = 0;
                    player.ResetSpeed();
                }
            }
        }
        private void ResetSpawnTime()
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(enemySpawnMinMilliseconds, enemySpawnMaxMilliseconds);
        }
        private void SpawnEnemy()
        {

            Vector2 speed = Vector2.Zero,
                 position = Vector2.Zero;
            Point frameSize = new Point(75, 75);
            //float random speed
            float speedF = 1;

            switch (((Game1)Game).rnd.Next(4))
            {
                case 0:
                case 1:
                case 2:
                case 3:// TOP to BOTTOM            
                    position = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    speed = new Vector2(0, ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
                    speedF = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
                    break;
            }


            if (((Game1)Game).IsEnemiesActive == true)
            {
                int random = ((Game1)Game).rnd.Next(100);
                if (random < likelihoodAutomated)
                {

                    if (((Game1)Game).rnd.Next(2) == 0)
                    {
                        spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"images\fourblades"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                    }
                    else
                    {
                        spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"images\threeblade"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                    }

                }
                else if (random < likelihoodAutomated + likelihoodEvading)
                {
                    if (((Game1)Game).rnd.Next(2) == 0)
                    {
                        spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\bolt"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 4), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                    }
                    else
                    {
                        enemies.Add(new ShootingEnemy(enemyTexture, position, textureBullet, speedF, 40));
                    }
                }
                else
                {
                    spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed  , "explosion", this, evadingSpritePointValue, .8f, "addarmore"));
                }
            }
            else spriteList.Clear();
        }
        public override void Initialize()
        {
            ResetSpawnTime();
            base.Initialize();
        }
        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            textureBullet = Game.Content.Load<Texture2D>(@"Images\particlef");
            enemyTexture = Game.Content.Load<Texture2D>(@"Images\enemy2");
            //usercontrolled
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images\threerings"),
            new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height - 120), new Point(75, 75), 10, new Point(0, 0), new Point(0, 0), new Vector2(6, 6), .01f, Game.Content.Load<Texture2D>(@"Images\bullet"), 0f);
            turret = new Turret(Game.Content.Load<Texture2D>(@"Images\player"), new Vector2(400, 400),
                       Vector2.Zero, 0f, 0f, this);


            base.LoadContent();
        }
        private void Explosions(Vector2 position)
        {
            ex = new Explosion(Game.Content.Load<Texture2D>(@"Images\explosionsprite"), position, Vector2.Zero, 1, 0);
            explosionList.Add(ex);
        }
        private void AddLife()
        {
            if (((Game1)Game).GameOver == false)
            {
                for (int i = 0; i < ((Game1)Game).NumberLivesRemaining; ++i)
                {
                    offset = 10 + i * 40;
                    livesList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\threerings"),
                        new Vector2(offset, 35), new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), Vector2.Zero, null, 0, .5f, null));
                }
                ((Game1)Game).gameOver = true;
            }
        }
        private void UpdatedEnemy(GameTime gameTime)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                enemy = enemies[i];
                enemy.Update(gameTime);

                if (enemy.collisionRect.Intersects(player.collisionRect))
                {
                    Explosions(enemies[i].getEnemyPosition);
                    ((Game1)Game).PlayCue("explosion");
                    if (enemy is ShootingEnemy)
                    {
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --((Game1)Game).NumberLivesRemaining;
                        }
                    }
                    enemies.RemoveAt(i);
                    --i;
                }
                if (enemy.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    enemies.RemoveAt(i);
                    --i;
                }
            }
        }
        protected void UpdatedSprites(GameTime gametime)
        {
            AddLife();
            UpdatedEnemy(gametime);
            player.Update(gametime, Game.Window.ClientBounds);
            MouseState mouse = Mouse.GetState();
            mousePosition = new Vector2(mouse.X, mouse.Y);

            turret.Direction = GetDirection(turret.Position, mousePosition);
            turret.Rotation = GetRotation(turret.Direction);
            turret.Update(gametime);

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                shotTimer += gametime.ElapsedGameTime.Milliseconds;
             
                if (shotTimer > timeBetweenShots)
                {
                    shotTimer = 0;

                    b = new Bullet(
                    textureBullet,
                    turret.Position,
                    turret.Direction,
                    12,
                    2000);

                    bullets.Add(b);
                    ((Game1)Game).PlayCue("gunshot");
                }
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                try
                {
                    b = bullets[i];
                    b.Update(gametime);
                    for (int x = 0; x < spriteList.Count; x++)
                    {

                        if (b.collisionRect.Intersects(spriteList[x].collisionRect))
                        {
                            spriteList[x].Enemylife -= 10;
                            bullets.RemoveAt(i);
                            --i;
                            if (spriteList[x].Enemylife <= 0)
                            {
                                Explosions(spriteList[x].GetPosition);
                                ((Game1)Game).PlayCue(s.collisionCueName);
                                spriteList.RemoveAt(x);
                                --x;
                                ((Game1)Game).AddScore(2);

                            }

                        }
                    }
                    for (int es = 0; es < enemies.Count; es++)
                    {
                        
                        if (b.collisionRect.Intersects(enemies[es].collisionRect))
                        {
                            enemies[es].EnemyLife -= 10;
                            bullets.RemoveAt(i);
                            --i;
                            if (enemies[es].EnemyLife <= 0)
                            {
                                Explosions(enemies[es].getEnemyPosition);
                                ((Game1)Game).PlayCue("explosion");
                                enemies.RemoveAt(es);
                                --es;
                                ((Game1)Game).AddScore(5);
                            }
                        }
                    }
                    if (b.IsOutOfBounds(Game.Window.ClientBounds))
                    {
                        bullets.RemoveAt(i);
                        --i;
                    }
                }
                catch { return; }
            }

            for (int i = 0; i < spriteList.Count; i++)
            {
                s = spriteList[i];
                s.Update(gametime, Game.Window.ClientBounds);
                if (s.collisionRect.Intersects(player.collisionRect) && spriteList[i].collisionName == "automated")
                {
                    Explosions(spriteList[i].GetPosition);
                    if (s.collisionCueName != null)
                        ((Game1)Game).PlayCue(s.collisionCueName);
                    if (s is AutomatedSprites)
                    {
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --((Game1)Game).NumberLivesRemaining;
                        }
                    }
                    spriteList.RemoveAt(i);
                    --i;
                }
                else
                {
                    if (s.collisionRect.Intersects(player.collisionRect))
                    {
                        spriteList.RemoveAt(i);
                        --i;

                        if (s.collisionName == "chase")
                        {
                            powerUpExpiration = 2000;
                            player.ModifySpeed(1.5f);
                        }
                    }
                }

                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    spriteList.RemoveAt(i);
                    --i;
                }
            }
            if (((Game1)Game).IsEnemiesActive == false)
            {
                spriteList.Clear();
                bullets.Clear();
                explosionList.Clear();
            }


            foreach (SpriteConstructor sprite in livesList)
                sprite.Update(gametime, Game.Window.ClientBounds);
            foreach (Explosion explode in explosionList)
                explode.Update(gametime);
        }
        public override void Update(GameTime gameTime)
        {
            nextSpawnTime += -gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnEnemy();
                ResetSpawnTime();
            }
            AdjustSpawnTimes(gameTime);
            UpdatedSprites(gameTime);
            CheckPowerUpExpiration(gameTime);
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            turret.Draw(spriteBatch);
            player.Draw(gameTime, spriteBatch);

            //DRAW AUTOMATED ENEMIES
            foreach (SpriteConstructor s in spriteList)
                s.Draw(gameTime, spriteBatch);
            //DRAW SHOOTING ENEMIES
            foreach(ShootingEnemy se in enemies)
                se.Draw(spriteBatch);
            //DRAW LIFE
            foreach (SpriteConstructor sprite in livesList)
                sprite.Draw(gameTime, spriteBatch);
            //DRAW BULLETS
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);
            //DRAW EXPLOSIONS
            foreach (Explosion e in explosionList)
                e.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        //BULLET DIRECTION
        private Vector2 GetDirection(Vector2 positionOfObject, Vector2 directionToPointAt)
        {
            Vector2 direction = directionToPointAt - positionOfObject;
            direction.Normalize();
            return direction;
        }
        private float GetRotation(Vector2 direction)
        {
            return (float)Math.Atan2((float)direction.Y, (float)direction.X);
        }
    }
}
