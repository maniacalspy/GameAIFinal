using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundEnemy : MonoBehaviour
{
    public UnityAction PlayerFoundAction;
    float MoveSpeed;
    GameObject Player;
    Vector3 SearchPoint;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SearchPoint = new Vector3(0, 0, 0);
        Player = GameObject.Find("Player");
        MoveSpeed = 5f;
        PlayerFoundAction += OnPlayerFound;
    }

    // Update is called once per frame
    void Update()
    {
        if (SearchPoint.magnitude > 0)
        {
            rb.AddForce(SearchPoint - transform.position);
            if (Mathf.Abs((SearchPoint - transform.position).magnitude) < .5) SearchPoint = new Vector3(0, 0, 0);
            //rb.MovePosition((SearchPoint));
        }
    }

    void OnPlayerFound()
    {
        GetComponent<Renderer>().material.color = Color.red;
        SearchPoint = Player.transform.position;
    }

    void OnPlayerLost()
    {

    }
}
