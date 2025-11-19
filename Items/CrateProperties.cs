using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fisobs.Properties;

namespace My_Nice_Rain_World_Mod.Items
{
    public class CrateProperties : ItemProperties
    {
        public override void Throwable(Player player, ref bool throwable) => throwable = true;

        // The player should only be able to grab one Crate at a time
        public override void Grabability(Player player, ref Player.ObjectGrabability grabability)
        {
            grabability = Player.ObjectGrabability.TwoHands;
        }
    }
}
