using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using Tears_Of_Void.Resources;
using Tears_Of_Void.Saving;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillSystemBehaviour : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnAbilityUse = new MyFloatEvent();
    public bool canUse = true;
    protected UISpellInfo config;
    const float PARTICLE_CLEAN_UP_DELAY = 8.0f;
    [SerializeField] public UISpellSlot slot;
    UISpellInfo spellInfo;
    public SpellCaster spell;
    public AIHealth selectedTarget;
    public Fighter fighter;
    public PlayerControls player;
    public Health playerHealth;
    public bool isPlayerCasting;
    AnimatorOverrideController animatorOverrideController;
    public Animator animator;
    public const string ATTACK_TRIGGER = "attack";
    public const string DEFAULT_ATTACK = "Sword-Attack-R6";
    //AIHealth selectedTarget;
    void Awake()
    {
        playerHealth = GetComponent<Health>();
        fighter = GetComponent<Fighter>();
        spell = GetComponent<SpellCaster>();
        player = GetComponent<PlayerControls>();
        animatorOverrideController = GetComponent<PlayerControls>().GetAnimatorOverride();
        animator = GetComponent<Animator>();
        if (this.slot == null)
            this.slot = this.GetComponent<UISpellSlot>();
    }
    public void TriggerAbility()
    {
        if (canUse)
        {
            Ability();
            //if (spellInfo.GetParticlePrefab() == null) return;
        }
    }
    public abstract void Ability();
    public abstract void Ability(AIHealth target);
    public void SetConfig(UISpellInfo configToSet)
    {
        config = configToSet;
    }
    public UISpellInfo getCurrentSlotSpellInfo()
    {
        return slot.GetSpellInfo();
    }

    public void SetTarget(AIHealth target)
    {
        selectedTarget = target;
    }
    public void isCasting(bool b)
    {
        fighter.isPlayerCasting(b);
    }
    public void StartCooldown()
    {
        StartCoroutine(Cooldown(slot));
        IEnumerator Cooldown(UISpellSlot slot)
        {
            canUse = false;
            // Make sure we have the cooldown component
            if (slot.cooldownComponent == null)
                yield return null;

            // Make sure the slot is assigned
            if (!slot.IsAssigned())
                yield return null;

            // Get the spell info from the slot
            spellInfo = slot.GetSpellInfo();
            // Handle cooldown just for the demonstration
            if (spellInfo.Cooldown > 0f)
            {
                // Start the cooldown
                slot.cooldownComponent.StartCooldown(spellInfo.ID, spellInfo.Cooldown);
            }
            yield return new WaitForSeconds(spellInfo.Cooldown);
            canUse = true;
            print("Skill ready");
        }
    }
    public void canMove(bool b)
    {
        player.setCanMove(b);
    }

    public void GetDebuff()
    {
        spell.GetDebuff();
    }
    public void SetDebuffText(int k)
    {
        spell.SetDebuffText(k);
    }

    protected void PlayParticleEffect()
    {
        var particlePrefab = config.GetParticlePrefab();
        if (config.GetParticlePrefab() == null) return;
        var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
        particleObject.transform.parent = transform;
        if (particleObject.GetComponent<ParticleSystem>() == null) return;
        particleObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyParticleWhenFinished(particleObject));
    }
    protected void PlayTargetParticleEffectWithProjectile(AIHealth target)
    {
        var particlePrefab = config.GetParticlePrefab();
        if (config.GetParticlePrefab() == null) return;
        var particleObject = Instantiate(particlePrefab, transform.position + Vector3.forward, particlePrefab.transform.rotation);
        particleObject.transform.parent = transform;
        particleObject.transform.LookAt(GetAimLocation(target));
        if (particleObject.GetComponent<ParticleSystem>() == null) return;
        particleObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyParticleWhenFinished(particleObject));
    }

    protected void PlayTargetParticleEffect(AIHealth target)
    {
        var particlePrefab = config.GetParticlePrefab();
        if (config.GetParticlePrefab() == null) return;
        var particleObject = Instantiate(particlePrefab, target.transform.position, particlePrefab.transform.rotation);
        particleObject.transform.parent = transform;
        particleObject.transform.LookAt(GetAimLocation(target));
        if (particleObject.GetComponent<ParticleSystem>() == null) return;
        particleObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyParticleWhenFinished(particleObject));
    }

    private Vector3 GetAimLocation(AIHealth target)
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
    {
        if (particlePrefab == null) yield return 0;
        while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
        {
            yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
        }
        Destroy(particlePrefab);
        yield return new WaitForEndOfFrame();
    }

    public bool AssignSpellToSlot(UISpellSlot slotID)
    {
        return slot = slotID;
    }

    protected void PlayAbilityAnimation()
    {
        // var animatorOverrideController = GetComponent<PlayerControls>().GetAnimatorOverride();
        // var animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorOverrideController;
        animatorOverrideController[DEFAULT_ATTACK] = config.GetAbilityAnimation();
        animator.SetTrigger(ATTACK_TRIGGER);
    }

    public override bool Equals(object obj)
    {
        return obj is SkillSystemBehaviour behaviour &&
               base.Equals(obj) &&
               EqualityComparer<Health>.Default.Equals(playerHealth, behaviour.playerHealth);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public object CaptureState()
    {
        throw new System.NotImplementedException();
    }

    public void RestoreState(object state)
    {
        throw new System.NotImplementedException();
    }
}
