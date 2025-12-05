using Pathfinding;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<AIPath>().enabled = false;
            GetComponent<AIDestinationSetter>().enabled = false;

            Destroy(gameObject);
        }
    }
}