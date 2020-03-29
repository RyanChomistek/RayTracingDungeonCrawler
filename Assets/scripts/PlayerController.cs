using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform m_Camera;

    public float m_MoveSpeed = 5;
    public float m_MaxMoveSpeed = 1;
    private Vector2 m_MoveDirection = new Vector2();

    public float m_JumpForce = 5000;

    // rotations
    public float m_RotateSensitivity = .25f;
    public float m_MaxYAngle = 80f;

    public GameObject m_FlashLight;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
    }

    #region input handlers
    private void OnMove(InputValue value)
    {
        m_MoveDirection = value.Get<Vector2>();
    }

    private void OnMoveX(InputValue value)
    {
        m_MoveDirection.x = value.Get<float>();
    }

    private void OnMoveY(InputValue value)
    {
        m_MoveDirection.y = value.Get<float>();
    }

    private void OnJump(InputValue value)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * m_JumpForce);
    }

    private void OnToggleLight(InputValue value)
    {
        m_FlashLight.SetActive(!m_FlashLight.activeSelf);
    }
    #endregion input handlers

    private void UpdateRotation(Transform t, Vector3 delta)
    {
        Vector3 currentRotation = t.rotation.eulerAngles;
        Vector3 currentRotationClone = currentRotation;
        
        currentRotation.y += delta.x * m_RotateSensitivity;
        currentRotation.x -= delta.y * m_RotateSensitivity;
        //currentRotation.y = Mathf.Repeat(currentRotation.y, 360);
        //currentRotation.x %= m_MaxYAngle;
        //currentRotation.x = Mathf.Clamp(currentRotation.x, -m_MaxYAngle, m_MaxYAngle);

        //if (delta.magnitude > 0)
            //Debug.Log($"{currentRotationClone} {delta} {currentRotation}");

        t.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
    }

    private void UpdateVelocity()
    {
        Vector3 cameraMove = new Vector3(m_MoveDirection.x, 0, m_MoveDirection.y).normalized * m_MoveSpeed * Time.deltaTime;
        var compositeDelta = (m_Camera.right * cameraMove.x) + (m_Camera.forward * cameraMove.z);
        //Vector3 newVelocity = GetComponent<Rigidbody>().velocity + compositeDelta;

        if(GetComponent<Rigidbody>().velocity.magnitude > m_MaxMoveSpeed)
        {
            // If velocity is too high just return
            return;
        }

        GetComponent<Rigidbody>().AddForce(compositeDelta);
    }

    void Update()
    {
        UpdateVelocity();
        //Debug.Log($"Aim {Mouse.current.delta.ReadValue()}");
        UpdateRotation(transform, new Vector3(Mouse.current.delta.ReadValue().x,0,0));
        UpdateRotation(m_Camera, new Vector3(0, Mouse.current.delta.ReadValue().y, 0));
    }
}
