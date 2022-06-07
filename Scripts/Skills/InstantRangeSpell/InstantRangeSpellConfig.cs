using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/Instant Range Spell"))]
public class InstantRangeSpellConfig : UISpellInfo
{

    [Header("Instant Range Spell Specific")]
    [SerializeField] float Damage = 5000f;

    public override SkillSystemBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
    {
        return objectToAttachTo.AddComponent<InstantRangeSpellBehaviour>();
    } 
    
    public float GetDamageAmount()
    {
        return Damage;
    }

}
