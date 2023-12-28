using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class FarmPlayer : Entity
    {
        float speed = 50f;
        float throwStrength = 50f;
        TransformComponent transform;
        bool isPressed = false;
        bool isFrozen = false;

        private bool currentPlayer = false;
        public float xPos;
        public float yPos;

        private bool holdingRock;
        private float xDir;
        private float yDir;

        public bool isAlive = true;
        private Farm farm;
        private Farmer farmer;
        public int playerIndex;

        public FarmPlayer(string name, string PATH, Vector2 POS, float ROT, Vector2 DIMS, Farm farm) : base(name, Layer.Player)
        {
            transform = (TransformComponent)AddComponent(new TransformComponent(this, POS, ROT, DIMS));
            AddComponent(new RenderComponent(this, PATH));
            AddComponent(new CollisionComponent(this, true, true));
            this.farm = farm;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (currentPlayer)
            {
                Movement(gameTime);
            }
        }

        public void Movement(GameTime gameTime)
        {
            if (isFrozen)
            {
                return;
            }

            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left))
            {
                transform.pos.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                xDir = -1;
                if (kstate.IsKeyUp(Keys.Up) && kstate.IsKeyUp(Keys.Down))
                {
                    yDir = 0;
                }
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                transform.pos.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                xDir = 1;
                if (kstate.IsKeyUp(Keys.Up) && kstate.IsKeyUp(Keys.Down))
                {
                    yDir = 0;
                }
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                transform.pos.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                yDir = -1;
                if (kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.Right))
                {
                    xDir = 0;
                }
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                transform.pos.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                yDir = 1;
                if (kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.Right))
                {
                    xDir = 0;
                }
            }
        }

        public override void OnCollision(Entity otherEntity)
        {
            if (currentPlayer)
            {
                if (otherEntity.name == "OverlapZone" && otherEntity.layer == Layer.NPC)
                {
                    InteractWithNPC(otherEntity);
                }
                else if (otherEntity.name == "OverlapZone" && otherEntity.layer == Layer.Item)
                {
                    InteractWithRock(otherEntity);
                }
            }

            //Kill player
            if (otherEntity.name == "Bullet" && isAlive)
            {
                bool isCurrent = currentPlayer;
                Debug.WriteLine(name + " is dead :(");
                isAlive = false;
                currentPlayer = false;
                farmer.RemovePlayerFromList(this);
                farm.SwitchToNextPlayer(true, isCurrent);
            }
        }

        public void InteractWithNPC(Entity overlapZone)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isPressed)
            {
                overlapZone.GetComponent<OverlapZoneComponent>().GetParentNPC().Speak();
                isFrozen = overlapZone.GetComponent<OverlapZoneComponent>().GetParentNPC().CheckIfPlayerFrozen();
                isPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                isPressed = false;
            }
        }

        public void InteractWithRock(Entity overlapZone)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isPressed && !holdingRock)
            {
                isPressed = true;
                holdingRock = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isPressed && holdingRock)
            {
                isPressed = true;
                holdingRock = false;
                overlapZone.GetComponent<OverlapZoneComponent>().GetParentRock().GetThrown(xDir, yDir, throwStrength);
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                isPressed = false;
            }

            if (holdingRock)
            {
                overlapZone.GetComponent<OverlapZoneComponent>().GetParentRock().FollowPlayer(transform.pos, transform.dims.X, xDir, yDir);
            }
        }

        //Update whether this is the currently selected player for camera and movement
        public void ChangeCurrentPlayer(bool value)
        {
            currentPlayer = value;
        }

        public bool CheckCurrentPlayer()
        {
            return currentPlayer;
        }

        public bool CheckAlive()
        {
            return isAlive;
        }

        public void ChangeSpeed(float factor)
        {
            speed *= factor;
        }

        public void ChangeStrength(float factor)
        {
            throwStrength *= factor;
        }

        public Vector2 GetPos()
        {
            return new Vector2(transform.pos.X, transform.pos.Y);
        }

        public void SetFarmer(Farmer farmer)
        {
            this.farmer = farmer;
        }
    }
}
