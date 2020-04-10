using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Movement Variables
    public Transform m_Camera = null;

    public float m_MoveSpeed = 5;
    public float m_MaxMoveSpeed = 1;
    private Vector2 m_MoveDirection = new Vector2();

    public float m_JumpForce = 5000;

    // rotations
    public float m_RotateSensitivity = .25f;
    public float m_MaxYAngle = 80f;
    #endregion

    [SerializeField]
    private GameObject m_FlashLight = null;

    [SerializeField]
    private CrossBowController m_CrossBowController = null;

    [SerializeField]
    private QuiverController m_QuiverController = null;

    [SerializeField]
    private Collider m_BodyCollider = null;


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

    private void OnFire1(InputValue value)
    {
        m_CrossBowController.Fire(gameObject, m_BodyCollider);
    }

    private void OnInteract(InputValue value)
    {
        CastForInteractables(interactable => 
        {
            Debug.Log($"{interactable.GetGameObject().name}");

            CrossBowBoltController bolt;
            if (bolt = interactable as CrossBowBoltController)
            {
                m_QuiverController.AddBolt(bolt);

                if(!m_CrossBowController.HasLoadedBolt())
                    m_CrossBowController.ReadyNextBolt();
            }

            interactable.OnInteract();
        });
        
    }
    #endregion input handlers

    private void UpdateRotation(Transform t, Vector3 delta)
    {
        Vector3 currentRotation = t.rotation.eulerAngles;

        // convert the x rotation so that its always between -90 and 90
        currentRotation.x = Mathf.Rad2Deg * Mathf.Asin(Mathf.Sin(currentRotation.x * Mathf.Deg2Rad));

        Vector3 currentRotationClone = currentRotation;

        currentRotation.x -= delta.y * m_RotateSensitivity;
        currentRotation.y += delta.x * m_RotateSensitivity;

        //currentRotation.y = Mathf.Repeat(currentRotation.y, 360);
        //currentRotation.x %= m_MaxYAngle;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -m_MaxYAngle, m_MaxYAngle);

        t.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
    }

    private void UpdateVelocity()
    {
        Vector3 cameraMove = new Vector3(m_MoveDirection.x, 0, m_MoveDirection.y).normalized * m_MoveSpeed * Time.deltaTime;
        var compositeDelta = (transform.right * cameraMove.x) + (transform.forward * cameraMove.z);

        if(GetComponent<Rigidbody>().velocity.magnitude > m_MaxMoveSpeed)
        {
            // If velocity is too high just return
            return;
        }

        GetComponent<Rigidbody>().AddForce(compositeDelta);
    }

    void Update()
    {
        // Update body rotation
        UpdateRotation(transform, new Vector3(Mouse.current.delta.ReadValue().x,0,0));

        // Update head rotation
        UpdateRotation(m_Camera, new Vector3(0, Mouse.current.delta.ReadValue().y, 0));
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
        CheckForInteractableHover();
    }

    private void CheckForInteractableHover()
    {
        CastForInteractables(x => x.OnHover());
    }

    private void CastForInteractables(Action<Interactable> OnHitCallback)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
        int maxDistance = 10;

        if (Physics.BoxCast(m_Camera.position, Vector3.one, m_Camera.forward, out RaycastHit hit, transform.rotation, maxDistance, layerMask))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                OnHitCallback(interactable);
            }
        }
    }
}
