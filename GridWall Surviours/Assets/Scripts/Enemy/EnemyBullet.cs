using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 7f;
    Rigidbody2D rb;

    [Header("Bullet Timings")]
    [SerializeField] private float lifeOfBullet = 4f;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = lifeOfBullet;
        if(rb != null)
        {
            rb.linearVelocity = transform.up * speed;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            var ph = GetComponent<PlayerHealth>();
            if ((ph != null))
            {
                ph.PlayerTakeDamage(4);
            }
        }
    }
}
