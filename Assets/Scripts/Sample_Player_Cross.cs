using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerCross : MonoBehaviour
{
    public float speed = 2f;
    public float jumpForce = 100f;
    public float jumpAngle = 75f;
    public GameObject controllerGo;
    private CrossInputController controller;
    private Rigidbody2D rb;

    // Should migrate to Object pool
    private Vector2 jumpVec;
    private Vector2 maxVel;
    private Vector2 currVel;
    private Vector2 lookRight;
    private bool isFlip;

    private float jumpGauge;
    private bool isFill;
    private bool isTouchGround;

    private const float GAUGE_PER_TICK = 0.01f;

    private Vector2 inverseX;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpVec = new Vector2(Mathf.Cos(jumpAngle * Mathf.Deg2Rad), Mathf.Sin(jumpAngle * Mathf.Deg2Rad)); // 45 dec
        maxVel = Vector2.right * speed;
        currVel = Vector2.zero;
        isFlip = false;
        lookRight = transform.localScale;
        jumpGauge = 0f;
        isFill = false;
        isTouchGround = false;
        inverseX = new Vector2(-1, 1);
        controller = controllerGo.GetComponent<CrossInputController>();
    }

    // Check if player touch ground -> block movement while on the air
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isTouchGround && other.gameObject.CompareTag("Ground"))
        {
            isTouchGround = true;
            currVel.y = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouchGround)
        {
            var keyboardHorizontal = controller.GetHorizontal();
            isFlip = keyboardHorizontal switch
            {
                < 0 => true,
                > 0 => false,
                _ => isFlip
            };
            transform.localScale = (isFlip ? lookRight * inverseX : lookRight);
            // Charging the jump
            if (controller.GetJump())
            {
                currVel = Vector2.zero;
                isFill = true;
                return;
            }
            if (isFill)
            {
                isFill = false;
                // Jump straight up
                if (keyboardHorizontal == 0)
                {
                    rb.AddForce(jumpGauge * jumpForce * Vector2.up, ForceMode2D.Impulse);
                }
                else // Hold left | right & jump
                {
                    rb.AddForce(jumpGauge * jumpForce * (isFlip ? jumpVec * inverseX : jumpVec), ForceMode2D.Impulse);
                }

                jumpGauge = 0;
                isTouchGround = false;
                return;
            }
            currVel = maxVel * keyboardHorizontal;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(currVel);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed * 3f);
        if (isFill)
        {
            jumpGauge = Mathf.Clamp(jumpGauge + GAUGE_PER_TICK, 0, 1);
        }
    }
}