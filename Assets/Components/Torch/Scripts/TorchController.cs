using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour, Interactable
{
    [SerializeField]
    private Light m_light;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OnHover()
    {
    }

    public void OnInteract()
    {
        m_light.enabled = !m_light.enabled;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
