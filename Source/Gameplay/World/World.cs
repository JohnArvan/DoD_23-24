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
    public class World
    {

<<<<<<< Updated upstream
        Level level;
        Player playerInstance;

        public World()
        {
            level = new Level("Content/map.tmx", "Tiny Adventure Pack\\");
            playerInstance = new Player("2D/Sprites/Item", new Vector2(100, 100), new Vector2(16, 16), true, level);
=======
        FarmPlayer playerInstance;
        FarmPlayer bigBrotherInstance;
        FarmPlayer littleBrotherInstance;

        private bool switchingCharacter = true;

        public World()
        {
            map = new TmxMap("Content/map.tmx");
            Texture2D tileSet = Globals.content.Load<Texture2D>("Tiny Adventure Pack\\" + map.Tilesets[0].Name.ToString());
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            int tileSetTilesWide = tileSet.Width / tileWidth;
            mapManager = new TileMapManager(Globals.spriteBatch, map, tileSet, tileSetTilesWide, tileWidth, tileHeight);

            collisionObjects = new List<Rectangle>();
            foreach(var o in map.ObjectGroups["Collisions"].Objects)
            {
                collisionObjects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            playerInstance = new FarmPlayer("2D\\Sprites\\Item", new Vector2(100, 100), new Vector2(16, 16), true);
            playerInstance.ChangeCurrentPlayer();

            bigBrotherInstance = new FarmPlayer("Tiny Adventure Pack\\Other\\Coin", new Vector2(80, 120), new Vector2(32, 32), true);
            bigBrotherInstance.ChangeSpeed(0.5f);

            littleBrotherInstance = new FarmPlayer("Tiny Adventure Pack\\Other\\Heart", new Vector2(120, 120), new Vector2(8, 8), true);
            littleBrotherInstance.ChangeSpeed(2);
>>>>>>> Stashed changes
        }

        public virtual void Update(GameTime gameTime)
        {
<<<<<<< Updated upstream
            playerInstance.Update(gameTime);
=======
            SwitchCharacter();

            var initPos = playerInstance.pos;
            playerInstance.Update(gameTime);
            var bigInitPos = bigBrotherInstance.pos;
            bigBrotherInstance.Update(gameTime);
            var litInitPos = littleBrotherInstance.pos;
            littleBrotherInstance.Update(gameTime);
            foreach(var rect in collisionObjects)
            {
                if (rect.Intersects(playerInstance.playerBounds))
                {
                    playerInstance.pos = initPos;
                }
                if (rect.Intersects(bigBrotherInstance.playerBounds))
                {
                    bigBrotherInstance.pos = bigInitPos;
                }
                if (rect.Intersects(littleBrotherInstance.playerBounds))
                {
                    littleBrotherInstance.pos = litInitPos;
                }
            }


>>>>>>> Stashed changes
        }

        public virtual void Draw()
        {
            level.Draw();
            playerInstance.Draw();
            bigBrotherInstance.Draw();
            littleBrotherInstance.Draw();
        }

        public Player GetPlayer()
        {
            if (playerInstance.CheckCurrentPlayer())
            {
                return playerInstance;
            }
            else if (bigBrotherInstance.CheckCurrentPlayer())
            {
                return bigBrotherInstance;
            }
            return littleBrotherInstance;
        }

        //Farm
        private void SwitchCharacter()
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Space) && switchingCharacter)
            {
                switchingCharacter = false;
                if (playerInstance.CheckCurrentPlayer())
                {
                    playerInstance.ChangeCurrentPlayer();
                    bigBrotherInstance.ChangeCurrentPlayer();
                }
                else if (bigBrotherInstance.CheckCurrentPlayer())
                {
                    bigBrotherInstance.ChangeCurrentPlayer();
                    littleBrotherInstance.ChangeCurrentPlayer();
                }
                else if (littleBrotherInstance.CheckCurrentPlayer())
                {
                    littleBrotherInstance.ChangeCurrentPlayer();
                    playerInstance.ChangeCurrentPlayer();
                }
            }

            if (kstate.IsKeyUp(Keys.Space))
            {
                switchingCharacter = true;
            }
        }
    }
}

