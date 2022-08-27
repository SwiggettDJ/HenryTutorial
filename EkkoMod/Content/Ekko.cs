using BepInEx.Configuration;
using EkkoMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EkkoMod.Modules.Survivors
{
    internal class Ekko : SurvivorBase
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

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("baseIcon"),
            bodyColor = Color.white,

            //crosshair = Modules.Assets.LoadCrosshair("Standard"),
            crosshair = Assets.mainAssetBundle.LoadAsset<GameObject>("baseIcon"),
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
                childName = "Model",
             },
             new CustomRendererInfo
             {
                childName = "Glass",
             },
             new CustomRendererInfo
             {
                childName = "Board",
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
            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = EkkoPlugin.DEVELOPER_PREFIX;

            #region Primary
            //Creates a skilldef for a typical primary 
            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo(prefix + "_EKKO_BODY_PRIMARY_SLASH_NAME",
                                                                                      prefix + "_EKKO_BODY_PRIMARY_SLASH_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ekko_p2"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ekko_q"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TimeWinder)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = .5f,
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ekko_e"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PhaseDive)),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ekko_r"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Chronobreak)),
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
                Assets.mainAssetBundle.LoadAsset<Sprite>("baseIcon"),
                defaultRenderers,
                mainRenderer,
                model);


            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Base"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Glass"),
                    renderer = defaultRenderers[1].renderer
                }
            };
            defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Board").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Glass"),
                    shouldActivate = true
                }
            };

            skins.Add(defaultSkin);

           
            #endregion

            #region ArcaneSkin

            Skins.SkinDefInfo arcaneSkinDefInfo = default(Skins.SkinDefInfo);
            arcaneSkinDefInfo.Name = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_ARCANE_SKIN_NAME";
            arcaneSkinDefInfo.NameToken = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_ARCANE_SKIN_NAME";
            arcaneSkinDefInfo.Icon = Assets.mainAssetBundle.LoadAsset<Sprite>("arcaneIcon");
            arcaneSkinDefInfo.UnlockableDef = null;
            arcaneSkinDefInfo.RootObject = model;

            arcaneSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkin };
            arcaneSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            arcaneSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            //Material bodyMat = Modules.Materials.CreateHopooMaterial("Arcane Body");

            arcaneSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ArcaneBody"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ArcaneGlass"),
                    renderer = defaultRenderers[1].renderer
                }
            };

            arcaneSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(arcaneSkinDefInfo.RendererInfos, 0);

            arcaneSkinDefInfo.RendererInfos[0].defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("ArcaneBodyMat");

            arcaneSkinDefInfo.RendererInfos[1].defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("GlassMat");

            SkinDef arcaneSkin = Skins.CreateSkinDef(arcaneSkinDefInfo);

            arcaneSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Board"),
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Glass"),
                    shouldActivate = true
                }
        };
            skins.Add(arcaneSkin);

            #endregion ArcaneSkin

            #region ProjectSkin

            Skins.SkinDefInfo projectSkinDefInfo = default(Skins.SkinDefInfo);
            projectSkinDefInfo.Name = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_PROJECT_SKIN_NAME";
            projectSkinDefInfo.NameToken = EkkoPlugin.DEVELOPER_PREFIX + "_EKKO_BODY_PROJECT_SKIN_NAME";
            projectSkinDefInfo.Icon = Assets.mainAssetBundle.LoadAsset<Sprite>("projectIcon");
            projectSkinDefInfo.UnlockableDef = null;
            projectSkinDefInfo.RootObject = model;

            projectSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkin };
            projectSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            projectSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            //Material bodyMat = Modules.Materials.CreateHopooMaterial("Project Body");

            projectSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ProjectBody"),
                    renderer = defaultRenderers[0].renderer
                },
                //new SkinDef.MeshReplacement
                //{
                   // mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ArcaneGlass"),
                    //renderer = defaultRenderers[1].renderer
                //}
            };

            projectSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(projectSkinDefInfo.RendererInfos, 0);

            projectSkinDefInfo.RendererInfos[0].defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("ProjectBodyMat");

            //projectSkinDefInfo.RendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("GlassMat");

            SkinDef projectSkin = Skins.CreateSkinDef(projectSkinDefInfo);

            projectSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Board"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Glass"),
                    shouldActivate = false
                }
        };
            skins.Add(projectSkin);

            #endregion ProjectSkin

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