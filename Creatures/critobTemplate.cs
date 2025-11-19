using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Nice_Rain_World_Mod.Creatures
{
    public static class critobTemplate
    {
        // true는 샌드박스에서 사용될 수 있음을 의미
        // 네 코드에서: nameof(TemplateLizard)는 빌드가 될 때
        // "TemplateLizard"라는 문자열 상수로 대체돼.
        // 이 코드는 **"TemplateLizard"**라는 문자열을 고유 ID로 사용하는,
        // 샌드박스 지원이 되는 새로운 크리처 타입을 정의하는 거야.
        public static CreatureTemplate.Type LavenderLizard = new(nameof(LavenderLizard), true);

        public static void UnregisterValues()
        {
            if (LavenderLizard != null)
            {
                LavenderLizard.Unregister();
                LavenderLizard = null;
            }
        }

        // 아레나에서 열 수 있게 함 
        public static class SandboxUnlockID
        {
            public static MultiplayerUnlocks.SandboxUnlockID LavenderLizard = new(nameof(LavenderLizard), true);

            public static void UnregisterValues()
            {
                if (LavenderLizard != null)
                {
                    LavenderLizard.Unregister();
                    LavenderLizard = null;
                }
            }
        }


    }



}
