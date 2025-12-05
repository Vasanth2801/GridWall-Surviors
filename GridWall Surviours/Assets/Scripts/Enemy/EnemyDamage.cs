using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 10;
    public float hitCooldown = 0.5f;
    public bool canAttack = true;

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canAttack)
            {
                PlayerHealth ph = other.gameObject.GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    ph.PlayerTakeDamage(damage);
                }
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        yield return new WaitForSeconds(hitCooldown);
        canAttack = true;
    }
}

