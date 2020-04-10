using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoltController : CrossBowBoltController
{
    [SerializeField]
    private float m_DampeningPercent = .1f;
    private Vector3 m_Direction;

    public override void OnFire(GameObject firedByRootObject, Collider firedByCollider, Vector3 FiredDirection)
    {
        base.OnFire(firedByRootObject, firedByCollider, FiredDirection);

        m_Direction = FiredDirection;
    }

    public new void FixedUpdate()
    {
        if(InFlight)
        {
            // because we want the velocity after physics, we put this in fixed update
            m_Direction = m_RB.velocity;
        }

        base.FixedUpdate();
    }

    protected new void OnCollisionEnter(Collision collision)
    {
        // Return early if the object we're hitting is the one who fired us
        if (collision.rigidbody != null && collision.rigidbody.gameObject == m_FiredByRootObject)
        {
            Physics.IgnoreCollision(m_FiredByCollider, GetComponent<Collider>());
            return;
        }

        base.OnCollisionEnter(collision);

        // get the point of contact
        ContactPoint contact = collision.contacts[0];

        // reflect our old velocity off the contact point's normal vector
        Vector3 reflectedVelocity = Vector3.Reflect(m_Direction, contact.normal);

        // assign the reflected velocity back to the rigidbody
        m_RB.velocity = reflectedVelocity * (1 - m_DampeningPercent);

        // rotate the object by the same ammount we changed its velocity
        Quaternion rotation = Quaternion.FromToRotation(m_Direction, reflectedVelocity);
        transform.rotation = rotation * transform.rotation;
    }
}
