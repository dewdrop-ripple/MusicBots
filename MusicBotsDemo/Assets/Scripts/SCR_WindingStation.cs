using UnityEngine;

public class WindingStation : MonoBehaviour
{
    [SerializeField] private GameObject mIcon;
    private PlayerMovement mPlayer;

    private bool mIsCollidingWithPlayer;

    private float mHealSpeed;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        mIsCollidingWithPlayer = false;
        mHealSpeed = 7.5f;

        mIcon.SetActive(false);
    }

    private void Update()
    {
        if (mIsCollidingWithPlayer)
        {
            if (mPlayer.IsPlayerInteracting())
            {
                mPlayer.Heal(mHealSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mPlayer = collision.GetComponent<PlayerMovement>();
            mIsCollidingWithPlayer = true;
            mIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mIsCollidingWithPlayer = false;
            mIcon.SetActive(false);
        }
    }
}
