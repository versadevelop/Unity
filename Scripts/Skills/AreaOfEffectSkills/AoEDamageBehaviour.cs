using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using UnityEngine;
public class AoEDamageBehaviour : SkillSystemBehaviour
{
    public override void Ability()
    {
        DealRadialDamage();
        PlayAbilityAnimation();
        PlayParticleEffect();
        StartCooldown();
    }

    public override void Ability(AIHealth target)
    {
        throw new System.NotImplementedException();
    }

    private void DealRadialDamage()
    {
        //static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as AoEDamageConfig).GetRadius(),
                Vector3.up,
                (config as AoEDamageConfig).GetRadius()
            );
        foreach (RaycastHit hit in hits)
        {
            var damagable = hit.collider.gameObject.GetComponent<AIHealth>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerControls>();
            if (damagable != null && !hitPlayer)
            {
                float damageToTake = (config as AoEDamageConfig).GetDamageToEachTarget();
                damagable.TakeDamage(gameObject, damageToTake, false); // Spells don't have critical hits
            }
        }
    }
}
