using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class Farmer : Basic2D
    {
        public float speed = 15f;
        public Rectangle farmerBounds;
        Level level;

        //Tracking player stuff
        private bool isMoving = true;
        private Vector2[] playerPositions;
        private float[] distances;
        private Vector2 trackPos;
        int minIndex;

        //Shooting stuff
        private FarmerBullet bullet;
        private float readyTimer = 3000;
        private float readyTimeNeeded_M = 3000;     //Needs 3 seconds to prepare before shooting
        private float shootDistance = 50;
        private float shootCooldown = 0;
        private float shootCooldownNeeded_M = 6000; //Needs 6 seconds to reload
        private bool reloading = false;
        private bool isAiming = false;

        public Farmer(string PATH, Vector2 POS, Vector2 DIMS, bool shouldScale, Level level) : base(PATH, POS, DIMS, shouldScale)
        {
            farmerBounds = new Rectangle((int)pos.X - (int)(dims.X / 2), (int)pos.Y - (int)(dims.Y / 2), (int)dims.X, (int)dims.Y);
            this.level = level;

            playerPositions = new Vector2[3];
            for (int i = 0; i < playerPositions.Length; i++)
            {
                playerPositions[i] = pos;
            }

            bullet = new FarmerBullet("Tiny Adventure Pack/Other/Blue_orb", pos, new Vector2(8, 8), true, level, trackPos);
            bullet.DisableBullet();
        }

        public override void Update(GameTime gameTime)
        {
            //If loaded, follow player
            if (!reloading)
            {
                FindClosestPlayer();
                if (isMoving)
                {
                    Move(gameTime);
                }
                ReadyShotgun(gameTime);
            }
            //Reload and stop moving
            else if (shootCooldown > 0)
            {
                shootCooldown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (shootCooldown <= 0)
                {
                    reloading = false;
                }
            }
            
            base.Update(gameTime);

            if (bullet.isActive)
            {
                bullet.Update(gameTime);
            }
        }

        public override void Draw()
        {
            base.Draw();

            if (bullet.isActive)
            {
                bullet.Draw();
            }
        }

        //Find closest of the three players
        private void FindClosestPlayer()
        {
            //Get Player's positions
            playerPositions[0] = Farm.farmPlayerPos;
            playerPositions[1] = Farm.farmBigBroPos;
            playerPositions[2] = Farm.farmLilBroPos;
            //Calculate distances
            distances = new float[3];
            distances[0] = MathF.Abs(pos.X - playerPositions[0].X) + MathF.Abs(pos.Y - playerPositions[0].Y);
            distances[1] = MathF.Abs(pos.X - playerPositions[1].X) + MathF.Abs(pos.Y - playerPositions[1].Y);
            distances[2] = MathF.Abs(pos.X - playerPositions[2].X) + MathF.Abs(pos.Y - playerPositions[2].Y);
            //Lock on to closest player
            minIndex = Array.IndexOf(distances, distances.Min());
            trackPos = playerPositions[minIndex];
        }

        //Move towards current target
        private void Move(GameTime gameTime)
        {
            Vector2 initPos = pos;
            if (pos.X < trackPos.X - 1)
            {
                pos.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.X > trackPos.X + 1)
            {
                pos.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            farmerBounds.X = (int)pos.X - (int)(dims.X / 2);
            if (level.CheckCollision(farmerBounds)) pos.X = initPos.X;

            if (pos.Y < trackPos.Y - 1)
            {
                pos.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (pos.Y > trackPos.Y + 1)
            {
                pos.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            farmerBounds.Y = (int)pos.Y - (int)(dims.Y / 2);
            if (level.CheckCollision(farmerBounds)) pos.Y = initPos.Y;
        }

        //Prepare to fire shotgun at player
        private void ReadyShotgun(GameTime gameTime)
        {
            //If player is within range, prepare to shoot
            if ((MathF.Abs(pos.X - trackPos.X) + MathF.Abs(pos.Y - trackPos.Y)) < shootDistance)
            {
                //Stop moving and prepare to shoot
                isMoving = false;
                isAiming = true;
                AimShotgun(gameTime);
            }
            else if ((MathF.Abs(pos.X - trackPos.X) + MathF.Abs(pos.Y - trackPos.Y)) < (shootDistance + 15) && isAiming)
            {
                AimShotgun(gameTime);
            }
            //If player leaves range (plus some more distance), start moving again
            else if ((MathF.Abs(pos.X - trackPos.X) + MathF.Abs(pos.Y - trackPos.Y)) > (shootDistance + 15))
            {
                isMoving = true;
                isAiming = false;
                readyTimer = readyTimeNeeded_M;
            }
        }

        //Aim at player while in range
        private void AimShotgun(GameTime gameTime)
        {
            readyTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (readyTimer <= 0)
            {
                ShootShotgun();
            }
        }

        //Shoot shotgun
        private void ShootShotgun()
        {
            Debug.WriteLine("pow!");

            reloading = true;
            shootCooldown = shootCooldownNeeded_M;

            bullet = new FarmerBullet("Tiny Adventure Pack/Other/Red_orb", pos, new Vector2(8, 8), true, level, trackPos);
        }
    }
}
