using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class Farmer : Basic2D
    {
        public float speed = 25f;
        public Rectangle farmerBounds;
        private Vector2 trackPos;
        Level level;

        public Farmer(string PATH, Vector2 POS, Vector2 DIMS, bool shouldScale, Level level) : base(PATH, POS, DIMS, shouldScale)
        {
            farmerBounds = new Rectangle((int)pos.X - (int)(dims.X / 2), (int)pos.Y - (int)(dims.Y / 2), (int)dims.X, (int)dims.Y);
            this.level = level;
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            base.Update(gameTime);
        }

        //Move towards current target
        public void Move(GameTime gameTime)
        {
            trackPos = Farm.farmerTrackPos;
            Vector2 initPos = pos;

            if (pos.X < trackPos.X - 0.5f)
            {
                pos.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.X > trackPos.X + 0.5f)
            {
                pos.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            farmerBounds.X = (int)pos.X - (int)(dims.X / 2);
            if (level.CheckCollision(farmerBounds)) pos.X = initPos.X;

            if (pos.Y < trackPos.Y - 0.5f)
            {
                pos.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.Y > trackPos.Y + 0.5f)
            {
                pos.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            farmerBounds.Y = (int)pos.Y - (int)(dims.Y / 2);
            if (level.CheckCollision(farmerBounds)) pos.Y = initPos.Y;
        }
    }
}
