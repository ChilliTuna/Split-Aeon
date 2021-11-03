using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMoveTransform : MonoBehaviour
{
    public Vector2 direction = Vector2.up;

    public float speed = 5.0f;

    public Vector3 bounds = Vector3.one * 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        move.x = direction.x * speed * Time.deltaTime;
        move.z = direction.y * speed * Time.deltaTime;
        transform.position += move;
    }
}
