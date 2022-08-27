using EkkoMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace EkkoMod.SkillStates
{
    public class SlashCombo : BaseMeleeAttack
    {
        protected string[] attackPoints = { "SwingDown", "SwingUp", "SwingCenter" };
        public override void OnEnter()
        {
            this.hitboxName = "Sword";

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.swordDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 300f;
            this.bonusForce = Vector3.zero;
            this.baseDuration = 1f;
            this.attackStartTime = .2f;
            this.attackEndTime = .4f;
            this.baseEarlyExitTime = .4f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 4f;

            //this.swingSoundString = "EkkoSwordSwing";
            //this.hitSoundString = "";
            this.muzzleString = attackPoints[swingIndex];
            this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            //this.impactSound = Modules.Assets.swordHitSoundEvent.index;

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 2) index = 0;
            else index += 1;

            this.outer.SetNextState(new SlashCombo
            {
                swingIndex = index
            });
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}