using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class FarmerBullet : Entity
    {
        TransformComponent transform;

        //Lifetime stuff
        public bool bulletActive;
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

        public FarmerBullet(string name, string PATH, Vector2 POS, float ROT, Vector2 DIMS, Vector2 trackPos) : base(name, Layer.Player)
        {
            transform = (TransformComponent)AddComponent(new TransformComponent(this, POS, ROT, DIMS));
            AddComponent(new RenderComponent(this, PATH));
            AddComponent(new CollisionComponent(this, false, false));

            activeTimer = 500;
            this.trackPos = trackPos;
            bulletActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (bulletActive)
            {
                Move(gameTime);
                //Count time bullet alive time
                activeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (activeTimer <= 0)
                {
                    DisableBullet();
                }
            }
            base.Update(gameTime);
        }

        //Move toward position tracked by farmer
        private void Move(GameTime gameTime)
        {
            //Calculate the differences on X and Y axes
            deltaX = trackPos.X - transform.pos.X;
            deltaY = trackPos.Y - transform.pos.Y;

            //Calculate the absolute distances
            distanceX = Math.Abs(deltaX);
            distanceY = Math.Abs(deltaY);

            //Determine the maximum distance
            maxDistance = Math.Max(distanceX, distanceY);

            //Calculate the movement amounts for X and Y axes
            transform.pos.X += (deltaX / maxDistance) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            transform.pos.Y += (deltaY / maxDistance) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        //Disable bullet and move it out of bounds
        public void DisableBullet()
        {
            bulletActive = false;
            transform.pos = new Vector2(-100, -100);
        }

        public override void OnCollision(Entity otherEntity)
        {
            if (bulletActive)
            {
                //For now bullet can only collide with the players, will be updated to include obstacles later
                if (otherEntity.name == "Player" || otherEntity.name == "BigBro" || otherEntity.name == "LilBro")
                {
                    DisableBullet();
                }
            }
        }
    }
}