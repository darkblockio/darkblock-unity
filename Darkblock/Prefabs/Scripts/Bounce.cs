using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    // the assetBundle has a reference to this script

    public float speed = 1.0f;
    public float distance = 1.0f;
    public float sizeSpeed = 0.2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // move the game object up and down over time at a given speed and distance
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * speed) * distance, 0);

        // change the game objects color smoothly over time at a given speed
        GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));

        // rotate the game object around all of its local axes at a given speed
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        // scale the game object from 0.5 to 1.5 over time at a given speed with a smooth ease in and out
        transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.5f, 1.5f, 1.5f), Mathf.SmoothStep(0.0f, 1.0f, Mathf.PingPong((Time.time * sizeSpeed), 1)));



    }



}
