using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using Tears_Of_Void.Resources;
using UnityEngine;

public class RangeDotSpellBehaviour : SkillSystemBehaviour
{

    private int appliedTimes = 0;
    Coroutine m_MyCoroutineReference;

    private void Update()
    {
        EscToCancel();
    }

    public override void Ability()
    {
        selectedTarget = fighter.GetTarget();
        if (canUse && getCurrentSlotSpellInfo().CastTime == 0)
        {
            StartCoroutine(CastSpell(selectedTarget));
        }
        else
        {
            m_MyCoroutineReference = StartCoroutine(CastSpellWithCasting(selectedTarget));
        }
    }

    IEnumerator CastSpell(AIHealth enemyTarget)
    {
        int debufftimer = (config as RangeDotSpellConfig).GetApplyDamageNTimes();
        canUse = false;
        UISpellInfo spellInfo = getCurrentSlotSpellInfo();
        PlayAbilityAnimation();
        StartCooldown();
        yield return new WaitForSeconds(0.5f);
        while (appliedTimes < (config as RangeDotSpellConfig).GetApplyDamageNTimes() && !enemyTarget.isTargetDead())
        {
            spell.GetDebuff().SetActive(true);
            spell.SetDebuffText(debufftimer);
            PlayTargetParticleEffect(enemyTarget);
            float damageToTake = (config as RangeDotSpellConfig).GetDamageAmount();
            enemyTarget.TakeDamage(gameObject, damageToTake, false);
            yield return new WaitForSeconds((config as RangeDotSpellConfig).GetDamageEveryN());
            appliedTimes++;
            debufftimer--;
        }
        appliedTimes = 0;
        spell.GetDebuff().SetActive(false);
    }
    IEnumerator CastSpellWithCasting(AIHealth enemyTarget)
    {
        int debufftimer = (config as RangeDotSpellConfig).GetApplyDamageNTimes();
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
        while (appliedTimes < (config as RangeDotSpellConfig).GetApplyDamageNTimes() && !enemyTarget.isTargetDead())
        {
            spell.GetDebuff().SetActive(true);
            spell.SetDebuffText(debufftimer);
            PlayTargetParticleEffect(enemyTarget);
            float damageToTake = (config as RangeDotSpellConfig).GetDamageAmount();
            enemyTarget.TakeDamage(gameObject, damageToTake, false);
            yield return new WaitForSeconds((config as RangeDotSpellConfig).GetDamageEveryN());
            appliedTimes++;
            debufftimer--;
        }
        appliedTimes = 0;
        spell.GetDebuff().SetActive(false);
    }

    public void EscToCancel()
    {
        if (m_MyCoroutineReference == null) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(slot.cooldownComponent.IsOnCooldown) return;
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
