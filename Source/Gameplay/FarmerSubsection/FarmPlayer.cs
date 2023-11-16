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
        public float xPos;
        public float yPos;

        public FarmPlayer(string PATH, Vector2 POS, Vector2 DIMS, bool shouldScale, Level level) : base(PATH, POS, DIMS, shouldScale, level)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (currentPlayer)
            {
                base.Update(gameTime);
            }
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
