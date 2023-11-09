using DoD_23_24;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class FarmPlayer : Player
    {
        private bool currentPlayer = false;
        
        public FarmPlayer(string PATH, Vector2 POS, Vector2 DIMS, bool shouldScale) : base(PATH, POS, DIMS, shouldScale)
        {
            playerBounds = new Rectangle((int)pos.X - (int)(dims.X / 2), (int)pos.Y - (int)(dims.Y / 2), (int)dims.X, (int)dims.Y);
        }

        

        public void ChangeCurrentPlayer()
        {
            currentPlayer = !currentPlayer;
        }

        public bool CheckCurrentPlayer()
        {
            return currentPlayer;
        }

        public void ChangeSpeed(float factor)
        {
            speed *= factor;
        }
    }
}
