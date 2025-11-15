using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib; // HarmonyLib 네임스페이스를 사용해야 해.

namespace TemplateMod
{
    internal class Hooks
    {
        public static void Apply()
        {
            On.Player.Jump += (orig, self) =>
            {
                orig(self);
                self.jumpBoost = self.jumpBoost * 1.5f;
            };

            Console.WriteLine("Hooks applied successfully");
        }

        // 🚨 타겟 클래스를 FAtlasManager로 지정
        // ... (Hooks 클래스 내부)
        /*[HarmonyPatch(typeof(FAtlasManager), "LogAllElementNames")]
        class Patch_LogAllElementNames
        {
            static void Postfix(FAtlasManager __instance)
            {
                // 1. 🚨 필드 이름을 "_atlases"로 정확히 지정하여 값 가져오기
                // 필드가 Private이지만, Harmony의 AccessTools는 Private 필드 접근을 지원해.
                List<FAtlas> allAtlases = AccessTools.Field(typeof(FAtlasManager), "_atlases")
                                                 .GetValue(__instance) as List<FAtlas>;

                Console.WriteLine("--- Logging ALL FAtlas Elements (Accessing _atlases) ---");

                if (allAtlases != null)
                {
                    foreach (FAtlas atlas in allAtlases)
                    {
                        Console.WriteLine($"[Atlas: {atlas.name}]");

                        // FAtlas.elements는 public이라고 가정하고 진행
                        foreach (FAtlasElement element in atlas.elements)
                        {
                            Console.WriteLine($" - {element.name}");
                        }
                    }
                }
                else
                {
                    // 이 로그가 뜨면 필드 이름을 다시 확인해야 함
                    Console.WriteLine("Error: Could not retrieve _atlases list via reflection.");
                }

                Console.WriteLine("-----------------------------------");
            }
        }*/
    }
}
