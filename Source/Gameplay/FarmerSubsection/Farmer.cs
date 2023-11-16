using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class Farmer : Basic2D
    {
        public float speed = 25f;
        private Vector2 trackPos;

        public Farmer(string PATH, Vector2 POS, Vector2 DIMS, bool shouldScale) : base(PATH, POS, DIMS, shouldScale)
        {
            trackPos = new Vector2(100, 100);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (pos.X < trackPos.X - 0.5f)
            {
                pos.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.X > trackPos.X + 0.5f)
            {
                pos.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (pos.Y < trackPos.Y - 0.5f)
            {
                pos.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.Y > trackPos.Y + 0.5f)
            {
                pos.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
