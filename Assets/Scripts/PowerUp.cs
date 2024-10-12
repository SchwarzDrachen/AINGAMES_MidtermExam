using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public static bool DamageIsActive = false;
    public PlayerTankController player;
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("POWER UP ACTIVE");
            DamageIsActive = true;

            //gameObject.Vector3.Movetow = 

            StartCoroutine(player.DamageTimer());

            Destroy(gameObject);

        }
    }
}
