using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fisobs.Core;
using IL.MoreSlugcats;

namespace TemplateMod
{
    public class CrateAbstract : AbstractPhysicalObject
    {
        public float hue;
        public float saturation;
        public float scaleX;
        public float scaleY;

        public CrateAbstract(World world, WorldCoordinate pos, EntityID ID) : base(world, CrateFisobs.AbstrCrate, null, pos, ID)
        {
            scaleX = 1;
            scaleY = 1;
            saturation = 0.8f;
            hue = 0.8f;
        }

        public override void Realize()
        {
            base.Realize();
            if (realizedObject == null)
                realizedObject = new Crate(this);
        }

        public override string ToString()
        {
            return this.SaveToString($"{hue};{saturation};{scaleX};{scaleY}");
        }
    }
}
