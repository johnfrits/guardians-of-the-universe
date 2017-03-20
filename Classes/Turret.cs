using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Final_Project
{
    public class Turret
    {
        SpriteManager playerPosition;
        public Texture2D DrawTexture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Rotation { get; set; }
        public float Speed { get; set; }
        Vector2 spaceShip;
        public int armor;
        public Texture2D armorTexture;
        public Rectangle armorRectangle;
        public Vector2 armorBarPosition;
     
        public Turret(Texture2D texture, Vector2 position, Vector2 direction,
                           float rotation, float Speed, SpriteManager playerPosition, int armor, Texture2D armorTexture, Vector2 armorBarPosition)
        {
            this.DrawTexture = texture;
            this.Position = position;
            this.Direction = direction;
            this.Rotation = rotation;
            this.Speed = Speed;
            this.playerPosition = playerPosition;
            this.armor = armor;
            this.armorTexture = armorTexture;
            this.armorBarPosition = armorBarPosition;
        }
        public void Update(GameTime gameTime)
        {
            this.Position += Direction * Speed;
            spaceShip = playerPosition.GetPlayerPosition();
            Position = new Vector2(spaceShip.X +50, spaceShip.Y + 50);
            armorRectangle = new Rectangle((int)armorBarPosition.X, ((int)armorBarPosition.Y), armor, 25);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.DrawTexture,
                this.Position,
                null,
                Color.White,
                0f,
                new Vector2(
                    this.DrawTexture.Width / 2,
                    this.DrawTexture.Height / 2),
                .3f,
                SpriteEffects.None,
                0);

            spriteBatch.Draw(this.armorTexture, this.armorRectangle, Color.White);
        }
    }
}
