using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/Fireball"))]
public class FireballConfig : UISpellInfo
{

    [Header("Fireball Specific")]
    [SerializeField] float Damage = 3000f;

    public override SkillSystemBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
    {
        return objectToAttachTo.AddComponent<FireballBehaviour>();
    } 
    
    public float GetDamageAmount()
    {
        return Damage;
    }

}
