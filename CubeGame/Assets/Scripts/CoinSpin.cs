using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    public float spinSpeed;
    public static Vector3 startPos;
    public float travelDistance;
    public bool backandForth;
    public bool reverse;

    private void Start()
    {
        startPos = transform.position;
    }
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * spinSpeed);
        if(backandForth)
        {
            if(reverse)
            {
                transform.position = new Vector3(startPos.x - Mathf.PingPong(Time.time*6f, travelDistance), transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(startPos.x + Mathf.PingPong(Time.time*6f, travelDistance), transform.position.y, transform.position.z);
            }
        }
    }
}
