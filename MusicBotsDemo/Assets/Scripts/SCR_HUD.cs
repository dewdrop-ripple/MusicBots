using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerMovement mCharacter;

    [SerializeField] private GameObject mStaminaHand;
    [SerializeField] private GameObject mHealthHand;

    [SerializeField] private float mMaxRotation;
    [SerializeField] private float mMinRotation;

    private void Update()
    {
        float staminaPercent = mCharacter.GetStaminaPercent();
        float staminaRotation = ((mMaxRotation - mMinRotation) * staminaPercent) + mMinRotation;
        mStaminaHand.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, staminaRotation));

        float healthPercent = mCharacter.GetHealthPercent();
        float healthRotation = ((mMaxRotation - mMinRotation) * healthPercent) + mMinRotation;
        mHealthHand.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, healthRotation));
    }
}
