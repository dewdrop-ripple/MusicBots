using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerMovement mCharacter;

    [SerializeField] private GameObject mStaminaHand;

    [SerializeField] private float mMaxRotation;
    [SerializeField] private float mMinRotation;

    private void Update()
    {
        float staminaPercent = mCharacter.GetStaminaPercent();
        float rotation = ((mMaxRotation - mMinRotation) * staminaPercent) + mMinRotation;
        mStaminaHand.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotation));

        Debug.Log("Stamina percent: " + staminaPercent);
        Debug.Log("Rotation: " + rotation);
        Debug.Log("Final Rotation: " + Quaternion.Euler(new Vector3(0.0f, 0.0f, rotation)));
    }
}
