using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour, Interactable
{
    [SerializeField]
    private Light m_Light = null;

    [SerializeField]
    private float m_FlickerSpeed = 5;

    [SerializeField]
    Vector2 m_ColorTempRange = new Vector2(1700, 4000);

    [SerializeField]
    Vector2 m_IntensityRange = new Vector2(0, 8);

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private float m_RandomSeed;

    public void OnHover()
    {
    }

    public void OnInteract()
    {
        m_Light.enabled = !m_Light.enabled;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_RandomSeed = Random.value * 500;
    }

    // Update is called once per frame
    void Update()
    {
        float FlickerValue = Mathf.PerlinNoise(Time.time * m_FlickerSpeed, m_RandomSeed);

        //if(m_Light.useColorTemperature)
        //    m_Light.colorTemperature = Mathf.Lerp(m_ColorTempRange.x, m_ColorTempRange.y, FlickerValue);

        m_Light.intensity = Mathf.Lerp(m_IntensityRange.x, m_IntensityRange.y, FlickerValue);
    }
}
