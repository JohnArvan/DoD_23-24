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
            
            //People
            playerInstance = new("Player", "2D/Sprites/Item", new Vector2(100, 100), 0.0f, new Vector2(16, 16));
            playerInstance.ChangeCurrentPlayer();

            bigBroInstance = new("BigBro", "2D/Sprites/Item", new Vector2(80, 100), 0.0f, new Vector2(20, 20));
            bigBroInstance.ChangeSpeed(0.75f);
            bigBroInstance.ChangeStrength(1.25f);

            lilBroInstance = new("LilBro", "2D/Sprites/Item", new Vector2(120, 100), 0.0f, new Vector2(12, 12));
            lilBroInstance.ChangeSpeed(1.25f);
            lilBroInstance.ChangeStrength(0.75f);

            farmerInstance = new Farmer("Farmer", "Tiny Adventure Pack/Other/Blue_orb", new Vector2(200, 200), 0.0f, new Vector2(16, 16));

            //Interactable items
            HayBale hayBale = new("HayBale", new Vector2(100, 200), 0.0f, new Vector2(16, 16));

            Rock rock = new("Rock", new Vector2(150, 150), 0.0f, new Vector2(8, 8));

            //Static items
            BrokenFence brokenFence1L = new("BrokenFence", "Tiny Adventure Pack/Other/Misc/Rock", new Vector2(50, 150), 0.0f, new Vector2(16, 16));
            Entity brokenFence1C = new Entity("RandomThing", Layer.Tiles);
            brokenFence1C.AddComponent(new TransformComponent(brokenFence1C, new Vector2(66, 150), 0.0f, new Vector2(12, 16)));
            brokenFence1C.AddComponent(new RenderComponent(brokenFence1C, "Tiny Adventure Pack/Other/Misc/Rock"));
            BrokenFence brokenFence1R = new("BrokenFence", "Tiny Adventure Pack/Other/Misc/Rock", new Vector2(78, 150), 0.0f, new Vector2(16, 16));

            //Camera
            camera = new Entity("Camera", Layer.Camera);
            camera.AddComponent(new CameraComponent(camera, playerInstance));
            
            TileMapGenerator tileMapGenerator = new TileMapGenerator("Content/map.tmx", "Tiny Adventure Pack\\");
            entities.AddRange(tileMapGenerator.GetTiles());
            entities.Add(playerInstance);
            entities.Add(bigBroInstance);
            entities.Add(lilBroInstance);
            entities.Add(farmerInstance);
            entities.Add(hayBale);
            entities.Add(rock);
            entities.Add(rock.GetOverlapZone());
            entities.Add(brokenFence1L);
            entities.Add(brokenFence1C);
            entities.Add(brokenFence1R);
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

        //Switch player currently being controlled 
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

        //Return the currently controlled player
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
