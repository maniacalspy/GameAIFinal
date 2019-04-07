using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LOSEnemy : MonoBehaviour
{
    public UnityAction PlayerFoundAction;
    GameObject Player;
    Vector3 SearchPoint;
    Rigidbody rb;
    float DetectRange;
    float FOV;
    States CurrentState;

    enum States
    {
        Idle,
        Chasing
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = States.Idle;
        rb = GetComponent<Rigidbody>();
        SearchPoint = new Vector3(0, 0, 0);
        Player = GameObject.Find("Player");
        DetectRange = 10f;
        FOV = 20;
        PlayerFoundAction += OnPlayerFound;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < DetectRange)
        {
            //Debug.Log(Vector3.Angle(Player.transform.position - transform.position, transform.forward));
            if (Vector3.Angle(Player.transform.position - transform.position, transform.forward) < FOV)
            {
                OnPlayerFound();
            }
        }

        StateUpdate();
    }

    void StateUpdate()
    {
        switch (CurrentState)
        {
            case States.Chasing:
                if (SearchPoint.magnitude > 0)
                {
                    Quaternion NewRotation = Quaternion.Euler(new Vector3(0, -Vector3.SignedAngle(Player.transform.position - transform.position, transform.forward, Vector3.up), 0) * Time.deltaTime);
                    rb.MoveRotation(rb.rotation * NewRotation);
                    rb.MovePosition(transform.position + (SearchPoint - transform.position) * Time.deltaTime);
                    if (Mathf.Abs((SearchPoint - transform.position).magnitude) < 1)
                    {
                        SearchPoint = new Vector3(0, 0, 0);
                        OnPlayerLost();
                    }
                }
                break;

        }
    }

    void OnPlayerFound()
    {
        GetComponent<Renderer>().material.color = Color.red;
        SearchPoint = Player.transform.position;
        CurrentState = States.Chasing;
    }

    void OnPlayerLost()
    {
        GetComponent<Renderer>().material.color = Color.white;
        CurrentState = States.Idle;
        //TODO: A* THE ENEMY BACK TO THEIR STARTING POSITION/NEXT WAYPOINT
    }
}
