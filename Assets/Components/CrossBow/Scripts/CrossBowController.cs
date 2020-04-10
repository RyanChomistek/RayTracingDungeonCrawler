using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBowController : MonoBehaviour
{
    [SerializeField]
    private QuiverController m_Quiver = null;

    [SerializeField]
    private CrossBowBoltController m_LoadedBolt = null;

    [SerializeField]
    private float m_FiringForce = 2000;

    public void Start()
    {
        ReadyNextBolt();
    }

    public bool HasLoadedBolt()
    {
        return m_LoadedBolt != null;
    }

    public void ReadyNextBolt()
    {
        if (m_Quiver.TryGetNextBolt(out CrossBowBoltController bolt))
        {
            ReadyBolt(bolt);
        }
    }

    public void ReadyBolt(CrossBowBoltController bolt)
    {
        if(bolt == null)
        {
            return;
        }

        bolt.transform.parent = transform;
        bolt.transform.localPosition = Vector3.zero;
        bolt.transform.localRotation = Quaternion.identity;
        m_LoadedBolt = bolt;
        m_LoadedBolt.OnReady();
    }

    public virtual void Fire(GameObject firedByRootObject, Collider firedByCollider)
    {
        if (m_LoadedBolt == null)
        {
            return;
        }

        m_LoadedBolt.transform.parent = null;
        m_LoadedBolt.OnFire(firedByRootObject, firedByCollider, transform.forward);
        m_LoadedBolt.GetComponent<Rigidbody>().AddForce(transform.forward * m_FiringForce);
        m_LoadedBolt = null;

        ReadyNextBolt();
    }
}
