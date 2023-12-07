using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class FarmPlayer : Entity
    {
        float speed = 50f;
        TransformComponent transform;
        bool isPressed = false;
        bool isFrozen = false;

        private bool currentPlayer = false;
        public float xPos;
        public float yPos;

        public FarmPlayer(string name, string PATH, Vector2 POS, float ROT, Vector2 DIMS) : base(name, Layer.Player)
        {
            transform = (TransformComponent)AddComponent(new TransformComponent(this, POS, ROT, DIMS));
            AddComponent(new RenderComponent(this, PATH));
            AddComponent(new CollisionComponent(this, true, true));
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
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                transform.pos.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                transform.pos.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                transform.pos.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void OnCollision(Entity otherEntity)
        {
            if (otherEntity.name == "OverlapZone")
            {
                InteractWithNPC(otherEntity);
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

        public Vector2 GetPos()
        {
            return new Vector2(transform.pos.X, transform.pos.Y);
        }
    }
}
