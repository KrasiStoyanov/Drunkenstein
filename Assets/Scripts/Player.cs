using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private const float initialJumpForce = 2.0f;

    private Vector2 jump;

    [Tooltip("The force with which the player would jump (type of variable - float).")]
    public float jumpForce = 2.0f;

    public bool shouldDetectForMovementKeys = false;

    private Animator animationsController;

    [SerializeField]
    private LayerMask groundLayerMask;

    public Transform groundCheck;

    private const float groundedRadius = 0.2f;

    private bool isOnGround = true;
    private bool isRunning = true;

    private GameObject shadow;

    [SerializeField]
    private float minShadowScale = 1.0f;

    [SerializeField]
    private float maxShadowScale = 1.8f;

    Rigidbody2D rb;

    // Use this for initialization
    void Awake()
    {
        animationsController = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");

        jump = new Vector2(0.0f, 8.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shouldDetectForMovementKeys)
        {
            GroundCheck();
            Jump();
        }
    }

    private void GroundCheck()
    {
        isOnGround = false;
        isRunning = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayerMask);
        foreach (Collider2D collider in colliders)
        {
            GameObject colliderGameObject = collider.gameObject;
            if (colliderGameObject != gameObject)
            {
                isOnGround = true;
                isRunning = true;
            }
        }

        animationsController.SetBool("isOnGround", isOnGround);
        animationsController.SetBool("isRunning", isRunning);
        animationsController.SetFloat("verticalSpeed", rb.velocity.y);
    }

    private void Jump()
    {
        bool shouldJump = Input.GetKeyDown(KeyCode.Space);
        bool animationsControllerIsOnGround = animationsController.GetBool("isOnGround");
        bool animationsControllerIsRunning = animationsController.GetBool("isOnGround");
        if (isOnGround && shouldJump && animationsControllerIsOnGround && animationsControllerIsRunning)
        {
            isOnGround = false;
            isRunning = false;

            animationsController.SetBool("isOnGround", isOnGround);
            animationsController.SetBool("isRunning", isRunning);

            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Change the jump force of the player.
    /// </summary>
    /// <param name="additionalJumpForce">The jump force which would be added to the additional jump force.</param>
    public void ChangeJumpForce(float additionalJumpForce)
    {
        jumpForce = initialJumpForce + additionalJumpForce;
    }
}
