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
using System.IO;
using Microsoft.Xna.Framework.Storage;



namespace Final_Project
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<SpriteConstructor> spriteList = new List<SpriteConstructor>();
        List<AutomatedSprites> livesList = new List<AutomatedSprites>();
        List<Bullet> bullets = new List<Bullet>();
        List<Explosion> explosionList = new List<Explosion>();
        List<ShootingEnemy> shootingEnemiesList = new List<ShootingEnemy>();
        List<SuicideShootingEnemy> suicideEnemyList = new List<SuicideShootingEnemy>();
        List<EvadingShootingEnemy> evadingEnemyList = new List<EvadingShootingEnemy>();
        List<BossFinal> bossFlist = new List<BossFinal>();
        Bullet b;
        SuicideShootingEnemy suicideEnemy;
        SpriteConstructor s;
        EvadingShootingEnemy evadingSenemy;
        BossFinal bossFinal;
        ShootingEnemy shootingEnemy;
        Turret turret;
        Explosion ex;
        Texture2D textureBullet;
        Texture2D bulletEnemyTexture;
        Texture2D bossBullet;
        Texture2D bossTexture;
        Texture2D enemyTexture1;
        Texture2D enemyTexture;
        int enemySpawnMinMilliseconds = 500,
            enemySpawnMaxMilliseconds = 1000,
            enemyMinSpeed = 2,
            enemyMaxSpeed = 6,
            suicideShootingEnemyMin = 2,
            suicideShootingEnemyMax = 5,
        nextSpawnTime = 0,
        likelihoodAutomated = 75,
        likelihoodEvading = 20,
        automatedSpritePointValue = 10,
        chasingSpritePointValue = 20,
        evadingSpritePointValue = 0,
        nextSpawnTimeChange = 5000,
        timeSinceLastSpawnTimeChange = 0,
        powerUpExpiration = 0,
        timeBetweenShots = 200,
        shotTimer = 0;
        public int offset;
        int GamePlayTime;
        Vector2 playerPosition;
        Vector2 getPPosition;
        public enum WaveState { Wave1, Wave2,Wave3, Wave4, FinalWave }
        public WaveState currentWaveState = WaveState.Wave1;
        private int waveOneGameTime = 50000;
        private int waveTwoGameTime = 50000;
        private int waveThreeGameTime = 60000;
        private int waveFourGameTime = 60000;
        public int spawnBoss = 1;
      
        string waves;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

        }

        protected void AdjustSpawnTimes(GameTime gameTime)
        {
            if (enemySpawnMaxMilliseconds > 400)
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
        private void SpawnBoss()
        {
            if (currentWaveState == WaveState.FinalWave)
            {
                float speed = 2;
                if (spawnBoss == 1)
                {
                    bossFlist.Add(new BossFinal(bossTexture, new Vector2(Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2, 5), bossBullet, speed, 180, this, Game.Content.Load<Texture2D>(@"Images\bossBarLife"), new Vector2(Game.Window.ClientBounds.Width -185, 35)));
                    spawnBoss = 2;
                }
            }
        }
        private void FinalWave()
        {
            if (currentWaveState == WaveState.FinalWave)
            {
                Vector2 speed = Vector2.Zero,
                position = Vector2.Zero,
                 positionF = Vector2.Zero;
                Point frameSize = new Point(75, 75);
                //float random speed
                float speedF = 1;
                float speedS = 1;

                switch (((Game1)Game).rnd.Next(4))
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:// TOP to BOTTOM            
                        position = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X), -frameSize.Y);
                        positionF = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X), -frameSize.Y);
                        speed = new Vector2(0, ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
                        speedF = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
                        speedS = ((Game1)Game).rnd.Next(suicideShootingEnemyMin, suicideShootingEnemyMax);
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
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"images\fourblades"), positionF, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture1, position, bulletEnemyTexture, speedF, 30));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"images\threeblade"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 2)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture1, position, bulletEnemyTexture, speedF, 30));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture1, positionF, bulletEnemyTexture, speedF, 30));
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
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }

                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 2)
                        {
                            evadingEnemyList.Add(new EvadingShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy5"), position, bulletEnemyTexture, speedF, 30, this, .75f, 150));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture1, positionF, bulletEnemyTexture, speedF, 30));
                        }
                        else
                        {
                            evadingEnemyList.Add(new EvadingShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy5"), position, bulletEnemyTexture, speedF, 30, this, .75f, 150));
                        }
                    }
                    else
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, evadingSpritePointValue, .8f, "addLife"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "addLife"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 2)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }
                        else
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }
                    }
                }
                else spriteList.Clear();
            }
        }
        private void WaveFourSpawnEnemy()
        {
            Vector2 speed = Vector2.Zero,
                 position = Vector2.Zero,
                  positionF = Vector2.Zero;
            Point frameSize = new Point(75, 75);
            //float random speed
            float speedF = 1;
            float speedS = 1;

            switch (((Game1)Game).rnd.Next(4))
            {
                case 0:
                case 1:
                case 2:
                case 3:// TOP to BOTTOM            
                    position = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    positionF = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    speed = new Vector2(0, ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
                    speedF = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
                    speedS = ((Game1)Game).rnd.Next(suicideShootingEnemyMin, suicideShootingEnemyMax);
                    break;
            }
            if (currentWaveState == WaveState.Wave4)
            {
                if (((Game1)Game).IsEnemiesActive == true)
                {
                    int random = ((Game1)Game).rnd.Next(100);
                    if (random < likelihoodAutomated)
                    {

                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"images\fourblades"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"images\fourblades"), positionF, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture1, position, bulletEnemyTexture, speedF, 30));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"images\threeblade"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 2)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture1, position, bulletEnemyTexture, speedF, 30));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedS, 40));
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
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }

                        else if(((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 2)
                        {
                            evadingEnemyList.Add(new EvadingShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy5"), positionF, bulletEnemyTexture, speedF, 30, this, .75f, 150));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture1, positionF, bulletEnemyTexture, speedF, 30));
                        }
                        else
                        {
                            evadingEnemyList.Add(new EvadingShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy5"), position, bulletEnemyTexture, speedF, 30, this, .75f, 150));
                        }
                    }
                    else
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, evadingSpritePointValue, .8f, "addLife"));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 1)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "addLife"));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 2)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));  
                        }
                        else
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));  
                        }
                    }
                }
                else spriteList.Clear();
            }
        }
        private void WaveThreeSpawnEnemy()
        {
            Vector2 speed = Vector2.Zero,
                 position = Vector2.Zero,
                  positionF = Vector2.Zero;
            Point frameSize = new Point(75, 75);
            //float random speed
            float speedF = 1;
            float speedS = 1;

            switch (((Game1)Game).rnd.Next(4))
            {
                case 0:
                case 1:
                case 2:
                case 3:// TOP to BOTTOM            
                    position = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    positionF = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    speed = new Vector2(0, ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
                    speedF = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
                    speedS = ((Game1)Game).rnd.Next(suicideShootingEnemyMin, suicideShootingEnemyMax);
                    break;
            }
            if (currentWaveState == WaveState.Wave3)
            {
                if (((Game1)Game).IsEnemiesActive == true)
                {
                    int random = ((Game1)Game).rnd.Next(100);
                    if (random < likelihoodAutomated)
                    {

                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            evadingEnemyList.Add(new EvadingShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy5"), position, bulletEnemyTexture, speedF, 30, this, .75f, 150));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\fourblades"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 2)
                        {
                            evadingEnemyList.Add(new EvadingShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy5"), position, bulletEnemyTexture, speedF, 30, this, .75f, 150));
                        }
                        else
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\threeblade"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }

                    }
                    else if (random < likelihoodAutomated + likelihoodEvading)
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }

                        else if(((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 2)
                        {
                            evadingEnemyList.Add(new EvadingShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy5"), position, bulletEnemyTexture, speedF, 30, this, .75f, 150));
                        }
                        else
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\fourblades"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                    }
                    else
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "addLife"));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 1)
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\bolt"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "speedUp"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 2)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }
                        else
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }
                    }
                }
                else spriteList.Clear();
            }
        }
        private void WaveTwoSpawnEnemy()
        {

            Vector2 speed = Vector2.Zero,
                 position = Vector2.Zero,
                  positionF = Vector2.Zero;
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
                    positionF = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    speed = new Vector2(0, ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
                    speedF = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
                    break;
            }
            if (currentWaveState == WaveState.Wave2)
            {
                if (((Game1)Game).IsEnemiesActive == true)
                {
                    int random = ((Game1)Game).rnd.Next(100);
                    if (random < likelihoodAutomated)
                    {

                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\fourblades"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            suicideEnemyList.Add(new SuicideShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy1"), position, bulletEnemyTexture, speedF, 20, this));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 2)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                        else
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\threeblade"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }

                    }
                    else if (random < likelihoodAutomated + likelihoodEvading)
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            suicideEnemyList.Add(new SuicideShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy1"), position, bulletEnemyTexture, speedF, 20, this));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 2)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            suicideEnemyList.Add(new SuicideShootingEnemy(Game.Content.Load<Texture2D>(@"Images\enemy1"), position, bulletEnemyTexture, speedF, 20, this));
                        }
                        else
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                    }
                    else
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, evadingSpritePointValue, .8f, "addLife"));
                        }
                        else if (((Game1)Game).rnd.Next(1) == 1)
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\addarmor"), position, new Point(0, 0), 10, new Point(0, 0), new Point(0, 0), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                    }
                }
                else spriteList.Clear();
            }
        }
        private void WaveOneSpawnEnemy()
        {
            Vector2 speed = Vector2.Zero,
                 position = Vector2.Zero,
                  positionF = Vector2.Zero;
            Point frameSize = new Point(75, 75);
            //float random speed
            float speedF = 1;
            float speedS = 1;

            switch (((Game1)Game).rnd.Next(4))
            {
                case 0:
                case 1:
                case 2:
                case 3:// TOP to BOTTOM            
                    position = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    positionF = new Vector2(((Game1)Game).rnd.Next(0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    speed = new Vector2(0, ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
                    speedF = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
                    speedS = ((Game1)Game).rnd.Next(suicideShootingEnemyMin, suicideShootingEnemyMax);
                    break;
            }
            if (currentWaveState == WaveState.Wave1)
            {
                if (((Game1)Game).IsEnemiesActive == true)
                {
                    int random = ((Game1)Game).rnd.Next(100);
                    if (random < likelihoodAutomated)
                    {

                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\fourblades"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\fourblades"), positionF, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if (((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\fourblades"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 2)
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\threeblade"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedS, 40));
                        }
                        else
                        {
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\fourblades"), positionF, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                            spriteList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\threeblade"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", automatedSpritePointValue, .8f, "automated"));
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedS, 40));
                        }

                    }
                    else if (random < likelihoodAutomated + likelihoodEvading)
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }
                        else if(((Game1)Game).rnd.Next(2) == 1)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                        else if(((Game1)Game).rnd.Next(2)== 2)
                        {
                            shootingEnemiesList.Add(new ShootingEnemy(enemyTexture, positionF, bulletEnemyTexture, speedF, 40));
                        }
                        else
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\skullball"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, chasingSpritePointValue, .8f, "chase"));
                        }
                    }
                    else
                    {
                        if (((Game1)Game).rnd.Next(2) == 0)
                        {
                            spriteList.Add(new ChasingSprites(Game.Content.Load<Texture2D>(@"Images\threerings"), position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), speed, "explosion", this, evadingSpritePointValue, .8f, "addLife"));
                        }
                    }
                }
                else spriteList.Clear();
            }
        }
      
        public override void Initialize()
        {
            ResetSpawnTime();
            playerPosition = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 -40, Game.GraphicsDevice.Viewport.Height - 120);
            getPPosition = playerPosition;
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
            textureBullet = Game.Content.Load<Texture2D>(@"Images\playerBullet");
            enemyTexture = Game.Content.Load<Texture2D>(@"Images\enemy40Life");
            enemyTexture1 = Game.Content.Load<Texture2D>(@"Images\enemy6");
            bossTexture = Game.Content.Load<Texture2D>(@"Images\bossSprite");
            bulletEnemyTexture = Game.Content.Load<Texture2D>(@"Images\enemyparticle");
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images\threerings"),
            playerPosition, new Point(75, 75),15, new Point(0, 0), new Point(0, 0), new Vector2(6, 6), .01f, Game.Content.Load<Texture2D>(@"Images\playerBullet"), 0f);
            turret = new Turret(Game.Content.Load<Texture2D>(@"Images\playerShipEdited"), new Vector2(400, 400),
                       Vector2.Zero, 0f, 0f, this, 100, Game.Content.Load<Texture2D>(@"Images\armorfinal"), new Vector2(10, 90));
            bossBullet = Game.Content.Load<Texture2D>(@"Images\bossbullet");

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
                    offset = 80 + i * 40;
                    livesList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\threerings"),
                        new Vector2(offset, 35), new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), Vector2.Zero, null, 0, .4f, null));
                }
                ((Game1)Game).gameOver = true;
            }
        }
        private void UpdatedEnemy(GameTime gameTime)
        {
            for (int i = 0; i < bossFlist.Count; i++)
            {
                bossFinal = bossFlist[i];
                bossFinal.Update(gameTime, Game.Window.ClientBounds);

                if(bossFinal.collisionRect.Intersects(player.collisionRect))
                {
                    Explosions(bossFlist[i].getEnemyPosition);
                    ((Game1)Game).PlayCue("explosion");

                    if(bossFinal is BossFinal)
                    {
                        if(livesList.Count > 0)
                        {
                            ((Game1)Game).NumberLivesRemaining = 0;
                        }
                    }
                    bossFlist.RemoveAt(i);
                    --i;
                }
            }
            for (int i = 0; i < evadingEnemyList.Count; i++)
            {
                evadingSenemy = evadingEnemyList[i];
                evadingSenemy.Update(gameTime);

                if (evadingSenemy.collisionRect.Intersects(player.collisionRect))
                {
                    Explosions(evadingEnemyList[i].getEnemyPosition);
                    ((Game1)Game).PlayCue("explosion");

                    if (evadingSenemy is EvadingShootingEnemy)
                    {
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --((Game1)Game).NumberLivesRemaining;
                        }
                    }
                    evadingEnemyList.RemoveAt(i);
                    --i;
                    if (evadingSenemy.IsOutOfBounds(Game.Window.ClientBounds))
                    {
                        evadingEnemyList.RemoveAt(i);
                        --i;
                    }
                }
            }
            for (int i = 0; i < shootingEnemiesList.Count; i++)
            {
                shootingEnemy = shootingEnemiesList[i];
                shootingEnemy.Update(gameTime);

                if (shootingEnemy.collisionRect.Intersects(player.collisionRect))
                {
                    Explosions(shootingEnemiesList[i].getEnemyPosition);
                    ((Game1)Game).PlayCue("explosion");
                    if (shootingEnemy is ShootingEnemy)
                    {
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --((Game1)Game).NumberLivesRemaining;
                        }
                    }
                    shootingEnemiesList.RemoveAt(i);
                    --i;
                }
                if (shootingEnemy.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    shootingEnemiesList.RemoveAt(i);
                    --i;
                }
            }
            for (int i = 0; i < suicideEnemyList.Count; i++)
            {
                suicideEnemy = suicideEnemyList[i];
                suicideEnemy.Update(gameTime);

                if (player.collisionRect.Intersects(suicideEnemyList[i].collisionRect))
                {
                    Explosions(player.GetPosition);
                    ((Game1)Game).PlayCue("explosion");

                    if (suicideEnemy is SuicideShootingEnemy)
                    {
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --((Game1)Game).NumberLivesRemaining;
                        }
                    }
                    suicideEnemyList.RemoveAt(i);
                    --i;
                }
                if (suicideEnemy.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    suicideEnemyList.RemoveAt(i);
                    --i;
                }
            }
        }
        int gegex;
        protected void UpdatedSprites(GameTime gametime)
        {
            try
            {
                AddLife();
                gegex += gametime.ElapsedGameTime.Milliseconds;
                UpdatedEnemy(gametime);
                player.Update(gametime, Game.Window.ClientBounds);
                turret.Direction = GetDirection(turret.Position, new Vector2(turret.Position.X, 0));
                turret.Rotation = GetRotation(turret.Direction);
                turret.Update(gametime);

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    shotTimer += gametime.ElapsedGameTime.Milliseconds;

                    if (shotTimer > timeBetweenShots)
                    {
                        shotTimer = 0;

                        b = new Bullet(
                        textureBullet,
                        new Vector2(turret.Position.X, turret.Position.Y),
                        turret.Direction,
                        12,
                        2000);

                        bullets.Add(b);
                        ((Game1)Game).PlayCue("gunshot");
                    }
                }
                for (int i = 0; i < bullets.Count; i++)
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
                    for (int x = 0; x < bossFlist.Count; x++)
                    {
                        if (b.collisionRect.Intersects(bossFlist[x].collisionRect))
                        {
                            bossFlist[x].EnemyLife -= 4;
                            bullets.RemoveAt(i);
                            --i;

                            if (bossFlist[x].EnemyLife <= 0)
                            {
                                Explosions(bossFlist[x].getEnemyPosition);
                                Explosions(new Vector2(bossFlist[x].getEnemyPosition.X + 2f, bossFlist[x].getEnemyPosition.Y + 20));
                                ((Game1)Game).PlayCue("explosion");
                                bossFlist.RemoveAt(x);
                                --x;
                                ((Game1)Game).AddScore(500);
                                ((Game1)Game).BossIsDestroyed = true;
                            }
                        }
                    }
                    for (int ese = 0; ese < evadingEnemyList.Count; ese++)
                    {
                        if (b.collisionRect.Intersects(evadingEnemyList[ese].collisionRect))
                        {
                            evadingEnemyList[ese].EnemyLife -= 10;
                            bullets.RemoveAt(i);
                            --i;

                            if (evadingEnemyList[ese].EnemyLife <= 0)
                            {
                                Explosions(evadingEnemyList[ese].getEnemyPosition);
                                ((Game1)Game).PlayCue("explosion");
                                evadingEnemyList.RemoveAt(ese);
                                --ese;
                                ((Game1)Game).AddScore(15);
                            }
                        }
                    }
                    for (int es = 0; es < shootingEnemiesList.Count; es++)
                    {
                        if (b.collisionRect.Intersects(shootingEnemiesList[es].collisionRect))
                        {
                            shootingEnemiesList[es].EnemyLife -= 10;
                            bullets.RemoveAt(i);
                            --i;
                            if (shootingEnemiesList[es].EnemyLife <= 0)
                            {
                                Explosions(shootingEnemiesList[es].getEnemyPosition);
                                ((Game1)Game).PlayCue("explosion");
                                shootingEnemiesList.RemoveAt(es);
                                --es;
                                ((Game1)Game).AddScore(10);
                            }
                        }
                    }
                    for (int sse = 0; sse < suicideEnemyList.Count; sse++)
                    {
                        if (b.collisionRect.Intersects(suicideEnemyList[sse].collisionRect))
                        {
                            suicideEnemyList[sse].EnemyLife -= 10;
                            bullets.RemoveAt(i);
                            --i;
                            if (suicideEnemyList[sse].EnemyLife <= 0)
                            {
                                Explosions(suicideEnemyList[sse].getEnemyPosition);
                                ((Game1)Game).PlayCue("explosion");
                                suicideEnemyList.RemoveAt(sse);
                                --sse;
                                ((Game1)Game).AddScore(5);
                            }
                        }
                    }
                    for (int bs = 0; bs < bossFlist.Count; bs++)
                    {
                        for (int sb = 0; sb < bossFlist[bs].listBullet.Count; sb++)
                        {
                            if (b.collisionRect.Intersects(bossFlist[bs].enemyBullet.collisionRect))
                            {
                                bullets.RemoveAt(i);
                                bossFlist[bs].listBullet.RemoveAt(bs);
                                --i;
                            }
                        }
                    }
                    for (int se = 0; se < shootingEnemiesList.Count; se++)
                    {
                        for (int ge = 0; ge < shootingEnemiesList[se].listBullet.Count; ge++)
                        {
                            if (b.collisionRect.Intersects(shootingEnemiesList[se].enemyBullet.collisionRect))
                            {
                                bullets.RemoveAt(i);
                                shootingEnemiesList[ge].listBullet.RemoveAt(ge);
                                --i;
                            }
                        }
                    }
                    for (int ese = 0; ese < evadingEnemyList.Count; ese++)
                    {
                        for (int ee = 0; ee < evadingEnemyList[ese].listBullet.Count; ee++)
                        {
                            if (b.collisionRect.Intersects(evadingEnemyList[ese].enemyBullet.collisionRect))
                            {
                                bullets.RemoveAt(i);
                                evadingEnemyList[ese].listBullet.RemoveAt(ese);
                                --i;
                            }
                        }
                    }
                    for (int ssb = 0; ssb < suicideEnemyList.Count; ssb++)
                    {
                        for (int w = 0; w < suicideEnemyList[ssb].listBullet.Count; w++)
                        {
                            if (b.collisionRect.Intersects(suicideEnemyList[ssb].enemyBullet.collisionRect))
                            {
                                bullets.RemoveAt(i);
                                suicideEnemyList[ssb].listBullet.RemoveAt(ssb);
                                --i;
                            }
                        }
                    }
                    if (b.IsOutOfBounds(Game.Window.ClientBounds))
                    {
                        bullets.RemoveAt(i);
                        --i;
                    }
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
                                player.ModifySpeed(.5f);
                            }
                            else if (s.collisionName == "addLife")
                            {
                                ((Game1)Game).numberLivesRemaining++;
                                livesList.Clear();

                                for (int f = 0; f < ((Game1)Game).NumberLivesRemaining; ++f)
                                {
                                    offset = 80 + f * 40;
                                    livesList.Add(new AutomatedSprites(Game.Content.Load<Texture2D>(@"Images\threerings"),
                                        new Vector2(offset, 35), new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), Vector2.Zero, null, 0, .4f, null));
                                }
                            }
                            else if (s.collisionName == "speedUp")
                            {
                                player.ModifySpeed(1f);
                                powerUpExpiration = 2500;
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
                foreach (ShootingEnemy s in shootingEnemiesList)
                {
                    for (int i = 0; i < s.listBullet.Count; i++)
                    {
                        if (player.collisionRect.Intersects(s.listBullet[i].collisionRect))
                        {
                            Explosions(player.GetPosition);
                            turret.armor -= 5;
                            ((Game1)Game).PlayCue("explosion");
                            s.listBullet.RemoveAt(i);
                            --i;
                            if (turret.armor <= 0)
                            {
                                if (s is ShootingEnemy)
                                {
                                    if (livesList.Count > 0)
                                    {
                                        livesList.RemoveAt(livesList.Count - 1);
                                        --((Game1)Game).NumberLivesRemaining;
                                    }
                                }
                            }
                        }
                        if (s.IsOutOfBounds(Game.Window.ClientBounds))
                        {
                            s.listBullet.RemoveAt(i);
                            shootingEnemiesList.RemoveAt(i);
                            --i;
                        }
                    }
                }
                foreach(BossFinal bs in bossFlist)
                {
                    for(int i = 0; i < bs.listBullet.Count; i++)
                    {
                        if(player.collisionRect.Intersects(bs.listBullet[i].collisionRect))
                        {
                            Explosions(player.GetPosition);
                            turret.armor -= 20;
                            ((Game1)Game).PlayCue("explosion");
                            bs.listBullet.RemoveAt(i);
                            --i;
                            if(turret.armor <= 0)
                            {
                                if(bs is BossFinal)
                                {
                                    if(livesList.Count > 0)
                                    {
                                        livesList.RemoveAt(livesList.Count - 1);
                                        --((Game1)Game).NumberLivesRemaining;
                                    }
                                }
                            }
                        }
                        if(bs.enemyBullet.IsOutOfBounds(Game.Window.ClientBounds))
                        {
                            bs.listBullet.RemoveAt(i);
                            --i;
                        }

                    }
                }
                foreach (SuicideShootingEnemy sse in suicideEnemyList)
                {
                    for (int i = 0; i < sse.listBullet.Count; i++)
                    {
                        if (player.collisionRect.Intersects(sse.listBullet[i].collisionRect))
                        {
                            Explosions(player.GetPosition);
                            turret.armor -= 5;
                            ((Game1)Game).PlayCue("explosion");
                            sse.listBullet.RemoveAt(i);
                            --i;
                            if (turret.armor <= 0)
                            {
                                if (sse is SuicideShootingEnemy)
                                {
                                    if (livesList.Count > 0)
                                    {
                                        livesList.RemoveAt(livesList.Count - 1);
                                        --((Game1)Game).NumberLivesRemaining;
                                    }
                                }
                            }
                        }
                        if (sse.enemyBullet.IsOutOfBounds(Game.Window.ClientBounds))
                        {
                            sse.listBullet.RemoveAt(i);
                            --i;
                        }
                    }
                }
                foreach (EvadingShootingEnemy ese in evadingEnemyList)
                {
                    for (int i = 0; i < ese.listBullet.Count; i++)
                    {
                        if (player.collisionRect.Intersects(ese.listBullet[i].collisionRect))
                        {
                            Explosions(player.GetPosition);
                            turret.armor -= 10;
                            ((Game1)Game).PlayCue("explosion");
                            ese.listBullet.RemoveAt(i);
                            --i;    
                            if (turret.armor <= 0)
                            {
                                if (ese is EvadingShootingEnemy)
                                {
                                    if (livesList.Count > 0)
                                    {
                                        livesList.RemoveAt(livesList.Count - 1);
                                        --((Game1)Game).NumberLivesRemaining;
                                    }
                                }
                            }
                        }
                        if (ese.enemyBullet.IsOutOfBounds(Game.Window.ClientBounds))
                        {
                            ese.listBullet.RemoveAt(i);
                            --i;
                        }
                    }
                }
                if (((Game1)Game).IsEnemiesActive == false)
                {
                    spriteList.Clear();
                    bullets.Clear();
                    explosionList.Clear();
                    shootingEnemiesList.Clear();
                    suicideEnemyList.Clear();
                    evadingEnemyList.Clear();
                    bossFlist.Clear();
                    player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images\threerings"),
                    getPPosition, new Point(75, 75), 10, new Point(0, 0), new Point(0, 0), new Vector2(6, 6), .01f, Game.Content.Load<Texture2D>(@"Images\playerBullet"), 0f);
                    currentWaveState = WaveState.Wave1;
                    livesList.Clear();
                    GamePlayTime = 0;
                    //UPDATE THIS LATER
                    waveOneGameTime = 50000;
                    waveTwoGameTime = 50000;
                    waveThreeGameTime = 60000;
                    waveFourGameTime = 60000;
                    turret.armor = 100;
                 
                    spawnBoss = 1;
                }
                foreach (SpriteConstructor sprite in livesList)
                    sprite.Update(gametime, Game.Window.ClientBounds);
                foreach (Explosion explode in explosionList)
                    explode.Update(gametime);
            }
            catch { return; }
        }
        public override void Update(GameTime gameTime)
        {
            GamePlayTime += gameTime.ElapsedGameTime.Milliseconds;

            switch (currentWaveState)
            {
                case WaveState.Wave1:
                    waves = "WAVE 1";
                    if (GamePlayTime > waveOneGameTime)
                    {
                        currentWaveState = WaveState.Wave2;
                        turret.armor = 100;
                        GamePlayTime = 0;
                    }
                    break;
                case WaveState.Wave2:
                    waves = "WAVE 2";
                    if (GamePlayTime > waveTwoGameTime)
                    {
                        currentWaveState = WaveState.Wave3;
                        turret.armor = 100;
                        GamePlayTime = 0;
                    } 
                    break;
                case WaveState.Wave3:
                    waves = "WAVE 3";

                    if (GamePlayTime > waveThreeGameTime)
                    {
                        currentWaveState = WaveState.Wave4;
                        turret.armor = 100;
                        GamePlayTime = 0;
                    }
                    break;
                case WaveState.Wave4:
                    waves = "WAVE 4";
                    
                if(GamePlayTime > waveFourGameTime)
                {
                    currentWaveState = WaveState.FinalWave;
                    turret.armor = 100;
                    GamePlayTime = 0;
                }
                    break;
                case WaveState.FinalWave:
                    SpawnBoss();
                    waves = "FINAL WAVE";
                    break;
            }   
            nextSpawnTime += -gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                WaveOneSpawnEnemy();
                WaveTwoSpawnEnemy();
                WaveThreeSpawnEnemy();
                WaveFourSpawnEnemy();
                FinalWave();
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
            switch(currentWaveState)
            {
                case WaveState.Wave1:
                    spriteBatch.DrawString(((Game1)Game).waveFont, waves, new Vector2(((Game1)Game).Window.ClientBounds.Width / 2 - 58, 5), Color.Goldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                case WaveState.Wave2:
                    spriteBatch.DrawString(((Game1)Game).waveFont, waves, new Vector2(((Game1)Game).Window.ClientBounds.Width / 2 - 58, 5), Color.Goldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                case WaveState.Wave3:
                    spriteBatch.DrawString(((Game1)Game).waveFont, waves, new Vector2(((Game1)Game).Window.ClientBounds.Width / 2 - 58, 5), Color.Goldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                case WaveState.Wave4:
                    spriteBatch.DrawString(((Game1)Game).waveFont, waves, new Vector2(((Game1)Game).Window.ClientBounds.Width / 2 - 58, 5), Color.Goldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                case WaveState.FinalWave:
                    spriteBatch.DrawString(((Game1)Game).waveFont, "BOSS LIFE", new Vector2(((Game1)Game).Window.ClientBounds.Width  - 200, 5), Color.Goldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(((Game1)Game).waveFont, waves, new Vector2(((Game1)Game).Window.ClientBounds.Width / 2 - 100, 5), Color.Goldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
            }
          
            turret.Draw(spriteBatch);
            player.Draw(gameTime, spriteBatch);
            
      
            //DRAW AUTOMATED ENEMIES
            foreach (SpriteConstructor s in spriteList)
                s.Draw(gameTime, spriteBatch);
            //DRAW SHOOTING ENEMIES
            foreach (ShootingEnemy se in shootingEnemiesList)
                se.Draw(spriteBatch);
            //DRAW SUICIDE SHOOTING ENEMIES
            foreach (SuicideShootingEnemy sse in suicideEnemyList)
                sse.Draw(spriteBatch);
            //DRAW EVADING ENEMIES
            foreach (EvadingShootingEnemy ese in evadingEnemyList)
                ese.Draw(spriteBatch);
            //DRAW LIFE
            foreach (SpriteConstructor sprite in livesList)
                sprite.Draw(gameTime, spriteBatch);
            //DRAW BULLETS
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);
            //DRAW EXPLOSIONS
            foreach (Explosion e in explosionList)
                e.Draw(spriteBatch);
            foreach (BossFinal bs in bossFlist)
                bs.Draw(spriteBatch);

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
