using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class FarmerBullet : Basic2D
    {
        public Rectangle bulletBounds;
        private Level level;

        public bool isActive;
        private float activeTimer = 500;

        private Vector2 trackPos;
        private float speed = 90;

        public FarmerBullet(string PATH, Vector2 POS, Vector2 DIMS, bool shouldScale, Level level, Vector2 trackPos) : base(PATH, POS, DIMS, shouldScale)
        {
            bulletBounds = new Rectangle((int)pos.X - (int)(dims.X / 2), (int)pos.Y - (int)(dims.Y / 2), (int)dims.X, (int)dims.Y);
            this.level = level;
            this.trackPos = trackPos;
            isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                Move(gameTime);
                activeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (activeTimer <= 0)
                {
                    DisableBullet();
                }
            }
        }

        //TODO: make movement smoother
        //Move toward position tracked by farmer
        private void Move(GameTime gameTime)
        {
            if (pos.X < trackPos.X)
            {
                pos.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.X > trackPos.X)
            {
                pos.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            bulletBounds.X = (int)pos.X - (int)(dims.X / 2);
            if (level.CheckCollision(bulletBounds)) DisableBullet();

            if (pos.Y < trackPos.Y)
            {
                pos.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.Y > trackPos.Y)
            {
                pos.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            bulletBounds.Y = (int)pos.Y - (int)(dims.Y / 2);
            if (level.CheckCollision(bulletBounds)) DisableBullet();
        }

        public void DisableBullet()
        {
            isActive = false;
        }
    }
}
