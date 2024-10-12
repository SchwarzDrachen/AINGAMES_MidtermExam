using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("POWER UP ACTIVE");
            PowerUpManager.DamageIsActive = true;
            Destroy(gameObject);
        }
    }
}
