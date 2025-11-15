using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevInterface;
using Fisobs.Core;
using Fisobs.Creatures;
using Fisobs.Sandbox;
using UnityEngine;


namespace TemplateMod.Creatures
{
    public class LizardCritob : Critob
    {
        public LizardCritob() : base(critobTemplate.TemplateLizard)
        {
            
            Icon = new SimpleIcon("slugcatSleeping", new Color(0.7254f, 1f, 0.9176f)); 
            LoadedPerformanceCost = 100f;
            SandboxPerformanceCost = new SandboxPerformanceCost(0.5f, 0.5f); // (linear cost, exponential cost)
            RegisterUnlock(KillScore.Configurable(6), critobTemplate.SandboxUnlockID.TemplateLizard, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat); // 죽이면 6점을 얻음 
            Console.WriteLine("Lizard Critob created!!!!!!!!!!!!!!!!!!!!!!!");
        }

        public override ArtificialIntelligence CreateRealizedAI(AbstractCreature acrit)
        {
            return new LizardAI(acrit, acrit.world);
        }

        public override Creature CreateRealizedCreature(AbstractCreature acrit)
        {
            return new Lizard(acrit, acrit.world);
        }

        public override CreatureState CreateState(AbstractCreature acrit)
        {
            return new LizardState(acrit);
        }

        public override CreatureTemplate CreateTemplate()
        {
            // return LizardBreeds.BreedTemplate(Type, StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LizardTemplate), null, null, null);
            var temp = LizardBreeds.BreedTemplate(
                type: CreatureTemplate.Type.BlackLizard,
                lizardAncestor: StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LizardTemplate),
                pinkTemplate: null,
                blueTemplate: null,
                greenTemplate: null
            );

            temp.type = Type; // Critob.Type (LizardCritob 클래스에 의해 자동으로 설정됨)
            temp.name = "Template Lizard";

            var breedparams = temp.breedParameters as LizardBreedParams;

            if (breedparams != null)
            {
                // 4. 기존 templateBreed()의 커스텀 로직을 여기에 삽입
                breedparams.tailSegments = 5;
                breedparams.standardColor = new(0.7254f, 1f, 0.9176f);

                temp.doPreBakedPathing = false;
                temp.preBakedPathingAncestor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.PinkLizard);
                temp.requireAImap = true;

                breedparams.baseSpeed = 2f;
                breedparams.terrainSpeeds[1] = new(1f, 1f, 1f, 1f);
                breedparams.terrainSpeeds[2] = new(1f, 1f, 1f, 1f);
                breedparams.terrainSpeeds[3] = new(1f, 1f, 1f, 1f);
            }

            return temp;
        }

        public override void EstablishRelationships()
        {
            var s = new Relationships(Type);
            s.Eats(CreatureTemplate.Type.Slugcat, 1f); // 두번째 매개변수는 강도
        }

        public override IEnumerable<string> WorldFileAliases()
        {
            // 월드.txt 파일 에서 두 가지 별칭 중 하나로 인식될 수 있음.
            return new string[] { "templatelizard", "templiz" };
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
            return "TLiz"; // keep this short!
        }
        public override Color DevtoolsMapColor(AbstractCreature acrit)
        {
            return new Color(0.7254f, 1f, 0.9176f);
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
