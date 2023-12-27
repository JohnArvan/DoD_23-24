using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoD_23_24
{
    public class FarmerComponent : Component
    {
        Farmer parentFarmer;

        public FarmerComponent(Entity entity, Farmer parent) : base(entity)
        {
            parentFarmer = parent;
        }

        public Farmer GetParentFarmer()
        {
            return parentFarmer;
        }
    }
}
