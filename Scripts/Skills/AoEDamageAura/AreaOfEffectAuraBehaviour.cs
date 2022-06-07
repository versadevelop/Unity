using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using Tears_Of_Void.Resources;
using UnityEngine;
public class AreaOfEffectAuraBehaviour : SkillSystemBehaviour
{
    public int timerSeconds = 5;

    public override void Ability()
    {
        InvokeRepeating("DealAuraDamage", 0, 1);
        timerSeconds = 5;
        PlayAbilityAnimation();
        StartCooldown();
    }

    void DealAuraDamage()
    {
        timerSeconds--;
        PlayParticleEffect();
        if (timerSeconds < 1)
        {
            CancelInvoke("DealAuraDamage");
        }
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            (config as AreaOfEffectAuraConfig).GetRadius());
        foreach (Collider hit in hits)
        {
            var damagable = hit.gameObject.GetComponent<AIHealth>();
            bool hitPlayer = hit.gameObject.GetComponent<PlayerControls>();
            if (damagable != null && !hitPlayer)
            {
                float damageToTake = (config as AreaOfEffectAuraConfig).GetDamageToEachTarget();
                damagable.TakeDamage(gameObject, damageToTake, false); // Spells don't have critical hits
            }
        }
    }

    public override void Ability(AIHealth target)
    {
        throw new System.NotImplementedException();
    }
}


