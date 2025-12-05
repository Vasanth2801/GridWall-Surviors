using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private int facingDirection = 1;


    [Header("Shooting Settings")]
    [SerializeField] Camera cam;
    Vector2 mousePos;
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] private float bulletLineDuration = 0.05f;

    [Header("References")]
    private Rigidbody2D rb;
    PlayerController controller;
    Vector2 movement;
    TrailRenderer trailRenderer;
    [SerializeField] private EnemyHealth eh;

    [Header("DashSettings")]
    [SerializeField] private float dashSpeed = 7f;
    [SerializeField] private float dashDuration = 0.05f;
    [SerializeField] private float dashCoolDown = 2f;
    bool isDashing = false;
    bool canDash = true;
    bool dashPressed = false;

    private void Awake()
    {
        controller = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        MovementCalling();
        Dashing();
    }

    void MovementCalling()
    {
        controller.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controller.Player.Move.canceled += ctx => movement = Vector2.zero;
    }

    void Dashing()
    {
        controller.Player.Dash.performed += ctx => dashPressed = true;
    }

    private void OnEnable()
    {
        controller.Player.Enable();
    }

    private void OnDisable()
    {
        controller.Player.Disable();
    }


    private void Update()
    {
        if(isDashing)
        {
            return;
        }

        if (canDash == true && dashPressed == true)
        {
            dashPressed = false;
            StartCoroutine(Dash());
            Debug.Log("Started Dashing");
        }


        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Shoot());
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Move();

        Vector2 lookDir = (mousePos - rb.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void Move()
    {
        Vector2 move = (rb.position + movement * speed * Time.deltaTime);
        rb.MovePosition(move);
    }


    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        rb.AddForce(movement * dashSpeed, ForceMode2D.Impulse);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    IEnumerator Shoot()
    {
        RaycastHit2D hitInfo =  Physics2D.Raycast(firePoint.position, firePoint.up);

        if(hitInfo)
        {
            eh.EnemyTakeDamage(8);
            Debug.Log("Taking Damage");
            Debug.Log(hitInfo.transform.name);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.up * 100f);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(bulletLineDuration);

        lineRenderer.enabled = false;
    }
}
