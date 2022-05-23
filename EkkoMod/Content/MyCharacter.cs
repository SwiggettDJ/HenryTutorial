using BepInEx.Configuration;
using EkkoMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EkkoMod.Modules.Survivors
{
    internal class MyCharacter : SurvivorBase
    {
        public override string bodyName => "Ekko";

        public const string EKKO_PREFIX = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => EKKO_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "EkkoBody",
            bodyNameToken = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_NAME",
            subtitleNameToken = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_NAME_SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("ekkoIcon"),
            bodyColor = Color.white,

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
             new CustomRendererInfo
                {
                    childName = "MainHurtbox",
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                }
        };

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new EkkoItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            //example of how to create a hitbox
            //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            //Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = EkkoPlugin.DEVELOPER_PREFIX;

            #region Primary
            //Creates a skilldef for a typical primary 
            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo(prefix + "_EKKO_BODY_PRIMARY_SLASH_NAME",
                                                                                      prefix + "_EKKO_BODY_PRIMARY_SLASH_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)),
                                                                                      "Weapon",
                                                                                      true));


            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
            #endregion

            #region Secondary
            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_EKKO_BODY_SECONDARY_GUN_NAME",
                skillNameToken = prefix + "_EKKO_BODY_SECONDARY_GUN_NAME",
                skillDescriptionToken = prefix + "_EKKO_BODY_SECONDARY_GUN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, shootSkillDef);
            #endregion

            #region Utility
            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_EKKO_BODY_UTILITY_ROLL_NAME",
                skillNameToken = prefix + "_EKKO_BODY_UTILITY_ROLL_NAME",
                skillDescriptionToken = prefix + "_EKKO_BODY_UTILITY_ROLL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Roll)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
            #endregion

            #region Special
            SkillDef bombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_EKKO_BODY_SPECIAL_BOMB_NAME",
                skillNameToken = prefix + "_EKKO_BODY_SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "_EKKO_BODY_SPECIAL_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, bombSkillDef);
            #endregion
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("ekkoIcon"),
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
            //place your mesh replacements here
            //unnecessary if you don't have multiple skins
            //new SkinDef.MeshReplacement
            //{
            //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshEkkoGun"),
            //    renderer = defaultRenderers[1].renderer
            //},
            //new SkinDef.MeshReplacement
            //{
            //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshEkko"),
            //    renderer = defaultRenderers[2].renderer
            //},
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Base"),
                    renderer = mainRenderer
                }
            };
            skins.Add(defaultSkin);

            //SkinDef arcaneSkin = Modules.Skins.CreateSkinDef(EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_MASTERY_SKIN_NAME",
                //Assets.mainAssetBundle.LoadAsset<Sprite>("ekkoIcon"),
                //defaultRenderers,
               // mainRenderer,
                //model);

           
            #endregion

            #region ArcaneSkin

            Skins.SkinDefInfo arcaneSkinDefInfo = default(Skins.SkinDefInfo);
            arcaneSkinDefInfo.Name = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_MASTERY_SKIN_NAME";
            arcaneSkinDefInfo.NameToken = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_MASTERY_SKIN_NAME";
            arcaneSkinDefInfo.Icon = Assets.mainAssetBundle.LoadAsset<Sprite>("ekkoIcon");
            arcaneSkinDefInfo.UnlockableDef = null;
            //arcaneSkinDefInfo.RootObject = Assets.LoadSurvivorModel("mdlEkkoArcane");
            arcaneSkinDefInfo.RootObject = model;

            arcaneSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkin };
            arcaneSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            arcaneSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            arcaneSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
           
             arcaneSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Arcane"),
                    renderer = mainRenderer
                },
            }; 

            arcaneSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(arcaneSkinDefInfo.RendererInfos, 0);

            arcaneSkinDefInfo.RendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("Arcane Body");

            arcaneSkinDefInfo.RendererInfos[arcaneSkinDefInfo.RendererInfos.Length - 1].defaultMaterial = Modules.Materials.CreateHopooMaterial("Arcane Body");

            SkinDef arcaneSkin = Skins.CreateSkinDef(arcaneSkinDefInfo);
            skins.Add(arcaneSkin);

            #endregion arcaneSkin 

            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            Material masteryMat = Modules.Materials.CreateHopooMaterial("matEkkoAlt");
            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masteryMat,
                masteryMat,
                masteryMat,
                masteryMat
            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshEkkoSwordAlt"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshEkkoAlt"),
                    renderer = defaultRenderers[2].renderer
                }
            };

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}