using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public static bool DamageIsActive = false;
   
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("POWER UP ACTIVE");
            DamageIsActive = true;

            //gameObject.Vector3.Movetow = 

            StartCoroutine(DamageTimer());

            Destroy(gameObject);

        }
    }

    public IEnumerator DamageTimer()
    {
        Debug.Log("TIMER");
        yield return new WaitForSeconds(15.0f);
        PowerUp.DamageIsActive = false;
    }
}
