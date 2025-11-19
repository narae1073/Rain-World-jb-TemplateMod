using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LizardCosmetics;

namespace My_Nice_Rain_World_Mod.Creatures
{
    sealed class LavenderLizardGraphics : LizardGraphics
    {
        // this determines what your lizard's cosmetics will be. the last two are random (ShortBodyScales and TailGeckoScales), the first is guaranteed (TailTuft)
        // also if you need to know what cosmetics there are, open dnspy and go into the LizardCosmetics namespace in AssemblyCSharp
        public LavenderLizardGraphics(LavenderLizard ow) : base(ow)
        {
            //var state = UnityEngine.Random.state;
            //UnityEngine.Random.InitState(ow.abstractPhysicalObject.ID.RandomSeed);
            var spriteIndex = startOfExtraSprites + extraSprites;
            //spriteIndex = AddCosmetic(spriteIndex, new TailTuft(this, spriteIndex));
            spriteIndex = AddCosmetic(spriteIndex, new TailFin(this, spriteIndex));
            //spriteIndex = AddCosmetic(spriteIndex, new TailGeckoScales(this, spriteIndex));
            spriteIndex = AddCosmetic(spriteIndex, new LongHeadScales(this, spriteIndex));
            spriteIndex = AddCosmetic(spriteIndex, new LongShoulderScales(this, spriteIndex));
            //spriteIndex = AddCosmetic(spriteIndex, new ShortBodyScales(this, spriteIndex));
            //spriteIndex = AddCosmetic(spriteIndex, new Antennae(this, spriteIndex));

            //UnityEngine.Random.state = state;
        }
    }
}
