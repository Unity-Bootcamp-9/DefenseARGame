using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{

    public float MovingSpeed = 100.0f;
    public Vector3 targetPos = new Vector3(0, -1000, 0);

    void GoUp()
    {
        if (transform.position == Vector3.zero)
        {
            //멈춰
            return;
        }
        // 위로 올라가
        transform.position += MovingSpeed * Vector3.up * Time.deltaTime;
    }

    void GoDown()
    {
        if (transform.position == targetPos)
        {
            //멈춰
            return;
        }
        transform.position += MovingSpeed * Vector3.down * Time.deltaTime;
    }

    void Update()
    {

    }

}
