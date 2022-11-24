using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //public Vector3 pos;
    //public Transform player;
    //void LateUpdate()
    //{
    //    //  transform.position = player.position;
    //    transform.position = new Vector3(pos.x, pos.y, player.position.z + pos.z);
    //}
    public Transform player;
    public Vector3 offset;
    public float delay = 1f;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + offset, delay);
    }
}
