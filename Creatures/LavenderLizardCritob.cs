using DevInterface;
using Fisobs.Core;
using Fisobs.Creatures;
using Fisobs.Sandbox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RWCustom;
using Watcher;

namespace TemplateMod.Creatures
{
    public class LavenderLizardCritob : Critob
    {
        public LavenderLizardCritob() : base(critobTemplate.LavenderLizard)
        {
            
            Icon = new SimpleIcon("Kill_Black_Lizard", new Color(1f, 0.714f, 0.945f)); 
            LoadedPerformanceCost = 100f;
            SandboxPerformanceCost = new SandboxPerformanceCost(0.5f, 0.5f); // (linear cost, exponential cost)
            RegisterUnlock(KillScore.Configurable(6), critobTemplate.SandboxUnlockID.LavenderLizard, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat); // 죽이면 6점을 얻음 
            Console.WriteLine("Lavender Lizard Critob created!!!!!!!!!!!!!!!!!!!!!!!");
        }

        // LizardAI()에서 패서, 여러 가지 트래커, 수퍼히어링, 덴 파인더, 행동, AI 추가
        public override ArtificialIntelligence CreateRealizedAI(AbstractCreature acrit)
        {
            
            return new LizardAI(acrit, acrit.world);
        }

        // Lizard()에서 바디청크와 바디청크 커넥션 만듦. 애니메이션 추가.
        // 혀, 목소리, 색, (점프, 블리자드) 모듈, StartUp()
        // Lizard 클래스는 도마뱀의 행동을 구현함.

        // registers the color and rot module for your lizard
        public override Creature CreateRealizedCreature(AbstractCreature acrit)
        {
            return new LavenderLizard(acrit, acrit.world);
        }

        public override CreatureState CreateState(AbstractCreature acrit)
        {
            return new LizardState(acrit);
        }

        public override CreatureTemplate CreateTemplate()
        {
            // return LizardBreeds.BreedTemplate(Type, StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LizardTemplate), null, null, null);
            var temp = LizardBreeds.BreedTemplate(
                type: CreatureTemplate.Type.BlueLizard,
                lizardAncestor: StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.BlueLizard),
                pinkTemplate: null,
                blueTemplate: null,
                greenTemplate: null
            ); 

            
            temp.type = Type; // Critob.Type (LizardCritob 클래스에 의해 자동으로 설정됨)
            temp.name = "Lavender Lizard";

            var breedparams = temp.breedParameters as LizardBreedParams;

            if (breedparams != null)
            {
                // 4. 기존 templateBreed()의 커스텀 로직을 여기에 삽입
                
                //breedparams.standardColor = new(1f, 0.714f, 0.945f);

                temp.doPreBakedPathing = false;
                temp.preBakedPathingAncestor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.WhiteLizard);
                temp.requireAImap = true;

