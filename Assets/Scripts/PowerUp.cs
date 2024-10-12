using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public float extraDamage;
    public bool DamageBoostActive = false
    // Start is called before the first frame update
    void Start()
    {
        extraDamage = 5f;
    }

    public void DamageBoost()
    {
        DamageBoostActive = true;
    }
}
