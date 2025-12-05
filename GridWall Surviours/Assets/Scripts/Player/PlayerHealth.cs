using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("PlayerHealth Settings")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;


    void Start()
    {
        currentHealth = maxHealth;
        
        healthSlider.maxValue = currentHealth;
        healthSlider.value = currentHealth;
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;

        if(healthSlider != null )
        {
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
