using EkkoMod.SkillStates;
using EkkoMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace EkkoMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(TimeWinder));

            Modules.Content.AddEntityState(typeof(PhaseDive));

            Modules.Content.AddEntityState(typeof(Chronobreak));
        }
    }
}