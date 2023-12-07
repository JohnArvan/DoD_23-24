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
using System.Net;

#endregion

namespace DoD_23_24
{
    public class Farm
    {
        public List<Entity> entities = new List<Entity>();
        FarmPlayer playerInstance;
        FarmPlayer bigBroInstance;
        FarmPlayer lilBroInstance;
        Farmer farmerInstance;
        Entity camera;

        private bool switchingPlayer = true;

        public Farm()
        {
            Globals.collisionSystem = new CollisionSystem();
            
            playerInstance = new FarmPlayer("Player", "2D/Sprites/Item", new Vector2(100, 100), 0.0f, new Vector2(16, 16));
            playerInstance.ChangeCurrentPlayer();

            bigBroInstance = new FarmPlayer("Player", "2D/Sprites/Item", new Vector2(80, 100), 0.0f, new Vector2(20, 20));
            bigBroInstance.ChangeSpeed(0.75f);

            lilBroInstance = new FarmPlayer("Player", "2D/Sprites/Item", new Vector2(120, 100), 0.0f, new Vector2(12, 12));
            lilBroInstance.ChangeSpeed(1.25f);

            farmerInstance = new Farmer("Farmer", "Tiny Adventure Pack/Other/Blue_orb", new Vector2(50, 200), 0.0f, new Vector2(16, 16));

            camera = new Entity("Camera", Layer.Camera);
            camera.AddComponent(new CameraComponent(camera, playerInstance));
            
            TileMapGenerator tileMapGenerator = new TileMapGenerator("Content/map.tmx", "Tiny Adventure Pack\\");
            entities.AddRange(tileMapGenerator.GetTiles());
            entities.Add(playerInstance);
            entities.Add(bigBroInstance);
            entities.Add(lilBroInstance);
            entities.Add(farmerInstance);
            entities.Add(camera);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities)
            {
                entity.Update(gameTime);
            }

            SwitchPlayer();
            farmerInstance.UpdatePlayerPositions(playerInstance.GetPos(), bigBroInstance.GetPos(), lilBroInstance.GetPos());

            Globals.collisionSystem.Update(gameTime);
        }

        public void Draw()
        {
            foreach (Entity entity in entities)
            {
                entity.Draw();
            }
        }

        public Entity GetCamera()
        {
            foreach (Entity entity in entities)
            {
                if (entity.name == "Camera")
                {
                    return entity;
                }
            }
            return null;
        }

        public void SwitchPlayer()
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.S) && switchingPlayer)
            {
                switchingPlayer = !switchingPlayer;
                //Switch to big brother
                if (playerInstance.CheckCurrentPlayer())
                {
                    playerInstance.ChangeCurrentPlayer();
                    bigBroInstance.ChangeCurrentPlayer();
                    camera.GetComponent<CameraComponent>().ChangeTarget(bigBroInstance);
                }
                //Switch to little brother
                else if (bigBroInstance.CheckCurrentPlayer())
                {
                    bigBroInstance.ChangeCurrentPlayer();
                    lilBroInstance.ChangeCurrentPlayer();
                    camera.GetComponent<CameraComponent>().ChangeTarget(lilBroInstance);
                }
                //Switch to player
                else
                {
                    lilBroInstance.ChangeCurrentPlayer();
                    playerInstance.ChangeCurrentPlayer();
                    camera.GetComponent<CameraComponent>().ChangeTarget(playerInstance);
                }
            }
            else if (kstate.IsKeyUp(Keys.S) && !switchingPlayer)
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
