using Tears_Of_Void.Combat;
using Tears_Of_Void.Resources;

public class HealSpellBehaviour : SkillSystemBehaviour
{
    public override void Ability()
    {
        playerHealth.AddHealth((config as HealSpellConfig).GetHealAmount());
        PlayParticleEffect();
        PlayAbilityAnimation();
        StartCooldown();
    }

    public override void Ability(AIHealth target)
    {
        throw new System.NotImplementedException();
    }
}
