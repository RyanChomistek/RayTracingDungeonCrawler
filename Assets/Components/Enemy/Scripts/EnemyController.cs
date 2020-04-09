using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public float m_MaxSpeed = 4;
    public float m_PushForce = 5;
    public float m_SightRange = 5;

    public SphereCollider m_SightTrigger;
    public Rigidbody m_RigidBody;
    public ConstantForce m_ConstantForceController;

    [SerializeField]
    private PlayerController m_VisiblePlayer = null;

    [SerializeField]
    private float m_HP = 1;

    void Awake()
    {
        m_SightTrigger.radius = m_SightRange;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_VisiblePlayer != null)
        {
            //m_RigidBody.velocity = (m_VisiblePlayer.transform.position - transform.position).normalized * m_Speed;
            //m_RigidBody.AddForce((m_VisiblePlayer.transform.position - transform.position).normalized);
            m_ConstantForceController.force = (m_VisiblePlayer.transform.position - transform.position).normalized * m_PushForce;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var playerControllerInRoot = other.gameObject.transform.root.GetComponent<PlayerController>();
        if (playerControllerInRoot)
        {
            m_VisiblePlayer = playerControllerInRoot;
        }
    }

    public void Damage(float amount)
    {
        m_HP -= amount;

        if(m_HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
