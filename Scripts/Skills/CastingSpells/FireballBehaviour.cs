using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using Tears_Of_Void.Resources;
using UnityEngine;

public class FireballBehaviour : SkillSystemBehaviour
{
    Coroutine m_MyCoroutineReference;

    private void Update()
    {
        EscToCancel();
    }
    public override void Ability()
    {
        if (canUse)
        {
            selectedTarget = fighter.GetTarget();
            m_MyCoroutineReference = StartCoroutine(CastSpell(selectedTarget));
        }
    }

    IEnumerator CastSpell(AIHealth enemyTarget)
    {
        canUse = false;
        UISpellInfo spellInfo = getCurrentSlotSpellInfo();
        canMove(true);
        PlayAbilityAnimation();
            // Start casting
            isCasting(true);
            spell.CastBar().StartCasting(getCurrentSlotSpellInfo(), spellInfo.CastTime, Time.time + spellInfo.CastTime);
            yield return new WaitForSeconds(spellInfo.CastTime);
            isCasting(false);
        canMove(false);
        StartCooldown();
        PlayTargetParticleEffectWithProjectile(enemyTarget);
            float damageToTake = (config as FireballConfig).GetDamageAmount();
            enemyTarget.TakeDamage(gameObject, damageToTake, false);
    }

    public void EscToCancel()
    {
        if (m_MyCoroutineReference == null) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            spell.CastBar().Interrupt();
            StopCoroutine(m_MyCoroutineReference);
            GetComponent<Animator>().SetTrigger("stopAttack");
            SetTarget(null);
            canUse = true;
            canMove(false);
        }
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
