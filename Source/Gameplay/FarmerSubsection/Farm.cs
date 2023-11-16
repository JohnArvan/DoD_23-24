using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class Farm
    {
        Level level;
        FarmPlayer playerInstance;
        FarmPlayer bigBroInstance;
        FarmPlayer lilBroInstance;
        Farmer farmer;
        private bool switchingPlayer = true;

        public Farm()
        {
            level = new Level("Content/map.tmx", "Tiny Adventure Pack\\");
            playerInstance = new FarmPlayer("2D/Sprites/Item", new Vector2(100, 100), new Vector2(16, 16), true, level);
            playerInstance.ChangeCurrentPlayer();

            bigBroInstance = new FarmPlayer("2D/Sprites/Item", new Vector2(80, 100), new Vector2(20, 20), true, level);
            bigBroInstance.ChangeSpeed(0.75f);

            lilBroInstance = new FarmPlayer("2D/Sprites/Item", new Vector2(120, 100), new Vector2(12, 12), true, level);
            lilBroInstance.ChangeSpeed(1.25f);

            farmer = new Farmer("2D/Sprites/Item", new Vector2(50, 150), new Vector2(16, 16), true);
        }

        public void Update(GameTime gameTime)
        {
            playerInstance.Update(gameTime);
            bigBroInstance.Update(gameTime);
            lilBroInstance.Update(gameTime);
            farmer.Update(gameTime);

            SwitchPlayer();
        }

        public void Draw()
        {
            level.Draw();
            playerInstance.Draw();
            bigBroInstance.Draw();
            lilBroInstance.Draw();
            farmer.Draw();
        }

        public void SwitchPlayer()
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Space) && switchingPlayer)
            {
                switchingPlayer = !switchingPlayer;
                if (playerInstance.CheckCurrentPlayer())
                {
                    playerInstance.ChangeCurrentPlayer();
                    bigBroInstance.ChangeCurrentPlayer();
                }
                else if (bigBroInstance.CheckCurrentPlayer())
                {
                    bigBroInstance.ChangeCurrentPlayer();
                    lilBroInstance.ChangeCurrentPlayer();
                }
                else
                {
                    lilBroInstance.ChangeCurrentPlayer();
                    playerInstance.ChangeCurrentPlayer();
                }
            }
            else if (kstate.IsKeyUp(Keys.Space) && !switchingPlayer)
            {
                switchingPlayer = !switchingPlayer;
            }
        }

        public FarmPlayer GetFarmPlayer()
        {
            if (playerInstance.CheckCurrentPlayer())
            {
                return playerInstance;
            }
            else if (bigBroInstance.CheckCurrentPlayer())
            {
                return bigBroInstance;
            }
            return lilBroInstance;
        }
    }
}
