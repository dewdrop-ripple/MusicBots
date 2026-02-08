using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject mPlayerCharacter;

    private void Update()
    {
        transform.position = new Vector3(mPlayerCharacter.transform.position.x, mPlayerCharacter.transform.position.y, -10.0f);
    }
}
