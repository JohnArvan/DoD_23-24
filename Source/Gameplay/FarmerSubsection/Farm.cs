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

using System.Reflection.Metadata;

#endregion

namespace DoD_23_24
{
    public class Farm
    {
        Level level;
        FarmPlayer playerInstance;
        FarmPlayer bigBroInstance;
        FarmPlayer lilBroInstance;

        FarmPlayer currentPlayerInstance;
        Farmer farmer;
        private bool switchingPlayer = true;

        public static Vector2 farmPlayerPos;
        public static Vector2 farmBigBroPos;
        public static Vector2 farmLilBroPos;

        public Farm()
        {
            level = new Level("Content/map.tmx", "Tiny Adventure Pack\\");
            playerInstance = new FarmPlayer("2D/Sprites/Item", new Vector2(100, 100), new Vector2(16, 16), true, level);
            playerInstance.ChangeCurrentPlayer();

            bigBroInstance = new FarmPlayer("2D/Sprites/Item", new Vector2(80, 100), new Vector2(20, 20), true, level);
            bigBroInstance.ChangeSpeed(0.75f);

            lilBroInstance = new FarmPlayer("2D/Sprites/Item", new Vector2(120, 100), new Vector2(12, 12), true, level);
            lilBroInstance.ChangeSpeed(1.25f);

            //currentPlayerInstance = playerInstance;

            farmer = new Farmer("Tiny Adventure Pack/Other/Blue_orb", new Vector2(50, 200), new Vector2(16, 16), true, level);
        }

        public void Update(GameTime gameTime)
        {
            playerInstance.Update(gameTime);
            bigBroInstance.Update(gameTime);
            lilBroInstance.Update(gameTime);
            farmer.Update(gameTime);

            SwitchPlayer();

            farmPlayerPos = playerInstance.pos;
            farmBigBroPos = bigBroInstance.pos;
            farmLilBroPos = lilBroInstance.pos;
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
                //Switch to big brother
                if (playerInstance.CheckCurrentPlayer())
                {
                    playerInstance.ChangeCurrentPlayer();
                    bigBroInstance.ChangeCurrentPlayer();

                    //currentPlayerInstance = bigBroInstance;
                }
                //Switch to little brother
                else if (bigBroInstance.CheckCurrentPlayer())
                {
                    bigBroInstance.ChangeCurrentPlayer();
                    lilBroInstance.ChangeCurrentPlayer();

                    //currentPlayerInstance = lilBroInstance;
                }
                //Switch to player
                else
                {
                    lilBroInstance.ChangeCurrentPlayer();
                    playerInstance.ChangeCurrentPlayer();

                    //currentPlayerInstance = playerInstance;
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
