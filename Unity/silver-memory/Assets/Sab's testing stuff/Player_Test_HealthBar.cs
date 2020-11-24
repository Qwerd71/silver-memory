using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Test_HealthBar : MonoBehaviour
{
    Animator m_Damage;

    public int maxHealth = 5;
    public int currentHealth;

    public HealthbarScript healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        m_Damage = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
            m_Damage.SetTrigger("Hit");

            
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
