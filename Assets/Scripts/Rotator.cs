using UnityEngine;
using System.Collections;

//used to rotate the coins to tell the user they need to collect them
public class Rotator : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().material.color = new Color(255,215,0);
    }

    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}