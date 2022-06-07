using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Tears_Of_Void.Combat;
using Tears_Of_Void.Control;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/On Ground Spell"))]
public class AreaOfEffectAuraConfig : UISpellInfo
{

    [Header("Area Effect Specific")]
    [SerializeField] float radius = 5;
    [SerializeField] float damagePerSecond = 1500;

    public override SkillSystemBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
    {
        return objectToAttachTo.AddComponent<AreaOfEffectAuraBehaviour>();
    }

    public float GetRadius()
    {
        return radius;
    }
    public float GetDamageToEachTarget()
    {
        return damagePerSecond;
    }
}
