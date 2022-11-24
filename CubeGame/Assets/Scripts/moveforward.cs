using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveforward : MonoBehaviour
{
    public float fallSpeed;
    
    void FixedUpdate()
    {
        StartCoroutine(FallDelay());
    }
    IEnumerator FallDelay()
    {
        yield return new WaitForSeconds(8f);
        transform.Translate(Vector3.back * Time.deltaTime * fallSpeed);


    }
}
