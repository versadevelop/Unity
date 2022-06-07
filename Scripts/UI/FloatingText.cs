using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] float destroyTime = 1f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
