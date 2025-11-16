using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWCustom;
using UnityEngine; // UnityEngine.Random 등을 사용하기 위해 필요
using Watcher; // LizardState.RotType 등을 사용하기 위해 필요 (LizardState가 Watcher 네임스페이스에 있다면)

namespace TemplateMod.Creatures
{
    internal class LavenderLizard : Lizard
    {
        public LavenderLizard(AbstractCreature acrit, World world) : base(acrit, world)
        {
            // 이 코드는 네가 이전에 제공한 TestLizard 예제에서 가져온 것으로,
            // 도마뱀의 색상과 돌연변이 모듈 관련 로직을 설정합니다.
            var state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(acrit.ID.RandomSeed);
            // 라벤더색
            effectColor = Custom.HSL2RGB(Custom.WrappedRandomVariation(.9f, .05f, .5f), .7f, Custom.ClampedRandomVariation(.75f, .1f, .5f));
            // rotModule과 LizardState.rotType을 사용하려면 해당 네임스페이스를 using 해야 합니다.
            // Watcher 네임스페이스를 using 했다면 LizardState에 접근 가능합니다.
            if (rotModule is LizardRotModule mod && (State as LizardState)?.rotType != LizardState.RotType.Slight)
                effectColor = Color.Lerp(effectColor, mod.RotEyeColor, (State as LizardState)?.rotType == LizardState.RotType.Opossum ? .2f : .8f);

            UnityEngine.Random.state = state;

            // 이곳에 TestLizard만의 추가 초기화 로직을 넣을 수 있습니다.
            // 예를 들어, 커스텀 모듈 추가 등.
        }

        // add graphics module
        public override void InitiateGraphicsModule() => graphicsModule ??= new LavenderLizardGraphics(this);

        // piece of code that fixes a bug with fisobs
        public override void LoseAllGrasps() => ReleaseGrasp(0);
    }
}
