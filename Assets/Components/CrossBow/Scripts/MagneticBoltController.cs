using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticBoltController : CrossBowBoltController
{
    [SerializeField]
    private float m_AttractionForceInitial = 5;

    [SerializeField]
    private float m_AttractionForceDuration = 1;

    private void OnTriggerEnter(Collider other)
    {
        CrossBowBoltController boltController = other.gameObject.GetComponent<CrossBowBoltController>();
        if (boltController != null && boltController.HasBeenFired)
        {
            Vector3 dir = this.transform.position - boltController.transform.position;

            boltController.OnFire(gameObject, m_Collider, dir * m_AttractionForceInitial);
            other.attachedRigidbody.velocity = dir * m_AttractionForceInitial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CrossBowBoltController boltController = other.gameObject.GetComponent<CrossBowBoltController>();
        if (boltController != null && boltController.HasBeenFired)
        {
            Vector3 dir = this.transform.position - boltController.transform.position;

            boltController.OnFire(gameObject, m_Collider, dir * m_AttractionForceDuration);
            other.attachedRigidbody.velocity = dir * m_AttractionForceDuration;
        }
    }

}
