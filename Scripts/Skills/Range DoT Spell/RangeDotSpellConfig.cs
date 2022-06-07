using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/RangeDotSpell"))]
public class RangeDotSpellConfig : UISpellInfo
{

    [Header("Range Damage Over Time Spell Specific Configuration")]
    [SerializeField] float Damage = 1500f;
    [SerializeField] float DamageEveryNTime = 10;
    [SerializeField] int ApplyDamageNTimes = 5;


    public override SkillSystemBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
    {
        return objectToAttachTo.AddComponent<RangeDotSpellBehaviour>();
    } 
    
    public float GetDamageAmount()
    {
        return Damage;
    }

    public float GetDamageEveryN()
    {
        return DamageEveryNTime;
    }
    public int GetApplyDamageNTimes()
    {
        return  ApplyDamageNTimes;
    }


}
