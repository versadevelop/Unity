using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using Tears_Of_Void.Resources;
using UnityEngine;

public class InstantRangeSpellBehaviour : SkillSystemBehaviour
{
 
    public override void Ability()
    {
        if (canUse)
        {
            selectedTarget = fighter.GetTarget();
            CastSpell(selectedTarget);
            StartCooldown();
        }
    }

    private void CastSpell(AIHealth enemyTarget)
    {
        canUse = false;
        UISpellInfo spellInfo = getCurrentSlotSpellInfo();
        PlayAbilityAnimation();
        PlayTargetParticleEffect(enemyTarget);
            float damageToTake = (config as InstantRangeSpellConfig).GetDamageAmount();
            enemyTarget.TakeDamage(gameObject, damageToTake, false);
    }


    public override void Ability(AIHealth target)
    {
        // //selectedTarget = fighter.GetTarget();
        // if (target == null) return;
        // if (target.isTargetDead()) return;
        // StartCoroutine(CastSpell(target));
    }
    // public override void Ability(AIHealth target)
    // {
    //     selectedTarget = fighter.GetTarget();
    //     if (selectedTarget == null) return;
    //     if (selectedTarget.isTargetDead()) return;
    //     StartCoroutine(CastSpell(selectedTarget));
    // }
}
