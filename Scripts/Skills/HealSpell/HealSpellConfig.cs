using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/Heal"))]
public class HealSpellConfig : UISpellInfo
{

    [Header("Heal Spell Specific")]
    [SerializeField] float healAmount = 3000f;

    public override SkillSystemBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
    {
        return objectToAttachTo.AddComponent<HealSpellBehaviour>();
    } 
    
    public float GetHealAmount()
    {
        return healAmount;
    }
}
