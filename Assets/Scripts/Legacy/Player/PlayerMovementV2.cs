using UnityEngine;

/// <summary>
/// Handles all of Player Movement. TODO: Perhaps separate this
/// into separate components. (Ask team first before preceding).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementV2 : MonoBehaviour, IMovementComponent
{
    #region Constants
    /// <summary>
    /// Constant force applied for movement of the Player.
    /// </summary>
    private const int MOVEMENTFORCE = 1000;
    #endregion

    #region IMovement Component Reference
    /// <summary>
    /// The movement component for the Player.
    /// </summary>
    private IMovementComponent thisMovementComponent;
    #endregion

    #region Unity Variables

    [Header("Movement")]

    /// <summary>
    /// The movement speed of the entity's movement.
    /// </summary>
    [SerializeField]
    [Tooltip("Movement Speed of Player Character.")]
    private float movementSpeed_Unity;

    /// <summary>
    /// The maximum velocity of the entity's movement.
    /// </summary>
    [SerializeField]
    [Tooltip("Maximum Velocity of Player Character.")]
    private float maxVelocity_Unity;

    [Header("Collisions")]

    /// <summary>
    /// A Rect Transform that when overlapped lets the player know
    /// they are blocked on the Left.
    /// </summary>
    [SerializeField]
    [Tooltip("Rect Transform for comparing if the player is blocked on the left.")]
    private RectTransform isBlockedLeftedChecker_Unity;

    /// <summary>
    /// A Rect Transform that when overlapped lets the player know
    /// they are blocked on the Right.
    /// </summary>
    [SerializeField]
    [Tooltip("Rect Transform for comparing if the player is blocked on the right.")]
    private RectTransform isBlockedRightChecker_Unity;

    #endregion

    #region Backing Variables

    /// <summary>
    /// The backing variable for if the player is Blocked Left.
    /// </summary>
    private bool isBlockedLeft_Backing = false;

    /// <summary>
    /// The backing variable for if the player is Blocked Right.
    /// </summary>
    private bool isBlockedRight_Backing = false;

    #endregion

    #region Layers
    /// <summary>
    /// The physics layer corresponding with the ground.
    /// </summary>
    [SerializeField]
    [Tooltip("The physics layer that corresponds with the ground.")]
    private LayerMask groundLayer;
    #endregion

    #region Other Components
    /// <summary>
    /// The rigid body for the Player to handle physics calculations.
    /// </summary>
    private Rigidbody2D playerRigidBody;
    #endregion

    #region IMovementComponent Implementation

    /// <summary>
    /// Handles the entity's (player) movement speed.
    /// </summary>
    float IMovementComponent.MovementSpeed
    {
        get
        {
            return movementSpeed_Unity;
        }
        set
        {
            movementSpeed_Unity = value;
        }
    }

    /// <summary>
    /// Handles the entity's (player) maximum velocity.
    /// </summary>
    float IMovementComponent.MaxVelocity
    {
        get
        {
            return maxVelocity_Unity;
        }
        set
        {
            maxVelocity_Unity = value;
        }
    }

    /// <summary>
    /// Checks if the entity (player) is blocked left.
    /// </summary>
    bool IMovementComponent.IsBlockedLeft
    {
        get
        {
            return isBlockedLeft_Backing;
        }
        set
        {
            isBlockedLeft_Backing = value;
        }
    }

    /// <summary>
    /// Checks if the entity (player) is blocked right.
    /// </summary>
    bool IMovementComponent.IsBlockedRight
    {
        get
        {
            return isBlockedRight_Backing;
        }
        set
        {
            isBlockedRight_Backing = value;
        }
    }

    /// <summary>
    /// Checks if the entity (player) is moving
    /// at its maximum velocity.
    /// </summary>
    /// <returns>A bool whether or not the entity is moving at its fastest velocity.</returns>
    bool IMovementComponent.IsMovingMaxVelocity()
    {
        return Mathf.Abs(playerRigidBody.velocity.x) > thisMovementComponent.MaxVelocity;
    }

    /// <summary>
    /// Handles the entity (player) moving left.
    /// </summary>
    void IMovementComponent.MoveLeft()
    {
        Vector2 velocity = new Vector2(thisMovementComponent.MovementSpeed * Time.deltaTime, 0);
        playerRigidBody.AddForce(-MOVEMENTFORCE * velocity);
    }

    /// <summary>
    /// Handles the entity (player) moving right.
    /// </summary>
    void IMovementComponent.MoveRight()
    {
        Vector2 velocity = new Vector2(thisMovementComponent.MovementSpeed * Time.deltaTime, 0);
        playerRigidBody.AddForce(MOVEMENTFORCE * velocity);
    }

    #endregion

    #region Functions

    /// <summary>
    /// Checks if the player is blocked using the RectTransforms.
    /// </summary>
    /// <param name="isBlockedChecker"></param>
    /// <returns>Whether or not the player is blocked.</returns>
    private bool CheckIfBlocked(RectTransform isBlockedChecker)
    {
        Rect blockCheckerShape = isBlockedChecker.rect;
        Collider2D collider = Physics2D.OverlapBox(isBlockedChecker.position, blockCheckerShape.size, 0, groundLayer);
        if (collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Handles input for moving the Player left or right.
    /// </summary>
    private void HandleMovementInput()
    {
        if (Input.GetAxisRaw("Horizontal") < 0 && !thisMovementComponent.IsBlockedLeft)
        {
            thisMovementComponent.MoveLeft();
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && !thisMovementComponent.IsBlockedRight)
        {
            thisMovementComponent.MoveRight();
        }
    }

    /// <summary>
    /// Handles if the player is blocked.
    /// </summary>
    private void HandleBlocking()
    {

        if (CheckIfBlocked(isBlockedRightChecker_Unity))
        {
            thisMovementComponent.IsBlockedRight = true;
        }
        else
        {
            thisMovementComponent.IsBlockedRight = false;
        }

        if (CheckIfBlocked(isBlockedLeftedChecker_Unity))
        {
            thisMovementComponent.IsBlockedLeft = true;
        }
        else
        {
            thisMovementComponent.IsBlockedLeft = false;
        }
    }

    /// <summary>
    /// Handles how to handle movement once at max velocity.
    /// </summary>
    private void HandleMovementMaxVelocity()
    {
        if (thisMovementComponent.IsMovingMaxVelocity())
        {
            Vector2 direction = playerRigidBody.velocity;
            direction.Normalize();
            if (direction.x > 0)
            {
                playerRigidBody.velocity = new Vector2(thisMovementComponent.MaxVelocity, playerRigidBody.velocity.y);
            }
            else if (direction.x < 0)
            {
                playerRigidBody.velocity = new Vector2(-thisMovementComponent.MaxVelocity, playerRigidBody.velocity.y);
            }
        }
    }

    #endregion

    #region Unity Functions

    /// <summary>
    /// Awake function is called before first frame.
    /// This initializes our cached components
    /// and our other components.
    /// </summary>
    public void Awake()
    {
        thisMovementComponent = this;
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Fixed Update is called on a set interval of frames.
    /// This handles our input functions and our character
    /// movement.
    /// </summary>
    public void FixedUpdate()
    {
        HandleBlocking();
        HandleMovementMaxVelocity();
        HandleMovementInput();
    }

    #endregion
}
