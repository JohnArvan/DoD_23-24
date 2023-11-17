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
    public class Farmer : Basic2D
    {
        public float speed = 25f;
        public Rectangle farmerBounds;
        Level level;

        //Tracking player stuff
        private Vector2[] playerPositions;
        private float[] distances;
        private Vector2 trackPos;
        int minIndex;

        public Farmer(string PATH, Vector2 POS, Vector2 DIMS, bool shouldScale, Level level) : base(PATH, POS, DIMS, shouldScale)
        {
            farmerBounds = new Rectangle((int)pos.X - (int)(dims.X / 2), (int)pos.Y - (int)(dims.Y / 2), (int)dims.X, (int)dims.Y);
            this.level = level;

            playerPositions = new Vector2[3];
            for (int i = 0; i < playerPositions.Length; i++)
            {
                playerPositions[i] = pos;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);

            base.Update(gameTime);
        }

        //Move towards current target
        private void Move(GameTime gameTime)
        {
            FindClosestPlayer();

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
    }
}
