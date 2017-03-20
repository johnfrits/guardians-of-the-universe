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

namespace Final_Project
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Sounds
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue trackCue;
        Cue mainCue;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BackgroundParallax newBGParallax;
        SpriteManager spriteManager;
        //Random Intervals
        public Random rnd { get; private set; }
        //score
        SpriteFont scoreFont;
        int currentScore = 0;
        public bool sounds = false;
        public bool sounds1 = false;

        private bool isEnemiesActive;

        public bool IsEnemiesActive
        {
            get { return isEnemiesActive; }
            set
            {
                isEnemiesActive = value;
            }
        }


        public bool gameOver;
        public bool GameOver
        {
            get { return gameOver; }
            set
            {
                gameOver = value;
            }
        }

        //life
        public int numberLivesRemaining = 4;
        public int NumberLivesRemaining
        {
            get { return numberLivesRemaining; }
            set
            {
                numberLivesRemaining = value;
                if (numberLivesRemaining == 0)
                {
                    numberLivesRemaining = 4;
                    currentGameState = GameState.GameOver;
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    gameOver = true;
                    isEnemiesActive = false;
                   
                }
            }
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            rnd = new Random();
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }
        public void PlayCue(string cueName) { soundBank.PlayCue(cueName); }
        public void AddScore(int score)
        {
            currentScore += score;
        }
        enum GameState { Start, InGame, GameOver };
        GameState currentGameState = GameState.Start;
        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;
            gameOver = true;
            newBGParallax = new BackgroundParallax();
            base.Initialize();
          
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            newBGParallax.Initialize(Content, @"Images\background1", graphics.PreferredBackBufferHeight, +3);

            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            //font
            scoreFont = Content.Load<SpriteFont>(@"Font\score");

        }
        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            if (!sounds)
            {
                trackCue = soundBank.GetCue("gameMenuTrack");
                trackCue.Play();
                sounds = true;
            }
            switch (currentGameState)
            {
                case GameState.Start:
                    isEnemiesActive = true;
                    break;
                case GameState.InGame:
                    newBGParallax.Update();
                    if (!sounds1)
                    {
                        mainCue = soundBank.GetCue("gameplayTrack");
                        mainCue.Play();
                        sounds1 = true;
                    }
                    break;
                case GameState.GameOver:
         
                    break;
            }
            audioEngine.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (currentGameState)
            {
                case GameState.Start:
                 
                    spriteBatch.Begin();
                    string text = "SHOT THE ENEMIES";
                    spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(text).X / 2),
                                          (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(text).Y / 2)), Color.SaddleBrown);
                    text = "(Press any key to begin)";
                    spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(text).X / 2),
                                            (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(text).Y / 2) + 30), Color.SaddleBrown);
                    spriteBatch.End();

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {

                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                        sounds1 = false;

                    }
                    break;

                case GameState.InGame:
                    spriteBatch.Begin();
                    trackCue.Stop(AudioStopOptions.AsAuthored);
                    newBGParallax.Draw(spriteBatch);
                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore,
                                    new Vector2(10, 10), Color.DarkGoldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                    spriteBatch.End();
                    break;

                case GameState.GameOver:

                    spriteBatch.Begin();
                    mainCue.Stop(AudioStopOptions.AsAuthored);
                    string gameover = "Game Over! The blades win again!";

                    spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).X / 2),
                                (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2)), Color.SaddleBrown);
                    gameover = "Your score: " + currentScore; spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).X / 2),
                     (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2) + 30), Color.SaddleBrown);
                    gameover = "(Press ENTER to exit)"; spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).X / 2),
                     (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2) + 60), Color.SaddleBrown);
                   

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.Start;
                        sounds = false;
                        currentScore = 0;
                    }
                    spriteBatch.End();


                    break;
            }
            base.Draw(gameTime);
        }
    }
}
