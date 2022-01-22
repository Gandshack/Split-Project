using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour, IJumpComponent, ICooldownComponent
{
    #region IJump Component Reference
    /// <summary>
    /// The jump component for the Player.
    /// </summary>
    private IJumpComponent thisJumpComponent;
    #endregion

    #region ICooldown Component Reference
    /// <summary>
    /// The cooldown component for the Player's Jumping.
    /// </summary>
    private ICooldownComponent thisCooldownComponent;
    #endregion

    #region Unity Variables
    [Header("Collisions")]
    /// <summary>
    /// The Rect Transform that when overlapped lets the player know
    /// they are on the ground.
    /// </summary>
    [SerializeField]
    [Tooltip("Rect Transform for comparing if the player is on the ground.")]
    private RectTransform isGroundedChecker_Unity;

    [Header("Jumping")]
    /// <summary>
    /// The momentum for the player jumping.
    /// </summary>
    [SerializeField]
    [Tooltip("The momentum used for the player jumping.")]
    private float jumpMomentum_Unity;

    [Header("Cooldown")]
    /// <summary>
    /// The cool down timer before the player can jump again.
    /// </summary>
    [SerializeField]
    [Tooltip("The amount of time that is needed before the player can jump.")]
    private float jumpCoolDownTimer_Unity;
    #endregion

    #region Backing Variables

    /// <summary>
    /// The backing variable for if the player is Grounded.
    /// </summary>
    private bool isGrounded_Backing = false;

    /// <summary>
    /// The backing variable for if the player is Jumping.
    /// </summary>
    private bool isJumping_Backing = false;

    /// <summary>
    /// The backing variable for if the player can Jump.
    /// </summary>
    private bool canJump_Backing = false;

    /// <summary>
    /// The last time we used jump.
    /// Ideally, we would split Jump into a separate component
    /// called PlayerJump, and likely have a separate interface
    /// for cooldown behaviors.
    /// </summary>
    private float lastJumpTime = 0.0f;

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


    #region IJumpComponent_Implementation
    /// <summary>
    /// Checks whether the entity (player) is on the ground.
    /// </summary>
    bool IJumpComponent.IsGrounded
    {
        get
        {
            return isGrounded_Backing;
        }
        set
        {
            isGrounded_Backing = value;
        }
    }

    /// <summary>
    /// The momentum for which the entity (player) can jump.
    /// </summary>
    float IJumpComponent.JumpMomentum
    {
        get
        {
            return jumpMomentum_Unity;
        }
        set
        {
            jumpMomentum_Unity = value;
        }
    }

    /// <summary>
    /// Is the entity (player) jumping.
    /// </summary>
    bool IJumpComponent.IsJumping
    {
        get
        {
            return isJumping_Backing;
        }
        set
        {
            isJumping_Backing = value;
        }
    }

    /// <summary>
    /// Handles the entity (player) jump functiontionality. 
    /// </summary>
    void IJumpComponent.Jump()
    {
        thisCooldownComponent.IsOnCoolDown = true;
        lastJumpTime = Time.time;
        Vector2 vel2 = new Vector2(0, jumpMomentum_Unity);
        playerRigidBody.AddForce(vel2);
    }

    #endregion

    #region ICooldownComponent_Implementation
    /// <summary>
    /// Whether or not the action (jumping) is on cool down.
    /// </summary>
    bool ICooldownComponent.IsOnCoolDown 
    {
        get
        {
            return canJump_Backing;
        }
        set
        {
            canJump_Backing = value;
        }
    }
    
    /// <summary>
    /// The amount of time needed to wait to perform the action (jumping) again.
    /// </summary>
    float ICooldownComponent.CooldownTimeNeeded 
    { 
        get
        {
            return jumpCoolDownTimer_Unity;
        }
        set
        {
            jumpCoolDownTimer_Unity = value;
        }
    }

    /// <summary>
    /// The last time the action (jump) was performed.
    /// </summary>
    float ICooldownComponent.LastTimeActionUsed 
    {
        get
        {
            return lastJumpTime;
        }
        set
        {
            lastJumpTime = value;
        }
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
    /// Handles if the player is blocked.
    /// </summary>
    private void HandleBlocking()
    {
        if (CheckIfBlocked(isGroundedChecker_Unity))
        {
            thisJumpComponent.IsGrounded = true;
        }
        else
        {
            thisJumpComponent.IsGrounded = false;
        }
    }

    /// <summary>
    /// Handles input for making the player jump.
    /// </summary>
    private void HandleJumpInput()
    {
        // Can jump a little bit too early and still succeed
        if ((Input.GetAxisRaw("Vertical") > 0) ^ Input.GetKey(KeyCode.Space))
        {
            if (thisJumpComponent.IsGrounded && !thisCooldownComponent.IsOnCoolDown)
            {
                thisJumpComponent.Jump();
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
        thisJumpComponent = this;
        thisCooldownComponent = this;
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update function is called every frame.
    /// This handles when the player is blocked.
    /// </summary>
    public void Update()
    {
        HandleCooldown(Time.time);
    }

    /// <summary>
    /// Fixed Update is called on a set interval of frames.
    /// This handles our input functions and our character
    /// movement.
    /// </summary>
    public void FixedUpdate()
    {
        HandleBlocking();
        HandleJumpInput();
    }

    /// <summary>
    /// Handles the jump cooldown timer.
    /// </summary>
    public void HandleCooldown(float currentTime)
    {
        if (currentTime > thisCooldownComponent.LastTimeActionUsed + thisCooldownComponent.CooldownTimeNeeded)
        {
            thisCooldownComponent.IsOnCoolDown = false;
        }
    }

    #endregion

}
