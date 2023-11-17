using System.Collections;
using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    private const float GAUGE_PER_TICK = 0.01f;
    public float speed = 2f;
    public float jumpForce = 100f;
    public float jumpAngle = 75f;
    public float distanceGroundCheck = 0.9f;

    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D bounce;
    public GameObject controllerGameObject;
    
    
    public LayerMask layerMask;
    private Animator animator;
    private Vector2 currVel;

    private Vector2 inverseX;
    private bool isFill;
    private bool isFlip;

    private float jumpGauge;

    // Should migrate to Object pool
    private Vector2 jumpVec;
    private Vector2 lookRight;
    private Vector2 maxVel;
    private Rigidbody2D rb;
    private Collider2D collider;
    private CrossInputController inputController;

    private bool raycast;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        jumpVec = new Vector2(
            Mathf.Cos(jumpAngle * Mathf.Deg2Rad),
            Mathf.Sin(jumpAngle * Mathf.Deg2Rad)
        ); // 45 dec
        maxVel = Vector2.right * speed;
        currVel = Vector2.zero;
        isFlip = false;
        lookRight = transform.localScale;
        jumpGauge = 0f;
        isFill = false;
        inverseX = new Vector2(-1, 1);
        raycast = false;
        inputController = controllerGameObject.GetComponent<CrossInputController>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        raycast = true;
    }

    private bool IsTouchGround()
    {
        if (!raycast) return false;
#if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + Vector3.down * distanceGroundCheck, Color.red);
#endif
        return Physics2D.Raycast(transform.position, Vector2.down, distanceGroundCheck, layerMask).collider != null;
    }

    // Update is called once per frame
    // TODO: Update the crouch and hit_wall animationß
    private void Update()
    {
        // Check if player is on the ground
        if (IsTouchGround())
        {
            collider.sharedMaterial = normal;
            // Check if player is moving
            var keyboardHorizontal = inputController.GetHorizontal();
            switch (keyboardHorizontal)
            {
                case < 0:
                    isFlip = true;
                    animator.Play("move");
                    break;
                case > 0:
                    isFlip = false;
                    animator.Play("move");
                    break;
                default:
                    animator.Play("idle");
                    break;
            }
            transform.localScale = isFlip ? lookRight * inverseX : lookRight;
            // Charging the jump
            if (inputController.GetJump())
            {
                currVel = Vector2.zero;
                animator.Play("crouch");
                isFill = true;
                return;
            }
            // Jump
            if (isFill)
            {
                isFill = false;
                raycast = false;
                // Jump straight up
                if (keyboardHorizontal == 0)
                    rb.AddForce(jumpGauge * jumpForce * Vector2.up, ForceMode2D.Impulse);
                else // Hold left | right & jump
                    rb.AddForce(
                        jumpGauge * jumpForce * (isFlip ? jumpVec * inverseX : jumpVec),
                        ForceMode2D.Impulse
                    );

                jumpGauge = 0;
                return;
            }

            if (keyboardHorizontal != 0)
                currVel = isFlip?maxVel*inverseX:maxVel;
            else
            {
                currVel = Vector2.zero;
            }
        }
        else
        {
            // If the rb.velocity.y > 0, the player is jumping
            // Else, it means that the player is falling down
            animator.Play(rb.velocity.y > 0f ? "jump" : "down_air");
            collider.sharedMaterial = bounce;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(currVel);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed * 3f);
        if (isFill) jumpGauge = Mathf.Clamp(jumpGauge + GAUGE_PER_TICK, 0, 1);
    }
}