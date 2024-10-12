using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static bool DamageIsActive = false;
    public float maxTimer = 3.0f;
    private float Timer = 0f;

    private void Update()
    {
        if (DamageIsActive)
        {
            Debug.Log("Damage Active");
            Debug.Log($"{Time.deltaTime}");
            Timer += Time.deltaTime;
            if (Timer >= maxTimer)
            {
                DamageIsActive = false;
                Debug.Log("Power Up Finished");
                Timer = 0;
            }
        }
    }
}
