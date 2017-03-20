using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    abstract class SpriteConstructor
    {
        Texture2D textureImage;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int collisionOffset, timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 2;
        protected Vector2 speed;
        public Vector2 orginalSpeed;
        protected Vector2 position;
        protected float scale = 1;
        protected float orginalScale = 1;
        private int enemylife;
        public int Enemylife
        {
            get { return enemylife; }
            set { enemylife = value; }
        }
        public Point getCurrentFrameSize { get { return frameSize; } }
    
        public SpriteConstructor(Texture2D textureImage, Vector2 position,
                      Point frameSize, int collisionOffset,
                      Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue, float scale, int life, string collisionName)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
              sheetSize, speed, defaultMillisecondsPerFrame, collisionCueName, scoreValue, collisionName)
        {
            this.scale = scale;
            this.enemylife = life;
        }
        public SpriteConstructor(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
                      Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName, int scoreValue, string collisionName)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.collisionCueName = collisionCueName;
            this.scoreValue = scoreValue;
            this.collisionName = collisionName;
            orginalSpeed = speed;
        }
        public string collisionName { get; private set; }
        public string collisionCueName { get; private set; }
        public abstract Vector2 direction { get; }
        public Vector2 GetPosition { get { return position; } } 
                                    
        public int scoreValue { get; protected set; }
        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }
        public void ModifyScale(float modifier)
        {
            scale *= modifier;
        }
        public void ResetScale()
        {
            scale = orginalScale;
        }
        public void ModifySpeed(float modifier)
        {
            speed *= modifier;
        }
        public void ResetSpeed()
        {
            speed = orginalSpeed;
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position,
                            new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                            frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, scale,
                            SpriteEffects.None, 0);
        }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    (int)((frameSize.X - (collisionOffset * 2) * scale)),
                    (int)((frameSize.Y - (collisionOffset * 2) * scale)));
            }
        }
        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (position.X < -frameSize.X ||
                position.X > clientRect.Width ||
                position.Y < -frameSize.Y ||
                position.Y > clientRect.Height)
            {
                return true;
            }
            return false;
        }


    }
}
