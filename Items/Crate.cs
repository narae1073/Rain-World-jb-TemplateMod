using My_Nice_Rain_World_Mod.Utils;
using Newtonsoft.Json.Linq;
using RWCustom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace My_Nice_Rain_World_Mod.Items
{
    public class Crate : PhysicalObject, IDrawable
    {
        // log counter for debugging
        private int logCounter = 0;
        float bodyChunkRad = 15f;
        int middleBodyChunkIndex = 0;
        float distance = 20f; // distance가 너무 길면 충돌이 일어나지 못함. 충돌이 일어나지 못하는건 elasticity와도 관계있음 
        int edgeLength = 1;
        float elasticity = 0.02f;
        public Crate(CrateAbstract abstr) : base(abstr)
        {
            float mass = 3f;
            var positions = new List<Vector2>();
            

            // Define positions for body chunks in a 3x3 grid
            /*for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    positions.Add(new Vector2(x, y) * 20);
                }
            }*/

            for (int x = -edgeLength; x <= edgeLength; x += 2)
            {
                for (int y = -edgeLength; y <= edgeLength; y += 2)
                {
                    //if (x == 3 && y == 3) continue;
                    //if (x == 1 && y == 1) continue;
                    //if (x == 1 && y == 1) continue;
                    //if (x == 3 && y == 3) continue;


                    positions.Add(new Vector2(x, y) * distance);
                }
            }
            for (int x = -(edgeLength-1); x <= (edgeLength-1); x += 2)
            {
                for (int y = -(edgeLength-1); y <= (edgeLength-1); y += 2)
                {
                    //if (x == 2 && y == 2) continue;
                    //if (x == 2 && y == 0) continue;
                    //if (x == 2 && y == 2) continue;
                    //if (x == 0 && y == 2) continue;
                    //if (x == 0 && y == 0) continue;
                    //if (x == 0 && y == 2) continue;
                    //if (x == 2 && y == 2) continue;
                    //if (x == 2 && y == 0) continue;
                    //if (x == 2 && y == 2) continue;
                    positions.Add(new Vector2(x, y) * distance);
                }
            }


            middleBodyChunkIndex = positions.Count / 2;
            Console.WriteLine($"positions 리스트 크기: {positions.Count}");

            bodyChunks = new BodyChunk[positions.Count];

            // Create body chunks
            for (int i = 0; i < bodyChunks.Length; i++)
            {
                bodyChunks[i] = new BodyChunk(this, i, Vector2.zero, bodyChunkRad, mass / bodyChunks.Length);
            }

            /*foreach (Vector2 pos in positions)
            {
                Trace.WriteLine($"BodyChunk 위치: X={pos.x}, Y={pos.y}");
            }*/

            bodyChunkConnections = new BodyChunkConnection[bodyChunks.Length * (bodyChunks.Length - 1) / 2];

            int connection = 0;

            // Create connections between body chunks
            for (int x = 0; x < bodyChunks.Length; x++)
            {
                for (int y = x + 1; y < bodyChunks.Length; y++)
                {
                    bodyChunkConnections[connection] = new BodyChunkConnection(bodyChunks[x], bodyChunks[y], Vector2.Distance(positions[x], positions[y]), BodyChunkConnection.Type.Normal, elasticity, -1f);
                    connection++;
                }
            }



            // Physical properties
            airFriction = 0.999f; // 1 being no air friction
            gravity = 0.9f; // 0 ~ 1
            bounce = 0.3f;
            surfaceFriction = 1f;
            collisionLayer = 1;
            waterFriction = 0.92f; // 1 being no water friction
            buoyancy = 0.75f;
            GoThroughFloors = false; // player can't fall through floors when pressed the down key;
        }

        // Positioning when placed in room
        public override void PlaceInRoom(Room placeRoom)
        {
            base.PlaceInRoom(placeRoom);

            // how danglefruit spawns
            Vector2 center = placeRoom.MiddleOfTile(abstractPhysicalObject.pos);
            int i = 0;
            for (int x = -edgeLength; x <= edgeLength; x+=2)
            {
                for (int y = -edgeLength; y <= edgeLength; y+=2)
                {
                    bodyChunks[i].HardSetPosition(new Vector2(x, y) * distance + center);
                    i++;
                }
            }

            for (int x = -(edgeLength-1); x <= (edgeLength-1); x+=2)
            {
                for (int y = -(edgeLength-1); y <= (edgeLength-1); y+=2)
                {
                    bodyChunks[i].HardSetPosition(new Vector2(x, y) * distance + center);
                    i++;
                }
            }
        }

        // Play sound on terrain impact
        /*public override void TerrainImpact(int chunk, IntVector2 direction, float speed, bool firstContact)
        {
            base.TerrainImpact(chunk, direction, speed, firstContact);

            if (speed > 10)
            {
                room.PlaySound(SoundID.Spear_Fragment_Bounce, bodyChunks[chunk].pos, 0.35f, 2f);
            }
        }*/

        // This initiates your sprites and what sprite they actually use in game
        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            string myFilePath = "D:\\GitRepo\\Rain-World-jb-TemplateMod\\Resources\\atlas_elements.txt";
            string[] atlasElements = ElementParser.LoadElementsFromFile(myFilePath);
            string str = null;
            Console.WriteLine($"총 {atlasElements.Length}개의 엘리먼트 로드 완료.");

            sLeaser.sprites = new FSprite[bodyChunks.Length];

            for (int i = 0; i < bodyChunks.Length; i++)
            {
                str = atlasElements[i+500];
                sLeaser.sprites[i] = new FSprite("DangleFruit0B");
            }

            AddToContainer(sLeaser, rCam, null);

        }

        // which will draw your Sprite when you’re in the room with it. 
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            for (int i = 0; i < bodyChunks.Length; i++)
            {
                var spr = sLeaser.sprites[i];
                spr.SetPosition(Vector2.Lerp(bodyChunks[i].lastPos, bodyChunks[i].pos, timeStacker) - camPos);
                spr.scale = bodyChunks[i].rad / 10f;
            }

            if (slatedForDeletetion || room != rCam.room)
                sLeaser.CleanSpritesAndRemove();
        }

        // applies the current room’s palette to your object
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            foreach (var sprite in sLeaser.sprites)
                sprite.color = palette.waterShineColor;
        }

        // FContainer? 는 널 값을 가질 수 있다는 의미
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer? newContainer)
        {
            // "왼쪽 변수가 null일 때만 오른쪽 값을 할당해라."
            newContainer ??= rCam.ReturnFContainer("Items");

            foreach (FSprite fsprite in sLeaser.sprites)
                newContainer.AddChild(fsprite);
        }


        public override void Update(bool eu)
        {
            // eu를 출력하면 true와 false가 번갈아가며 출력됨. 만약 네가 Update 함수 안에
            // if (eu) 조건문을 사용하면,
            // 그 안의 코드는 **전체 업데이트 중 절반(50%)**만 실행돼.
            base.Update(eu);
            // Console.WriteLine(eu);

            logCounter++;

            // if not grabbed by the slugcat?
            if (grabbedBy.Count == 0)
            {
                // slow down the crate's horizontal movement
                bodyChunks[middleBodyChunkIndex].vel = new Vector2(bodyChunks[middleBodyChunkIndex].vel.x *= 0.65f, bodyChunks[middleBodyChunkIndex].vel.y);

            }

            if (logCounter >= 60)
            {
                Console.WriteLine($"middleBodyChunk.vel: {bodyChunks[middleBodyChunkIndex].vel}");
                logCounter = 0;
            }
        }

        // Deal damage to creatures on collision
        public override void Collide(PhysicalObject otherObject, int myChunk, int otherChunk)
        {
            base.Collide(otherObject, myChunk, otherChunk);
            if (otherObject.bodyChunks[otherChunk].owner is Creature creature && !creature.dead)
            {
                float damage = (this.bodyChunks[myChunk].vel.x + this.bodyChunks[myChunk].vel.y) / 2;
                Console.WriteLine($"Damage value: {damage}");
                if (damage < 0)
                {
                    damage *= -1;
                }
                else if (damage < 1)
                {
                    // Don't deal damage
                }
                else
                {
                    //creature.Violence(this.bodyChunks[myChunk], this.bodyChunks[myChunk].vel, otherObject.bodyChunks[otherChunk], null, Creature.DamageType.Blunt, damage, 5f);
                }
            }
        }
    }
}