                // For terrainSpeeds, 1 is Floor, 2 is Corridor,
                // 3 is Climbing, 4 is Walls, 5 is Ceiling.
                List<TileTypeResistance> acclist = new List<TileTypeResistance>();
                List<TileConnectionResistance> acclist2 = new List<TileConnectionResistance>();
                breedparams.baseSpeed = 1.5f;
                temp.waterPathingResistance = 5f;
                breedparams.terrainSpeeds[1] = new LizardBreedParams.SpeedMultiplier(1.5f, 1.5f, 1.5f, 1f);
                acclist.Add(new TileTypeResistance(AItile.Accessibility.Floor, 1f, PathCost.Legality.Allowed));
                breedparams.terrainSpeeds[3] = new LizardBreedParams.SpeedMultiplier(1f, 1f, 1f, 1f);
                acclist.Add(new TileTypeResistance(AItile.Accessibility.Corridor, 1.2f, PathCost.Legality.Allowed));
                breedparams.terrainSpeeds[4] = new LizardBreedParams.SpeedMultiplier(1f, 1f, 1f, 1f);
                acclist.Add(new TileTypeResistance(AItile.Accessibility.Climb, 0.8f, PathCost.Legality.Allowed));
                breedparams.terrainSpeeds[5] = new LizardBreedParams.SpeedMultiplier(0.8f, 1f, 1f, 1f);
                acclist.Add(new TileTypeResistance(AItile.Accessibility.Wall, 1f, PathCost.Legality.Allowed));
                breedparams.terrainSpeeds[6] = new LizardBreedParams.SpeedMultiplier(0.6f, 1f, 1f, 1f);
                acclist.Add(new TileTypeResistance(AItile.Accessibility.Ceiling, 1.2f, PathCost.Legality.Allowed));
                acclist2.Add(new TileConnectionResistance(MovementConnection.MovementType.DropToFloor, 10f, PathCost.Legality.Allowed));
                acclist2.Add(new TileConnectionResistance(MovementConnection.MovementType.DropToClimb, 10f, PathCost.Legality.Allowed));
                acclist2.Add(new TileConnectionResistance(MovementConnection.MovementType.ShortCut, 2f, PathCost.Legality.Allowed));
                acclist2.Add(new TileConnectionResistance(MovementConnection.MovementType.ReachOverGap, 1.1f, PathCost.Legality.Allowed));
                acclist2.Add(new TileConnectionResistance(MovementConnection.MovementType.ReachUp, 1.1f, PathCost.Legality.Allowed));
                acclist2.Add(new TileConnectionResistance(MovementConnection.MovementType.ReachDown, 1.1f, PathCost.Legality.Allowed));
                acclist2.Add(new TileConnectionResistance(MovementConnection.MovementType.CeilingSlope, 20f, PathCost.Legality.Allowed));

                // 공용?
                breedparams.bodyRadFac = 1f; //
                breedparams.bodyLengthFac = 1.2f; // 몸 길이가 길어짐
                breedparams.pullDownFac = 1f; // 아래로 잡아당기는 힘, 중력?

