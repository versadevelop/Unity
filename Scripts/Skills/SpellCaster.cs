using UnityEngine;
using System.Collections.Generic;
using DuloGames.UI;
using System;
using System.Collections;
using Tears_Of_Void.Control;
using UnityEngine.Events;
using UnityEngine.UI;
using Tears_Of_Void.Stats;
using Tears_Of_Void.Combat;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] UISpellInfo[] abilities = default;
    [SerializeField] private UISpellSlot[] slot;
    [SerializeField] Image energyBar,energyGlobe = default;
    [SerializeField] Text energyBarText = default;
    [SerializeField] float currentEnegryPoint = 100;
    [SerializeField] float regenPointsPerSeconds = 2;
    [SerializeField] private UICastBar m_CastBar;
    //Sounds
    [SerializeField] AudioClip outOfEnergy;
    [SerializeField] GameObject debuff;
    [SerializeField] Text debuffText;
    [SerializeField] Text outOfRange;
    AudioSource audioSource;
    Fighter fighter;
    AIHealth selectedTarget;
    bool fading;
    const string ATTACK_TRIGGER = "Attack";
    private const string Message = "Target is out of Range";
    float maxEnergyPoint = 100;
    float energyAsPercent { get { return currentEnegryPoint / maxEnergyPoint; } }

    void Awake()
    {
        fighter = GetComponent<Fighter>();
        if (this.slot == null)
            this.slot = this.GetComponents<UISpellSlot>();
    }
    private void Update()
    {
        RegenEnergyBar();
    }

    public UICastBar CastBar()
    {
        return m_CastBar;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AttachInitialAbilities();
    }
    public AIHealth GetAITarget()
    {
        return fighter.GetTarget();
    }
    public GameObject GetDebuff()
    {
        return debuff;
    }
    public void SetDebuffText(int timer)
    {
        debuffText.text = timer.ToString();
    }
    private void OnEnable()
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateEnergyOnLevelUp;
    }
    private void OnDisable()
    {
        GetComponent<BaseStats>().onLevelUp -= RegenerateEnergyOnLevelUp;
    }

    private void RegenerateEnergyOnLevelUp()
    {
        currentEnegryPoint = 100;
        UpdateEnergyBar();
    }
    private void RegenEnergyBar()
    {
        if (currentEnegryPoint != 100 && currentEnegryPoint < 100)
        {
            currentEnegryPoint += regenPointsPerSeconds * Time.deltaTime;
            UpdateEnergyBar();
        }
        else
        {
            return;
        }
    }
    void UpdateEnergyBar()
    {
        if (!energyBar)
        {
            Debug.Log("No Energy bar assigned, please assign energy bar for " + gameObject.name);
            return;
        }
        energyBar.fillAmount = energyAsPercent;
        energyGlobe.fillAmount = energyAsPercent;
        energyBarText.text = (Mathf.Floor(energyAsPercent * 100)).ToString() + " " + "%";
    }

    public void ConsumeEnergy(float amount)
    {
        float newEnergyPoint = currentEnegryPoint - amount;
        currentEnegryPoint = Mathf.Clamp(newEnergyPoint, 0, maxEnergyPoint);
        UpdateEnergyBar();
    }
    private void AttachInitialAbilities()
    {
        for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
        {
            abilities[abilityIndex].AttachAbilityTo(gameObject);
        }
    }
    public int GetSlotID(int index)
    {
        if (!slot[index].IsAssigned())
            throw new Exception("No assigned spell in this slot, Please Assign Spell !!");
        return slot[index].GetSpellInfo().ID - 1;
    }

    public void AttemptSpecialAbility(int abilityIndex, AIHealth target = null)
    {

        int index = slot[abilityIndex].ID - 1;
        bool canCastAbility = abilities[abilityIndex].GetAbilityBehaviour().canUse;

        if (!canCastAbility) { return; }

        float energyCost = abilities[abilityIndex].GetPowerCost();

        // for (int i = 0; i < slot.Length; i++)
        // {
        //     if (!slot[i].cooldownComponent.IsOnCooldown)
        //     {
        //         slot[i].cooldownComponent.StartGlobalCooldown(abilities[index].GetID(), 1); 
        //     }
        // }

        if (energyCost <= currentEnegryPoint)
        {
            if (!abilities[index].GetFlags().HasFlag(UISpellInfo_Flags.RequiresTarget))
            {
                ConsumeEnergy(energyCost);
                abilities[index].Ability();
            }
            else
            {
                fighter.Cancel();
                if (GetAITarget() == null) return;
                if (GetAITarget().IsDead()) return;
                if (!IsTargetInRange(index))
                {
                    if (!fading)
                    {
                        StartCoroutine(ShowMessage(Message, 2));
                    }
                    return;
                }
                ConsumeEnergy(energyCost);
                abilities[index].Ability();
            }
        }
        // else
        // {
        //     if (!audioSource.isPlaying)
        //     {
        //         audioSource.PlayOneShot(outOfEnergy);
        //     }
        // }
    }

    private IEnumerator ShowMessage(string message, float delay)
    {
        fading = false;
        float fadeOutTime = 1.5f;
        outOfRange.text = message;
        outOfRange.color = Color.red;
        outOfRange.enabled = true;
        yield return new WaitForSeconds(delay);
        fading = true;
        Color originalColor = outOfRange.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            outOfRange.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        outOfRange.enabled = false;
        fading = false;
    }

    public bool IsTargetInRange(int index)
    {
        return Vector3.Distance(GetAITarget().transform.position, transform.position) < abilities[index].GetRange();
    }
}