using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiverController : MonoBehaviour
{
    [SerializeField]
    private List<CrossBowBoltController> m_Bolts = new List<CrossBowBoltController>();

    public void Start()
    {
        m_Bolts.ForEach(x => { x.OnPickUp(); });
    }

    public bool IsEmpty()
    {
        return m_Bolts.Count == 0;
    }

    public bool TryGetNextBolt(out CrossBowBoltController bolt)
    {
        bolt = GetNextBolt();
        return bolt != null;
    }

    public CrossBowBoltController GetNextBolt()
    {
        CrossBowBoltController bolt = null;

        if (m_Bolts.Count != 0)
        {
            bolt = m_Bolts[0];
            m_Bolts.RemoveAt(0);
        }

        return bolt;
    }

    public void AddBolt(CrossBowBoltController bolt)
    {
        m_Bolts.Add(bolt);
        bolt.OnPickUp();
        bolt.transform.parent = transform;
        bolt.transform.localPosition = Vector3.zero;
        bolt.transform.localRotation = Quaternion.identity;
    }
}
