using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fisobs.Core;
using Fisobs.Items;
using Fisobs.Properties;
using Fisobs.Sandbox;
using UnityEngine;

namespace TemplateMod
{
    internal class CrateFisobs : Fisob
    {
        public static readonly AbstractPhysicalObject.AbstractObjectType AbstrCrate = new("Crate", true);
        public static readonly MultiplayerUnlocks.SandboxUnlockID mCrate = new("Crate", true);

        public CrateFisobs() : base(AbstrCrate)
        {
            Icon = new CrateIcon();

            SandboxPerformanceCost = new(linear: 0.2f, 0f);

            RegisterUnlock(mCrate, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat, data: 0);
        }

        public override AbstractPhysicalObject Parse(World world, EntitySaveData saveData, SandboxUnlock? unlock)
        {
            string[] p = saveData.CustomData.Split(';');

            if (p.Length < 5)
            {
                p = new string[5];
            }

            // out var h는 float.TryParse가 실행되는 순간,
            // h라는 이름의 float 변수를 만들고(선언) 거기에
            // 결과를 저장하라는 뜻이야. 
            var result = new CrateAbstract(world, saveData.Pos, saveData.ID)
            {
                hue = float.TryParse(p[0], out var h) ? h : 0,
                saturation = float.TryParse(p[1], out var s) ? s : 1,
                scaleX = float.TryParse(p[2], out var x) ? x : 1,
                scaleY = float.TryParse(p[3], out var y) ? y : 1,
            };

            if (unlock is SandboxUnlock u)
            {
                result.hue = u.Data / 1000f;

                if (u.Data == 0)
                {
                    result.scaleX += 0.2f;
                    result.scaleY += 0.2f;
                }
            }

            return result;
        }

        private static readonly CrateProperties properties = new();
        public override ItemProperties Properties(PhysicalObject forObject)
        {
            // If you need to use the forObject parameter,
            // pass it to your ItemProperties class's constructor.
            // The Mosquitoes example from the Fisobs github demonstrates this.
            return properties;
        }
    }
}
