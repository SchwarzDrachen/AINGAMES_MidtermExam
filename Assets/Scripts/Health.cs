using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{ 
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private Image healthFill;

    private float currentHealth;
    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        currentHealth = maxHealth;
        healthFill.fillAmount = 1.0f;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthFill.fillAmount = currentHealth / maxHealth;
    }
}
