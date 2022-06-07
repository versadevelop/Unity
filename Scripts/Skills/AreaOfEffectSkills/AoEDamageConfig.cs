using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/Area Of Effects"))]
public class AoEDamageConfig : UISpellInfo
{

    [Header("Area Effect Specific")]
    [SerializeField] float radius = 5;
    [SerializeField] float damageToEachTarget = 15;

    public override SkillSystemBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
    {
        return objectToAttachTo.AddComponent<AoEDamageBehaviour>();
    }

    public float GetRadius()
    {
        return radius;
    }
    public float GetDamageToEachTarget()
    {
        return damageToEachTarget;
    }
}
