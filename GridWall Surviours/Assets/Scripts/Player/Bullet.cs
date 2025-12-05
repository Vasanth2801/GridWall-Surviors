using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("BulletSettings")]
    [SerializeField] private float bulletTime = 7f;
    private float timer;


    private void Start()
    {
        bulletTime = timer;
    }


    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if(timer == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
