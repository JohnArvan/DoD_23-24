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

        //Lifetime stuff
        public bool isActive;
        private float activeTimer = 500;

        //Movement stuff
        private float deltaX;
        private float deltaY;
        private float distanceX;
        private float distanceY;
        private float maxDistance;

        //Tracking player stuff
        private Vector2 trackPos;
        private float speed = 120;

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

        //Move toward position tracked by farmer
        private void Move(GameTime gameTime)
        {
            //Calculate the differences on X and Y axes
            deltaX = trackPos.X - pos.X;
            deltaY = trackPos.Y - pos.Y;

            //Calculate the absolute distances
            distanceX = Math.Abs(deltaX);
            distanceY = Math.Abs(deltaY);

            //Determine the maximum distance
            maxDistance = Math.Max(distanceX, distanceY);

            //Calculate the movement amounts for X and Y axes
            pos.X += (deltaX / maxDistance) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            pos.Y += (deltaY / maxDistance) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Check collisions with level
            bulletBounds.X = (int)pos.X - (int)(dims.X / 2);
            bulletBounds.Y = (int)pos.Y - (int)(dims.Y / 2);
            if (level.CheckCollision(bulletBounds)) DisableBullet();
        }

        public void DisableBullet()
        {
            isActive = false;
        }
    }
}
