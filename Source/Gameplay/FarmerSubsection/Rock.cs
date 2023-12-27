#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
#endregion

namespace DoD_23_24
{
    public class Rock : Entity
    {
        Entity overlapZone;
        TransformComponent transform;
        CollisionComponent collision;

        private bool isMoving;

        private float xDir, yDir, strength;
        private float timeMoved = 0; 
        private float airtime = 0;

        public Rock(string name, Vector2 POS, float ROT, Vector2 DIMS) : base(name, Layer.Item)
        {
            transform = (TransformComponent)AddComponent(new TransformComponent(this, POS, ROT, DIMS));
            AddComponent(new RenderComponent(this, "Tiny Adventure Pack/Other/Green_orb"));
            collision = (CollisionComponent)AddComponent(new CollisionComponent(this, false, false));

            overlapZone = new Entity("OverlapZone", Layer.Item);
            overlapZone.AddComponent(new TransformComponent(overlapZone, new Vector2((int)POS.X - 8, (int)POS.Y - 8), 0.0f, new Vector2(24, 24)));
            overlapZone.AddComponent(new CollisionComponent(overlapZone, false, false));
            overlapZone.AddComponent(new OverlapZoneComponent(overlapZone, this));
            overlapZone.SetParent(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isMoving) 
            {
                Move(gameTime);
                if (timeMoved > airtime)
                {
                    timeMoved = 0;
                    isMoving = false;
                }
                timeMoved += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void FollowPlayer(Vector2 playerPos, float playerWidth, float xDir, float yDir)
        {
            //unsimplified
            //transform.pos.X = playerPos.X + (playerWidth / 4) + (xDir * playerWidth / 2);
            //transform.pos.Y = playerPos.Y + (playerWidth / 4) + (yDir * playerWidth / 2);
            //simplified (figured it would run better)
            transform.pos.X = playerPos.X + playerWidth * 0.5f * (xDir + 0.5f);
            transform.pos.Y = playerPos.Y + playerWidth * 0.5f * (yDir + 0.5f);

            overlapZone.GetComponent<TransformComponent>().pos = new Vector2((int)transform.pos.X - 8, (int)transform.pos.Y - 8);
        }

        public void GetThrown(float xDir, float yDir, float strength)
        {
            this.xDir = -xDir;
            this.yDir = -yDir;
            this.strength = strength;
            timeMoved = 0;
            airtime = 1;
            isMoving = true;
        }

        private void Move(GameTime gameTime)
        {
            transform.pos.X -= strength * 2.5f * xDir * (float)gameTime.ElapsedGameTime.TotalSeconds;
            transform.pos.Y -= strength * 2.5f * yDir * (float)gameTime.ElapsedGameTime.TotalSeconds;
            overlapZone.GetComponent<TransformComponent>().pos = new Vector2((int)transform.pos.X - 8, (int)transform.pos.Y - 8);
        }

        public Entity GetOverlapZone()
        {
            return overlapZone;
        }

        public override void OnCollision(Entity otherEntity)
        {
            if (isMoving)
            {
                if (otherEntity.name != "Player" && otherEntity.name != "BigBro" && otherEntity.name != "LilBro" && otherEntity.name != "OverlapZone" && otherEntity.name != "Bullet")
                {
                    isMoving = false;
                }
            }
        }
    }
}
