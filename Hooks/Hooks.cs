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
            // 점프 시 점프 부스트를 1.5배로 증가시키는 훅
            On.Player.Jump += (orig, self) =>
            {
                orig(self);
                self.jumpBoost = self.jumpBoost * 1.5f;
            };

            On.DebugMouse.Update += DebugMouse_Update;

            Console.WriteLine("Hooks applied successfully");
        }

        private static void DebugMouse_Update(On.DebugMouse.orig_Update orig, DebugMouse self, bool eu)
        {
            orig(self, eu);

            // 방이 AI 준비가 안 되었거나 뷰잉 중이 아니면 리턴
            if (!self.room.readyForAI || !self.room.BeingViewed) return;

            // 기존 텍스트를 가져와서 수정합니다.
            string text = self.label.text;

            // 마우스 위치의 AI 타일 정보를 가져옵니다.
            AItile aiTile = self.room.aimap.getAItile(self.pos);
            int terrainProximity = self.room.aimap.getTerrainProximity(self.pos);
            int roomSizeX = self.room.abstractRoom.size.x;
            int roomSizeY = self.room.abstractRoom.size.y;

            // 텍스트에 AI 타일 정보를 추가합니다.
            text += $"\n\n--aiTile--\n" +
                $"acc: {aiTile.acc}\n" +
                $"floorAltitude: {aiTile.floorAltitude}    smoothed: {aiTile.smoothedFloorAltitude}\n" +
                $"terrain prox: {terrainProximity}\n" +
                $"room size: {roomSizeX}x{roomSizeY}";

            self.label.text = text;
            self.label2.text = text;
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
