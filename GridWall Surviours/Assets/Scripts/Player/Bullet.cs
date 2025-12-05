using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("BulletSettings")]
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float speed = 20f;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.up * speed;

        CancelInvoke();
        Invoke(nameof(DestroyBullet), lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth eh = other.gameObject.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.EnemyTakeDamage(8);
            }
        }

        DestroyBullet();
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}