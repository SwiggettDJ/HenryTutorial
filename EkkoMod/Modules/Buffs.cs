using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EkkoMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        //internal static BuffDef armorBuff;
        internal static BuffDef phaseBuff;

        internal static void RegisterBuffs()
        {
            //armorBuff = AddNewBuff("EkkoArmorBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.white, false, false);
            phaseBuff = AddNewBuff("EkkoPhaseBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ekko_e"), Color.white, false, false);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}