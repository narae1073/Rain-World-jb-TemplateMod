using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;

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
        [HarmonyPatch(typeof(FAtlasManager), "LogAllElementNames")]
        public class Patch_LogAllElementNames
        {
            static void Prefix(FAtlasManager __instance)
            {
                Console.WriteLine("--- Logging ALL FAtlas Elements ---");

                // FAtlasManager가 관리하는 모든 아틀라스(_atlases)를 순회하며 요소를 출력해야 합니다.
                // __instance._atlases는 private 필드일 가능성이 높으므로, 리플렉션이 필요할 수 있습니다.

                // 만약 'atlases'라는 public 속성이 있다면:
                foreach (FAtlas atlas in __instance._atlases)
                {
                    Console.WriteLine($"[Atlas: {atlas.name}]");
                    foreach (FAtlasElement element in atlas.elements)
                    {
                        Console.WriteLine($" - {element.name}");
                    }
                }
                Console.WriteLine("-----------------------------------");
            }
        }
    }
}
