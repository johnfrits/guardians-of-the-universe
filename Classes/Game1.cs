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
        Cue hoveCue1;
        Cue hoveCue2;
        Cue hoveCue3;
        Cue hoveCue4;
        Cue hoveCue5;
        Cue hoveCue6;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BackgroundParallax newBGParallax1;
        BackgroundParallax newBGParallax2;
        SpriteManager spriteManager;
        Texture2D textureBackground;
        Texture2D textureLogoFont;
        //
        Texture2D textureStart;
        Rectangle startRectangle;
        //
        Texture2D textureAbout;
        Rectangle aboutRectangle;
        //
        Texture2D textureInsc;
        Rectangle inscRectangle;
        //
        Texture2D textureExit;
        Rectangle exitRectangle;
        //
        Texture2D texturePlay;
        Rectangle playRect;
        //
        Texture2D textureGoBack;
        Rectangle goBackRect;
        //
        Texture2D aboutText;
        Rectangle aboutTextRect;
        //
        Texture2D justshootem;

        Texture2D gameOverBackGround;
        Color color1 = new Color(255, 255, 255, 255);
        Color color2 = new Color(255, 255, 255, 255);
        Color color3 = new Color(255, 255, 255, 255);
        Color color4 = new Color(255, 255, 255, 255);
        Color color5 = new Color(255, 255, 255, 255);
        Color color6 = new Color(255, 255, 255, 255);

        bool hove1, hove2, hove3, hove4, hove5, hove6;
        //Random Intervals
        public Random rnd { get; private set; }
        //score
        public SpriteFont scoreFont;
        public SpriteFont waveFont;
        int currentScore = 0;
        public bool sounds = false;
        public bool sounds1 = false;
        bool startHoveSound, aboutHoveSound, inscHoveSound, exitHoveSound;
        bool aboutPlayHoveSound, aboutGBHoveSound;

        enum GameState { Start, About, Instructions, InGame, GameOver, WinTheGame };
        GameState currentGameState = GameState.Start;

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
        private bool bossIsDestroyed = false;

        public bool BossIsDestroyed
        {
            get { return bossIsDestroyed; }
            set
            {
                bossIsDestroyed = value;

                if (bossIsDestroyed == true)
                {
                    currentGameState = GameState.WinTheGame;
                    numberLivesRemaining = 3;
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    gameOver = false;
                    isEnemiesActive = false;
                    sounds = false;
                    spriteManager.offset = 0;
                }
            }
        }
        //life
        public int numberLivesRemaining = 3;
        public int NumberLivesRemaining
        {
            get { return numberLivesRemaining; }
            set
            {
                numberLivesRemaining = value;
                if (numberLivesRemaining == 0)
                {
                    currentGameState = GameState.GameOver;
                    numberLivesRemaining = 3;
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    gameOver = false;
                    isEnemiesActive = false;
                    spriteManager.offset = 0;
                }
            }
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            rnd = new Random();
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 933;
            this.Window.Title = "GUARDIAN OF THE UNIVERSE";
        }
        public void PlayCue(string cueName) { soundBank.PlayCue(cueName); }
        public void AddScore(int score)
        {
            currentScore += score;
        }

        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;
            gameOver = false;
            newBGParallax1 = new BackgroundParallax();
            newBGParallax2 = new BackgroundParallax();
            startRectangle = new Rectangle((Window.ClientBounds.Width / 2) - 80, (Window.ClientBounds.Height) - 380, 156, 52);
            aboutRectangle = new Rectangle((Window.ClientBounds.Width / 2) - 80, (Window.ClientBounds.Height) - 335, 156, 52);
            inscRectangle = new Rectangle((Window.ClientBounds.Width / 2) - 130, (Window.ClientBounds.Height) - 290, 260, 52);
            exitRectangle = new Rectangle((Window.ClientBounds.Width / 2) - 80, (Window.ClientBounds.Height) - 240, 156, 52);
            playRect = new Rectangle((Window.ClientBounds.Width / 2) - 80, (Window.ClientBounds.Height) - 210, 156, 52);
            goBackRect = new Rectangle((Window.ClientBounds.Width / 2) - 80, (Window.ClientBounds.Height) - 170, 156, 52);
            aboutTextRect = new Rectangle((Window.ClientBounds.Width / 2) - 450, (Window.ClientBounds.Height / 2) - 270, 900, 500);
            base.Initialize();

        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            newBGParallax1.Initialize(Content, @"Images\backg", graphics.PreferredBackBufferHeight, +(int)4);
            newBGParallax2.Initialize(Content, @"Images\backg", graphics.PreferredBackBufferHeight, +(int)2);

            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            textureBackground = Content.Load<Texture2D>(@"Images\backg");
            textureLogoFont = Content.Load<Texture2D>(@"Images\gameFinalLogo");
            scoreFont = Content.Load<SpriteFont>(@"Font\score");
            waveFont = Content.Load<SpriteFont>(@"Font\waves");
            textureStart = Content.Load<Texture2D>(@"Images\start");
            textureAbout = Content.Load<Texture2D>(@"Images\about");
            textureInsc = Content.Load<Texture2D>(@"Images\instructions");
            textureExit = Content.Load<Texture2D>(@"Images\exit");
            texturePlay = Content.Load<Texture2D>(@"Images\play");
            textureGoBack = Content.Load<Texture2D>(@"Images\goback");
            aboutText = Content.Load<Texture2D>(@"Images\aboutText");
            gameOverBackGround = Content.Load<Texture2D>(@"Images\gameoverbg");
            justshootem = Content.Load<Texture2D>(@"Images\justshootem");
        }
        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Rectangle mouseRec = new Rectangle(mouse.X, mouse.Y, (int).5, (int).5);

            switch (currentGameState)
            {
                case GameState.Start:
                    newBGParallax2.Update();
                    //START BUTTON
                    if (mouseRec.Intersects(startRectangle))
                    {
                        if (color1.A == 255) hove1 = false;
                        if (color1.A == 0) hove1 = true;
                        if (hove1) { color1.A += 5; } else color1.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.InGame;
                            spriteManager.Enabled = true;
                            spriteManager.Visible = true;
                            sounds1 = false;
                        }
                        if (!startHoveSound)
                        {
                            hoveCue1 = soundBank.GetCue("button-3");
                            hoveCue1.Play();
                            startHoveSound = true;
                        }
                        else if (!hoveCue1.IsPlaying)
                        {
                            startHoveSound = true;
                            hoveCue1.Stop(AudioStopOptions.AsAuthored);
                        }

                    }
                    else if (!mouseRec.Intersects(startRectangle))
                    {
                        startHoveSound = false;
                        if (color1.A < 255)
                        {
                            color1.A += 5;
                        }
                        else if (color1.A > 0)
                        {
                            color1.A -= 5;
                        }
                    }
                    //
                    if (mouseRec.Intersects(inscRectangle))
                    {
                        if (color2.A == 255) hove2 = false;
                        if (color2.A == 0) hove2 = true;
                        if (hove2) color2.A += 5; else color2.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.Instructions;
                        }
                        if (!inscHoveSound)
                        {
                            hoveCue2 = soundBank.GetCue("button-3");
                            hoveCue2.Play();
                            inscHoveSound = true;
                        }
                        else if (!hoveCue2.IsPlaying)
                        {
                            inscHoveSound = true;
                            hoveCue2.Stop(AudioStopOptions.AsAuthored);
                        }
                    }
                    else if (!mouseRec.Intersects(inscRectangle))
                    {
                        inscHoveSound = false;
                        if (color2.A < 255)
                        {
                            color2.A += 5;
                        }
                        else if (color2.A > 0)
                        {
                            color2.A -= 5;
                        }
                    }
                    //
                    if (mouseRec.Intersects(aboutRectangle))
                    {
                        if (color3.A == 255) hove3 = false;
                        if (color3.A == 0) hove3 = true;
                        if (hove3) color3.A += 5; else color3.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.About;
                        }
                        if (!aboutHoveSound)
                        {
                            hoveCue3 = soundBank.GetCue("button-3");
                            hoveCue3.Play();
                            aboutHoveSound = true;
                        }
                        else if (!hoveCue3.IsPlaying)
                        {
                            aboutHoveSound = true;
                            hoveCue3.Stop(AudioStopOptions.AsAuthored);
                        }

                    }
                    else if (!mouseRec.Intersects(aboutRectangle))
                    {
                        aboutHoveSound = false;
                        if (color3.A < 255)
                        {
                            color3.A += 5;
                        }
                        else if (color3.A > 0)
                        {
                            color3.A -= 5;
                        }
                    }
                    //
                    if (mouseRec.Intersects(exitRectangle))
                    {
                        if (color4.A == 255) hove4 = false;
                        if (color4.A == 0) hove4 = true;
                        if (hove4) color4.A += 5; else color4.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            this.Exit();
                        }
                        if (!exitHoveSound)
                        {
                            hoveCue4 = soundBank.GetCue("button-3");
                            hoveCue4.Play();
                            exitHoveSound = true;
                        }
                        else if (!hoveCue4.IsPlaying)
                        {
                            exitHoveSound = true;
                            hoveCue4.Stop(AudioStopOptions.AsAuthored);
                        }

                    }
                    else if (!mouseRec.Intersects(exitRectangle))
                    {
                        exitHoveSound = false;
                        if (color4.A < 255)
                        {
                            color4.A += 5;
                        }
                        else if (color4.A > 0)
                        {
                            color4.A -= 5;
                        }
                    }

                    if (!sounds)
                    {
                        trackCue = soundBank.GetCue("gameMenuTrack");
                        trackCue.Play();
                        sounds = true;
                    }
                    isEnemiesActive = true;
                    break;
                case GameState.About:
                    newBGParallax2.Update();
                    if (mouseRec.Intersects(playRect))
                    {
                        if (color5.A == 255) hove5 = false;
                        if (color5.A == 0) hove5 = true;
                        if (hove5) { color5.A += 5; } else color5.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.InGame;
                            spriteManager.Enabled = true;
                            spriteManager.Visible = true;
                            sounds1 = false;
                        }
                        if (!aboutPlayHoveSound)
                        {
                            hoveCue5 = soundBank.GetCue("button-3");
                            hoveCue5.Play();
                            aboutPlayHoveSound = true;
                        }
                        else if (!hoveCue5.IsPlaying)
                        {
                            aboutPlayHoveSound = true;
                            hoveCue5.Stop(AudioStopOptions.AsAuthored);
                        }

                    }
                    else if (!mouseRec.Intersects(playRect))
                    {
                        aboutPlayHoveSound = false;
                        if (color5.A < 255)
                        {
                            color5.A += 5;
                        }
                        else if (color5.A > 0)
                        {
                            color5.A -= 5;
                        }
                    }
                    //
                    if (mouseRec.Intersects(goBackRect))
                    {
                        if (color6.A == 255) hove6 = false;
                        if (color6.A == 0) hove6 = true;
                        if (hove6) { color6.A += 5; } else color6.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.Start;
                        }
                        if (!aboutGBHoveSound)
                        {
                            hoveCue6 = soundBank.GetCue("button-3");
                            hoveCue6.Play();
                            aboutGBHoveSound = true;
                        }
                        else if (!hoveCue6.IsPlaying)
                        {
                            aboutGBHoveSound = true;
                            hoveCue6.Stop(AudioStopOptions.AsAuthored);
                        }
                    }
                    else if (!mouseRec.Intersects(goBackRect))
                    {
                        aboutGBHoveSound = false;
                        if (color6.A < 255)
                        {
                            color6.A += 5;
                        }
                        else if (color6.A > 0)
                        {
                            color6.A -= 5;
                        }
                    }
                    //
                    break;
                case GameState.Instructions:
                    newBGParallax2.Update();

                    if (mouseRec.Intersects(playRect))
                    {
                        if (color5.A == 255) hove5 = false;
                        if (color5.A == 0) hove5 = true;
                        if (hove5) { color5.A += 5; } else color5.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.InGame;
                            spriteManager.Enabled = true;
                            spriteManager.Visible = true;
                            sounds1 = false;
                        }
                        if (!aboutPlayHoveSound)
                        {
                            hoveCue5 = soundBank.GetCue("button-3");
                            hoveCue5.Play();
                            aboutPlayHoveSound = true;
                        }
                        else if (!hoveCue5.IsPlaying)
                        {
                            aboutPlayHoveSound = true;
                            hoveCue5.Stop(AudioStopOptions.AsAuthored);
                        }

                    }
                    else if (!mouseRec.Intersects(playRect))
                    {
                        aboutPlayHoveSound = false;
                        if (color5.A < 255)
                        {
                            color5.A += 5;
                        }
                        else if (color5.A > 0)
                        {
                            color5.A -= 5;
                        }
                    }
                    //
                    if (mouseRec.Intersects(goBackRect))
                    {
                        if (color6.A == 255) hove6 = false;
                        if (color6.A == 0) hove6 = true;
                        if (hove6) { color6.A += 5; } else color6.A -= 5;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.Start;
                        }
                        if (!aboutGBHoveSound)
                        {
                            hoveCue6 = soundBank.GetCue("button-3");
                            hoveCue6.Play();
                            aboutGBHoveSound = true;
                        }
                        else if (!hoveCue6.IsPlaying)
                        {
                            aboutGBHoveSound = true;
                            hoveCue6.Stop(AudioStopOptions.AsAuthored);
                        }
                    }
                    else if (!mouseRec.Intersects(goBackRect))
                    {
                        aboutGBHoveSound = false;
                        if (color6.A < 255)
                        {
                            color6.A += 5;
                        }
                        else if (color6.A > 0)
                        {
                            color6.A -= 5;
                        }
                    }
                    break;

                case GameState.InGame:
                    newBGParallax1.Update();
                    if (!sounds1)
                    {
                        mainCue = soundBank.GetCue("gameplayTrack");
                        mainCue.Play();
                        sounds1 = true;
                    }
                    break;
                case GameState.GameOver:
                    newBGParallax2.Update();
                    break;
                case GameState.WinTheGame:
                    newBGParallax2.Update();
                    if (!sounds)
                    {
                        trackCue = soundBank.GetCue("gameMenuTrack");
                        trackCue.Play();
                        sounds = true;
                    }
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
                    spriteBatch.Draw(textureBackground, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
                    newBGParallax2.Draw(spriteBatch);
                    spriteBatch.Draw(textureLogoFont, new Rectangle((Window.ClientBounds.Width - textureLogoFont.Width - 220) / 2, 50, 800, 170), Color.White);
                    spriteBatch.Draw(textureStart, startRectangle, color1);
                    spriteBatch.Draw(textureAbout, aboutRectangle, color3);
                    spriteBatch.Draw(textureInsc, inscRectangle, color2);
                    spriteBatch.Draw(textureExit, exitRectangle, color4);
                    spriteBatch.End();
                    break;
                case GameState.About:
                    spriteBatch.Begin();
                    spriteBatch.Draw(textureBackground, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
                    newBGParallax2.Draw(spriteBatch);
                    spriteBatch.Draw(aboutText, aboutTextRect, Color.White);
                    spriteBatch.Draw(texturePlay, playRect, color5);
                    spriteBatch.Draw(textureGoBack, goBackRect, color6);
                    spriteBatch.End();
                    break;
                case GameState.Instructions:
                    spriteBatch.Begin();
                    spriteBatch.Draw(textureBackground, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
                    newBGParallax2.Draw(spriteBatch);
                    spriteBatch.Draw(justshootem, new Rectangle(145,90,700, 90), Color.White);
                    spriteBatch.Draw(texturePlay, new Rectangle(playRect.X, playRect.Y, playRect.Width, playRect.Height), color5);
                    spriteBatch.Draw(textureGoBack,new Rectangle(goBackRect.X,  goBackRect.Y,  goBackRect.Width,  goBackRect.Height), color6);
                    spriteBatch.End();
                    break;
                case GameState.InGame:
                    spriteBatch.Begin();
                 
                    spriteBatch.Draw(textureBackground, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
                    trackCue.Stop(AudioStopOptions.AsAuthored);
                    newBGParallax1.Draw(spriteBatch);
                    spriteBatch.DrawString(scoreFont, "Life: ", new Vector2(10, 35), Color.DarkGoldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore,
                                    new Vector2(10, 10), Color.DarkGoldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(scoreFont, "Armor: ",
                                   new Vector2(10, 62), Color.DarkGoldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                    spriteBatch.End();
                    break;

                case GameState.GameOver:
                    spriteBatch.Begin();
                    spriteBatch.Draw(textureBackground, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
                    newBGParallax2.Draw(spriteBatch);
                    spriteBatch.Draw(gameOverBackGround, new Rectangle((Window.ClientBounds.Width / 2) - 300, 30, 600, 300), Color.White);

                    string gameover = "";
                    gameover = "YOUR SCORE: " + currentScore;
                    spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).X / 2), (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2) + 30), Color.Goldenrod);
                    gameover = "(Press ENTER TO GO BACK TO MAIN MENU)";
                    spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).X / 2), (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2) + 60), Color.Goldenrod);
                    gameover = "(Press ESC TO GO BACK TO EXIT)";
                    spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).X / 2), (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2) + 90), Color.Goldenrod);

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.Start;
                        currentScore = 0;
                        mainCue.Stop(AudioStopOptions.AsAuthored);
                        sounds = false;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    spriteBatch.End();


                    break;

                case GameState.WinTheGame:
                    spriteBatch.Begin();
                    mainCue.Stop(AudioStopOptions.AsAuthored);
                    spriteBatch.Draw(textureBackground, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
                    newBGParallax2.Draw(spriteBatch);
                    string s = "Your score: " + currentScore;
                    spriteBatch.DrawString(scoreFont, s, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(s).X / 2),
                     (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(s).Y / 2) + 30), Color.SaddleBrown);
                    s = "(Press Enter To Go Back To Main Menu)";
                    spriteBatch.DrawString(scoreFont, s, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(s).X / 2), (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(s).Y / 2) + 60), Color.Goldenrod);
                    s = "(Press Esc to Exit)";
                    spriteBatch.DrawString(scoreFont, s, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(s).X / 2), (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(s).Y / 2) + 90), Color.Goldenrod);

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.Start;
                        currentScore = 0; 
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }

    }

}
