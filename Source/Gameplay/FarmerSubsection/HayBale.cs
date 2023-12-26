using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class HayBale : Entity
    {
        TransformComponent transform;

        public HayBale(string name, Vector2 POS, float ROT, Vector2 DIMS) : base(name, Layer.Player)
        {
            transform = (TransformComponent)AddComponent(new TransformComponent(this, POS, ROT, DIMS));
            AddComponent(new RenderComponent(this, "Tiny Adventure Pack/Other/Coin"));
            AddComponent(new CollisionComponent(this, true, false));
        }

        public override void OnCollision(Entity otherEntity)
        {
            //Can only be pushed by Big Bro
            if (otherEntity.name == "BigBro")
            {
                GetComponent<CollisionComponent>().isMoveable = true;
            }
            else
            {
                GetComponent<CollisionComponent>().isMoveable = false;
            }
        }
    }
}
