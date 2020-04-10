using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CrossBowBoltController : MonoBehaviour, Interactable
{
    protected Rigidbody m_RB = null;
    protected Collider m_Collider = null;

    [SerializeField]
    private bool m_IsSticky = false;

    protected GameObject m_FiredByRootObject = null;
    protected Collider m_FiredByCollider = null;

    private float m_DamageAmount = 1;

    [SerializeField]
    protected bool m_HasBeenFired = true;
    public bool HasBeenFired { get { return m_HasBeenFired; } }

    [SerializeField]
    protected bool m_HasStopped = true;

    protected bool InFlight { get { return m_HasBeenFired && !m_HasStopped; } }

    public void Awake()
    {
        m_RB = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_HasBeenFired = true;
    }

    public void FixedUpdate()
    {
        m_HasStopped = m_RB.velocity.magnitude == 0;
    }

    public virtual void OnPickUp()
    {
        m_RB.isKinematic = true;
        m_RB.detectCollisions = false;
        m_HasBeenFired = false;
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
    public virtual void OnFire(GameObject firedByRootObject, Collider firedByCollider, Vector3 FiredDirection)
    {
        m_FiredByRootObject = firedByRootObject;
        m_FiredByCollider = firedByCollider;
        m_RB.isKinematic = false;
        m_Collider.enabled = true;
        m_RB.detectCollisions = true;
        m_HasBeenFired = true;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        // Return early if the object we're hitting is the one who fired us
        if (collision.rigidbody != null && collision.rigidbody.gameObject == m_FiredByRootObject)
        {
            Physics.IgnoreCollision(m_FiredByCollider, GetComponent<Collider>());
            return;
        }

        IDamageable damageableItem = collision.gameObject.GetComponent<IDamageable>();
        if (damageableItem != null)
        {
            damageableItem.Damage(m_DamageAmount);
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
