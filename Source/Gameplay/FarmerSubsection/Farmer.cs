﻿using Microsoft.Xna.Framework;
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
    public class Farmer : Entity
    {
        public float speed = 15f;
        TransformComponent transform;
        //Component used by rock to trigger distraction
        FarmerComponent farmerComponent;

        //Movement stuff
        private float deltaX;
        private float deltaY;
        private float distanceX;
        private float distanceY;
        private float maxDistance;

        //Tracking player stuff
        private bool isMoving = true;
        private List<FarmPlayer> players;
        private float[] distances;
        private Vector2 trackPos;
        private int minIndex;
        private bool isSeekingPlayer = true;

        //Shooting stuff
        private FarmerBullet bullet;
        private float readyTimer = 750;
        private float readyTimeNeeded_M = 750;      //Needs 0.75 seconds to prepare before shooting
        private float shootDistance = 75;
        private float shootCooldown = 0;
        private float shootCooldownNeeded_M = 3000; //Needs 3 seconds to reload
        private bool isReloading = false;
        private bool isAiming = false;

        public Farmer(string name, string PATH, Vector2 POS, float ROT, Vector2 DIMS, List<FarmPlayer> players) : base(name, Layer.Player)
        {
            transform = (TransformComponent)AddComponent(new TransformComponent(this, POS, ROT, DIMS));
            AddComponent(new RenderComponent(this, PATH));
            AddComponent(new CollisionComponent(this, true, true));

            this.players = players;

            farmerComponent = (FarmerComponent)AddComponent(new FarmerComponent(this, this));

            //Start bullet offscreen so as to not accidentally collide with anything
            //(may just be a temporary fix for now)
            bullet = new FarmerBullet("Bullet", "Tiny Adventure Pack/Other/Red_orb", new Vector2(0, 0), 0.0f, new Vector2(8, 8), trackPos);
            bullet.DisableBullet();
        }

        public override void Update(GameTime gameTime)
        {
            //If loaded, follow track position
            if (!isReloading)
            {
                //If looking for player, find closest player and move toward them
                if (isSeekingPlayer)
                {
                    FindClosestPlayer();
                }
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
                    isReloading = false;
                }
            }
            
            base.Update(gameTime);

            if (bullet.bulletActive)
            {
                bullet.Update(gameTime);
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (bullet.bulletActive)
            {
                bullet.Draw();
            }
        }

        public void RemovePlayerFromList(FarmPlayer player)
        {
            players.Remove(player);
        }

        //Find closest of the three players
        private void FindClosestPlayer()
        {
            //All players dead so stop
            if (players.Count == 0)
            {
                isMoving = false;
                isReloading = false;
                isAiming = false;
                isSeekingPlayer = false;
                return;
            }

            //Calculate distances
            distances = new float[players.Count];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = MathF.Abs(transform.pos.X - players[i].GetPos().X) + MathF.Abs(transform.pos.Y - players[i].GetPos().Y);
            }
            //Lock on to closest player
            minIndex = Array.IndexOf(distances, distances.Min());
            trackPos = players.ElementAt(minIndex).GetPos();
        }

        //Move towards current target
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

        //Trigger a distraction for the farmer to check 
        //Ex: direction of rock thrown
        public void TriggerDistraction(Vector2 pos)
        {
            trackPos = pos;
            isMoving = true;
            isAiming = false;
            isSeekingPlayer = false;
            readyTimer = readyTimeNeeded_M;
        }


        //  SHOTGUN STUFF

        //Prepare to fire shotgun at player
        private void ReadyShotgun(GameTime gameTime)
        {
            //If tracking source of distraction, reach point of distraction then continue tracking player
            if (!isSeekingPlayer)
            {
                if ((MathF.Abs(transform.pos.X - trackPos.X) + MathF.Abs(transform.pos.Y - trackPos.Y)) > 10)
                {
                    return;
                }
                else
                {
                    isSeekingPlayer = true;
                }
            }

            //If tracking player...
            //If player is within range, prepare to shoot
            if ((MathF.Abs(transform.pos.X - trackPos.X) + MathF.Abs(transform.pos.Y - trackPos.Y)) < shootDistance)
            {
                //Stop moving and prepare to shoot
                isMoving = false;
                isAiming = true;
                AimShotgun(gameTime);
            }
            else if ((MathF.Abs(transform.pos.X - trackPos.X) + MathF.Abs(transform.pos.Y - trackPos.Y)) < (shootDistance + 15) && isAiming)
            {
                AimShotgun(gameTime);
            }
            //If player leaves range (plus some more distance), start moving again
            else if ((MathF.Abs(transform.pos.X - trackPos.X) + MathF.Abs(transform.pos.Y - trackPos.Y)) > (shootDistance + 15))
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

            isReloading = true;
            shootCooldown = shootCooldownNeeded_M;

            bullet = new FarmerBullet("Bullet", "Tiny Adventure Pack/Other/Red_orb", transform.pos, 0.0f, new Vector2(8, 8), trackPos);
        }
    }
}