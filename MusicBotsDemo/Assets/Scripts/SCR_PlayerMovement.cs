using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D mRigidBody;
    [SerializeField] private BoxCollider2D mCharacterCollider;
    [SerializeField] private CircleCollider2D mGroundCollider;
    [SerializeField] private GameObject mCharacter;
    [SerializeField] private GameObject mEyes;

    private float mVelocity; // Meters/Second
    private float mJumpHeight; // Meters
    private float mDashSpeed;
    private float mHealth;
    private float mStamina;

    private float mStaminaDrain;
    private float mStaminaRegen;
    private float mHealthDrain;

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
        mVelocity = 3.0f;
        mJumpHeight = 1.0f;
        mDashSpeed = 6.0f;

        mMaxHealth = 50.0f;
        mMaxStamina = 50.0f;

        mStaminaDrain = 10.0f;
        mStaminaRegen = 5.0f;
        mHealthDrain = 1.0f;
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
                if (mDirection != 0) mStamina -= (mStaminaDrain + mStaminaRegen) * Time.deltaTime;
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

        // Character Movement
        {
            switch(mDirection)
            {
                case (-1):
                    mEyes.transform.localPosition = new Vector3(-0.15f, 0.0f, 0.0f);

                    if (mIsDashing)
                    {
                        mCharacter.transform.localPosition = new Vector3(0.35f, 0.0f, 0.0f);
                        mCharacter.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -15.0f));
                    }
                    else
                    {
                        mCharacter.transform.localPosition = new Vector3(0.17f, 0.0f, 0.0f);
                        mCharacter.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -7.0f));
                    }

                    break;

                case (1):
                    mEyes.transform.localPosition = new Vector3(0.15f, 0.0f, 0.0f);

                    if (mIsDashing)
                    {
                        mCharacter.transform.localPosition = new Vector3(-0.35f, 0.0f, 0.0f);
                        mCharacter.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 15.0f));
                    }
                    else
                    {
                        mCharacter.transform.localPosition = new Vector3(-0.17f, 0.0f, 0.0f);
                        mCharacter.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 7.0f));
                    }

                    break;

                default:
                    mEyes.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    mCharacter.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    mCharacter.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                    break;
            }
        }

        // Heal/Stamina Regen
        {
            mHealth -= mHealthDrain * Time.deltaTime;

            if (mStamina < mMaxStamina) mStamina += mStaminaRegen * Time.deltaTime;
            if (mStamina < 0.0f) mStamina = 0.0f;
            if (mHealth > mMaxHealth) mHealth = mMaxHealth; 
            if (mHealth < 0.0f)
            {
                mHealth = 0.0f;
                GameOver();
            }
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

    private void GameOver()
    {
        Debug.Log("Health Hit 0");
    }

    public void Heal(float healthIncreasePerSecond)
    {
        mHealth += (healthIncreasePerSecond + mHealthDrain) * Time.deltaTime;
    }
}