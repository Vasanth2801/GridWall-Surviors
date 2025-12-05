using UnityEngine;
using UnityEngine.UI;

public class RangedEnemy : MonoBehaviour
{
    [Header("Chasing Settings")]
    [SerializeField] private float enemySpeed = 1f;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 0.025f;

    [Header("References")]
    [SerializeField] Rigidbody2D rb;

    [Header("Distance for the Enemy to Shoot")]
    [SerializeField] private float distanceToShoot = 5f;
    [SerializeField] private float distanceToStop = 3f;

    [Header("BulletSettings for the enemy")]
    [SerializeField] private float fireRate;
    [SerializeField] private float timer;

    [Header("References for the bullet and Firepoint")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;


    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = fireRate;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(target == null)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
        }


        if (Vector2.Distance(rb.position, target.position) <= distanceToShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if(timer<=0)
        {
            Instantiate(bullet,firePoint.position,firePoint.rotation);
            timer = fireRate;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(Vector2.Distance(transform.position,target.position) <= distanceToStop)
        {
            rb.linearVelocity =  transform.up * enemySpeed * Time.deltaTime;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void RotateTowardsTarget()
    {
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x)*Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotationSpeed);
    }

}
