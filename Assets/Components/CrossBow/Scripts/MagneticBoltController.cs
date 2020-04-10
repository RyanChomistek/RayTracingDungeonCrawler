using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticBoltController : CrossBowBoltController
{
    private void OnTriggerEnter(Collider other)
    {
        CrossBowBoltController boltController = other.gameObject.GetComponent<CrossBowBoltController>();
        if (boltController != null && boltController.HasBeenFired)
        {
            Debug.Log(other.gameObject.name);
            boltController.OnFire(gameObject, m_Collider, m_RB.velocity.normalized);
            other.attachedRigidbody.velocity = m_RB.velocity;
        }
    }

}