                // 도마뱀마다 다름
                //breedparams.biteDelay = 12;
                //breedparams.attemptBiteRadius = 400f;
                breedparams.toughness = 10f;
                //breedparams.regainFootingCounter = 10;
                breedparams.bodyMass = 1f;
                breedparams.bodySizeFac = 1f;
                //breedparams.maxMusclePower = 50f;
                //breedparams.wiggleSpeed = 0.6f;
                //breedparams.bodyStiffnes = 0.2f; // 바디청크의 elasticity에 기여. 0에서 1 사이
                //breedparams.danger = 1f;
                //// Lounge parameters
                //breedparams.canExitLounge = false;
                //breedparams.canExitLoungeWarmUp = false;
                //breedparams.findLoungeDirection = 2f;
                //breedparams.loungeDistance = 2f;
                //breedparams.preLoungeCrouch = 25;
                //breedparams.preLoungeCrouchMovement = -4f;
                //breedparams.loungeSpeed = 0.1f;
                //breedparams.loungeJumpyness = 400f;
                //breedparams.loungeDelay = 1;
                //// Vision parameters
                temp.visualRadius = 400f;
                temp.throughSurfaceVision = 1f;
                temp.movementBasedVision = 1f;
                //// 몸체
                breedparams.limbSize = 1f;
                breedparams.limbThickness = 1;
                //breedparams.liftFeet = 0.3f;
                //breedparams.feetDown = 5f;
                //breedparams.limbSpeed = 5f;
                //breedparams.limbQuickness = 0.5f;
                //breedparams.limbGripDelay = 1;
                breedparams.smoothenLegMovement = false;
                breedparams.walkBob = 3f; // 걷기 위아래 흔들림
                //// 꼬리
                breedparams.tailSegments = 10;
                breedparams.tailStiffness = 800f;
                breedparams.tailColorationStart = 0.5f;
                breedparams.tailColorationExponent = 0.2f;
                //breedparams.headSize = 1f;
                //breedparams.neckStiffness = 0.2f;
                //breedparams.jawOpenAngle = 40f;
                // 0: jaw, 1: lowerteeth, 2: upperteeth, 3: head, 4: eyes
                breedparams.headGraphics = new int[] {1, 1, 2, 0, 2};
                //breedparams.framesBetweenLookFocusChange = 50;
                //breedparams.tamingDifficulty = 1f;
                // 혀
                //breedparams.tongue = true;
            }

            
            return temp;
        }

        public override void EstablishRelationships()
        {
            var s = new Relationships(Type);
            s.Eats(CreatureTemplate.Type.Slugcat, 0); // 두번째 매개변수는 강도
            s.IsInPack(CreatureTemplate.Type.Slugcat, 1f); // 같은 종과 무리를 이룸
            s.PlaysWith(CreatureTemplate.Type.Slugcat, 1f);
            s.Eats(CreatureTemplate.Type.LanternMouse, 1f);
        }

        public override IEnumerable<string> WorldFileAliases()
        {
            // 월드.txt 파일 에서 두 가지 별칭 중 하나로 인식될 수 있음.
            return new string[] { "lavenderlizard", "lavliz" };
        }

        // 데브툴의 Room Attractiveness 패널에 보이는 모습을 설정?
        public override IEnumerable<RoomAttractivenessPanel.Category> DevtoolsRoomAttraction()
        {
            return new[]
            {
                RoomAttractivenessPanel.Category.Lizards,
                RoomAttractivenessPanel.Category.All,
                RoomAttractivenessPanel.Category.LikesInside // 실내를 좋아함
            };
        }

        // 데브툴의 맵 탭에서 보이는 이름과 색을 설정
        public override string DevtoolsMapName(AbstractCreature acrit)
        {
            return "LavLiz"; // keep this short!
        }
        public override Color DevtoolsMapColor(AbstractCreature acrit)
        {
            return new Color(1f, 0.714f, 0.945f);
        }

        /*private static CreatureTemplate templateBreed(On.LizardBreeds.orig_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate orig, CreatureTemplate.Type type, CreatureTemplate lizardAncestor, CreatureTemplate pinkTemplate, CreatureTemplate blueTemplate, CreatureTemplate greenTemplate)
        {
            if (type == critobTemplate.TemplateLizard)
            {
                // 검은 도마뱀의 매개변수를 기본으로 매개변수를 설정.
                var temp = orig(CreatureTemplate.Type.BlackLizard, lizardAncestor, pinkTemplate, blueTemplate, greenTemplate);
                // as는 타입 변환을 시도하며 실패시 null을 반환.
                // CreatureTemplate.BreedParameters를 LizardBreedParams로 변환
                // 이 범용적인 파라미터 객체를 도마뱀 전용의 상세 설정 객체로 변환하려고 시도하는 거야.
                var breedparams = (temp.breedParameters as LizardBreedParams);
                temp.type = type;
                temp.name = "Template Lizard";
                breedparams.tailSegments = 5;
                breedparams.standardColor = new(0.7254f, 1f, 0.9176f);
                temp.doPreBakedPathing = false;
                // 핑크 도마뱀의 길탐색을 사용
                temp.preBakedPathingAncestor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.PinkLizard);
                temp.requireAImap = true;
                breedparams.baseSpeed = 2f;
                // For terrainSpeeds, 1 is Floor, 2 is Corridor,
                // 3 is Climbing, 4 is Walls, 5 is Ceiling.
                breedparams.terrainSpeeds[1] = new(1f, 1f, 1f, 1f);
                breedparams.terrainSpeeds[2] = new(1f, 1f, 1f, 1f);
                breedparams.terrainSpeeds[3] = new(1f, 1f, 1f, 1f);
                Console.WriteLine("templateBreed() executed");
                return temp;
            }
            return orig(type, lizardAncestor, pinkTemplate, blueTemplate, greenTemplate);
        }*/
    }
}
