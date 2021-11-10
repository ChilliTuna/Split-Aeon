using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShaderController : MonoBehaviour
{
    public Renderer outlineRenderer;
    public string intensityRef;

    public float targetIntensity = 0.007f;
    float m_currentIntensity;
    float m_smoothVelocity = 0.0f;
    public float smoothTime = 0.5f;
    public float stopDifference = 0.0001f;

    delegate void VoidAction();
    VoidAction actionDelegate;

    // Start is called before the first frame update
    void Start()
    {
        actionDelegate = () => { };
        m_currentIntensity = targetIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        actionDelegate.Invoke();
    }

    void SmoothIntensity()
    {
        if(Time.timeScale == 0.0f)
        {
            return;
        }

        m_currentIntensity = Mathf.SmoothDamp(m_currentIntensity, targetIntensity, ref m_smoothVelocity, smoothTime);

        if(Mathf.Abs(m_currentIntensity - targetIntensity) < stopDifference)
        {
            m_currentIntensity = targetIntensity;
            actionDelegate = () => { };
        }
        outlineRenderer.material.SetFloat(intensityRef, m_currentIntensity);
    }

    public void SetIntensity(float value)
    {
        targetIntensity = value;
        actionDelegate = SmoothIntensity;
    }
}
