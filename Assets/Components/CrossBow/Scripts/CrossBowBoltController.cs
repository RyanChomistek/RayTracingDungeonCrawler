using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CrossBowBoltController : MonoBehaviour, Interactable
{
    private Rigidbody m_RB;
    private Collider m_Collider;

    [SerializeField]
    private bool m_IsSticky;

    private GameObject m_FiredByRootObject = null;
    private Collider m_FiredByCollider = null;

    public void Awake()
    {
        m_RB = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
    }

    public virtual void OnPickUp()
    {
        m_RB.isKinematic = true;
        m_RB.detectCollisions = false;
    }

    /// <summary>
    /// called when the bold is placed into the crossbow but has not yet fired
    /// </summary>
    public virtual void OnReady()
    {
        m_RB.isKinematic = true;
        m_RB.detectCollisions = false;
    }

    /// <summary>
    /// called when the bolt fires
    /// </summary>
    public virtual void OnFire(GameObject firedByRootObject, Collider firedByCollider)
    {
        m_FiredByRootObject = firedByRootObject;
        m_FiredByCollider = firedByCollider;
        m_RB.isKinematic = false;
        m_Collider.enabled = true;
        m_RB.detectCollisions = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Return early if the object we're hitting is the one who fired us
        if (collision.rigidbody != null && collision.rigidbody.gameObject == m_FiredByRootObject)
        {
            Physics.IgnoreCollision(m_FiredByCollider, GetComponent<Collider>());
            return;
        }
            

        m_RB.velocity = Vector3.zero;
        if(m_IsSticky)
            m_RB.isKinematic = true;
    }

    public void OnHover()
    {
        //throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        //throw new System.NotImplementedException();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
