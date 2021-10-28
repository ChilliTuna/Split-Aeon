using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestControl : MonoBehaviour
{
    [System.Serializable]
    public class AgentInfo
    {
        public Transform movingAgent;
        public Transform stationaryAgent;
        public Transform movingGround;
        [Min(0.0f)] public float speed;
        [Min(0.0001f)]public float maxSpeed;

        public float animSpeedControl;
        public AnimationCurve moveCurve;

        Vector3 m_agentOrigin;

        Animator m_movingAnim;
        Animator m_stationaryAnim;
        Material m_groundMaterial;

        float m_timer;
        Vector2 m_offset;

        public void Init()
        {
            m_agentOrigin = movingAgent.position;

            m_movingAnim = movingAgent.GetComponent<Animator>();
            m_stationaryAnim = stationaryAgent.GetComponent<Animator>();

            m_groundMaterial = movingGround.gameObject.GetComponent<Renderer>().material;
            m_timer = 0.0f;
            m_offset = Vector2.zero;
        }

        public void Update(Vector3 direction, Vector3 bounds)
        {
            TextureScroll(direction);
            MoveAgent(direction);
            SetInBounds(bounds);
            AnimUpdate();
        }

        public void TextureScroll(Vector3 direction)
        {
            m_timer += speed * Time.deltaTime;
            m_timer = Mathf.Repeat(m_timer, 1.0f);
            m_offset = -direction * m_timer;

            m_groundMaterial.SetTextureOffset("_BaseColorMap", m_offset);
        }

        public void MoveAgent(Vector3 direction)
        {
            Vector3 move = Vector3.zero;
            move.x = direction.x * speed * Time.deltaTime;
            move.z = direction.y * speed * Time.deltaTime;
            movingAgent.transform.position += move;
        }

        public void SetInBounds(Vector3 bounds)
        {
            if (movingAgent.position.z > m_agentOrigin.z + bounds.z)
            {
                Vector3 pos = movingAgent.position;
                pos.z -= bounds.z * 2;
                movingAgent.position = pos;
            }
        }

        public void AnimUpdate()
        {
            m_movingAnim.speed = animSpeedControl;
            m_stationaryAnim.speed = animSpeedControl;

            float value = speed / maxSpeed;
            value = moveCurve.Evaluate(value);
            m_movingAnim.SetFloat("moveSpeed", value);
            m_stationaryAnim.SetFloat("moveSpeed", value);
        }
    }

    public Vector2 direction = Vector2.up;
    public Vector3 bounds = Vector3.one * 5;

    public AgentInfo[] agentPacks;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var agentPack in agentPacks)
        {
            agentPack.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var agentPack in agentPacks)
        {
            agentPack.Update(direction, bounds);
        }
    }

    private void Reset()
    {
        foreach (var agentPack in agentPacks)
        {
            agentPack.animSpeedControl = 1.0f;
            agentPack.speed = 5.0f;
            agentPack.maxSpeed = 5.0f;
        }
    }

    private void OnValidate()
    {
        foreach (var agentPack in agentPacks)
        {
            if(agentPack.speed > agentPack.maxSpeed)
            {
                agentPack.speed = agentPack.maxSpeed;
            }
        }
    }
}
