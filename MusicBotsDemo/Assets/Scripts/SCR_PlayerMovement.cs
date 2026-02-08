using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D mRigidBody;
    [SerializeField] private BoxCollider2D mCharacterCollider;
    [SerializeField] private CircleCollider2D mGroundCollider;

    private float mVelocity; // Meters/Second
    private float mJumpHeight; // Meters
    private float mDashSpeed;
    private float mHealth;
    private float mStamina;

    private float mStaminaDrain;
    private float mStaminaRegen;
    private float mHealthRegen;

    private float mMaxHealth;
    private float mMaxStamina;

    private int mDirection;
    private bool mIsInteracting;
    private bool mIsOnGround;
    private bool mIsDashing;

    private KeyCode mLeft;
    private KeyCode mRight;
    private KeyCode mUp;
    private KeyCode mDown;
    private KeyCode mInteract;
    private KeyCode mDash;

    // Called once when level starts
    private void Start()
    {
        Init();
    }

    // Intitalize all variables
    private void Init()
    {
        mRigidBody = GetComponent<Rigidbody2D>();

        RecalculatePlayerStats();
        mHealth = mMaxHealth;
        mStamina = mMaxStamina;

        mDirection = 0;
        mIsInteracting = false;
        mIsOnGround = false;
        mIsDashing = false;

        SetKeybinds();
    }

    // Change player stats based on current events
    // WILL BE CHANGED WHEN I ACTUALLY IMPLEMENT PARTS SYSTEM
    private void RecalculatePlayerStats()
    {
        mVelocity = 4.0f;
        mJumpHeight = 3.0f;
        mDashSpeed = 8.0f;

        mMaxHealth = 50.0f;
        mMaxStamina = 50.0f;

        mStaminaDrain = 10.0f;
        mStaminaRegen = 5.0f;
        mHealthRegen = 5.0f;
    }

    // Update keybinds
    // Use defaults if no value given
    private void SetKeybinds(KeyCode left = KeyCode.A, KeyCode right = KeyCode.D, KeyCode up = KeyCode.W,
        KeyCode down = KeyCode.S, KeyCode interact = KeyCode.E, KeyCode dash = KeyCode.LeftShift)
    {
        mLeft = left;
        mRight = right;
        mUp = up;
        mDown = down;
        mInteract = interact;
        mDash = dash;
    }

    // Is called once every frame
    private void Update()
    {
        // Lock rotation
        transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        // Respond to current input
        {
            mDirection = 0;
            if (Input.GetKey(mRight)) mDirection++;
            if (Input.GetKey(mLeft)) mDirection--;
            if (Input.GetKey(mDash) && mStamina > mStaminaDrain / 5.0f)
            {
                mIsDashing = true;
                mStamina -= (mStaminaDrain + mStaminaRegen) * Time.deltaTime;
            }
            else
            {
                mIsDashing= false;
            }
             Move();

            if (Input.GetKeyDown(mUp) && mIsOnGround) Jump();

            if (Input.GetKey(mInteract)) mIsInteracting = true;
            else mIsInteracting = false;
        }

        // Heal/Stamina Regen
        {
            if (mStamina < mMaxStamina) mStamina += mStaminaRegen * Time.deltaTime;
            if (mStamina < 0.0f) mStamina = 0.0f;
            if (mHealth < mMaxHealth) mHealth += mHealthRegen * Time.deltaTime;
            if (mHealth <  0.0f) mHealth = 0.0f;
        }
    }

    public bool IsPlayerInteracting()
    {
        return mIsInteracting;
    }

    private void Jump()
    {
        float jumpStrength = mJumpHeight * 2.5f * mRigidBody.gravityScale;
        mRigidBody.linearVelocity = new Vector2(mRigidBody.linearVelocity.x, jumpStrength);
    }

    private void Move()
    {
        if (mIsDashing) mRigidBody.linearVelocity = new Vector2(mDirection * mDashSpeed, mRigidBody.linearVelocity.y);
        else mRigidBody.linearVelocity = new Vector2(mDirection * mVelocity, mRigidBody.linearVelocity.y);

    }

    // Check ground collison
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile") mIsOnGround = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tile") mIsOnGround = false;
    }

    public float GetHealth() { return mHealth; }
    public float GetMaxHealth() { return mMaxHealth; }
    public float GetHealthPercent() { return mHealth / mMaxHealth; }
    public float GetStamina() { return mStamina; }
    public float GetMaxStamina() { return mMaxStamina; }
    public float GetStaminaPercent() { return mStamina / mMaxStamina; }
}