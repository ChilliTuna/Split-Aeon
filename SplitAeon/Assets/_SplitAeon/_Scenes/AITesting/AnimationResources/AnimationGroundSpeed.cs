using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationGroundSpeed : MonoBehaviour
{
    public Vector2 direction = Vector2.up;

    public float speed = 5.0f;
    float m_timer = 0.0f;

    Material m_material;

    Vector2 m_offset = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        TextureAnimate();
    }

    void TextureAnimate()
    {
        m_timer += speed * Time.deltaTime;
        if (m_timer > 1.0f)
        {
            m_timer -= 1.0f;
        }
        m_offset = direction * m_timer;

        m_material.SetTextureOffset("_BaseColorMap", m_offset);
    }

    void PositionAnimate()
    {
        m_timer += speed * Time.deltaTime;
        if (m_timer > 1.0f)
        {
            m_timer -= 1.0f;
        }
        m_offset = direction * m_timer;

        Vector3 position = Vector3.zero;
        position.x = m_offset.x;
        position.z = m_offset.y;
        transform.position = position;
    }
}
