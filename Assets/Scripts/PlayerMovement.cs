using MLAPI;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] float MovementSpeed = 5f;
    [SerializeField] Transform cameraTransform;

    CharacterController charController;
    float pitch = 0f;

    void Awake()
    {
        if (!IsLocalPlayer)
        {
            cameraTransform.GetComponent<AudioListener>().enabled = false;
            cameraTransform.GetComponent<Camera>().enabled = false;
        }
        else
        {
            charController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            MovePlayer();
            Look();
        }
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * 3f;
        transform.Rotate(0, mouseX, 0);
        pitch -= Input.GetAxis("Mouse Y") * 3f;
        pitch = Mathf.Clamp(pitch, -45f, 45f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1f);
        move = transform.TransformDirection(move);
        charController.SimpleMove(move * MovementSpeed);
    }
}