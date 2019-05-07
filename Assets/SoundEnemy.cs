using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SoundEnemy : MonoBehaviour
{
    public UnityAction PlayerFoundAction;
    public UnityAction NotifiedAction;
    GameObject Player;
    Vector3 SearchPoint;
    Rigidbody rb;
    States CurrentState;
    NavMeshAgent thisAgent;
    

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
        PlayerFoundAction += OnPlayerFound;
        thisAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
    }
    void StateUpdate()
    {
        switch (CurrentState)
        {
            case States.Chasing:
                if (SearchPoint.magnitude > 0)
                {
                    thisAgent.SetDestination(SearchPoint);
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
    }
}
