using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    class BackgroundParallax
    {
        Texture2D texture;
        Vector2[] positions;
        int speed;

        public void Initialize(ContentManager content, string texturePath, int screenHeight, int speed)
        {
            texture = content.Load<Texture2D>(texturePath);

            this.speed = speed;
            positions = new Vector2[screenHeight];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2(0, i * texture.Height);
            }
        }
        public void Update()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i].Y += speed;

                if (speed <= 0)
                {
                    if (positions[i].Y <= -texture.Height)
                    {
                        positions[i].Y = texture.Height * (positions.Length - 2);
                    }
                }
                else
                {
                    if (positions[i].Y >= texture.Height * (positions.Length - 2))
                    {
                        positions[i].Y = -texture.Height;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture, positions[i], Color.White);
            }
        }
    }
}
