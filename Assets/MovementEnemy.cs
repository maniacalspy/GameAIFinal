using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementEnemy : MonoBehaviour
{
    Plane[] TerritoryPlanes;
    float TerritoryRadius;
    Vector3 TerritoryCenter;
    GameObject Player;
    Vector3 SearchPoint;
    Rigidbody rb;
    float Detectspeed;
    States CurrentState;
    enum States
    {
        Idle,
        Chasing,
        Returning
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentState = States.Idle;
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        SearchPoint = new Vector3(0, 0, 0);
        Detectspeed = 10;
        TerritoryRadius = 5;
        TerritoryCenter = transform.position;
        SetTerritory();
    }

    // Update is called once per frame
    void Update()
    {
        if (GeometryUtility.TestPlanesAABB(TerritoryPlanes, Player.GetComponent<BoxCollider>().bounds))
        {
            CharacterController PlayerController = Player.GetComponent<CharacterController>();
            if (PlayerController.velocity.magnitude >= Detectspeed) OnPlayerFound();
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
                    rb.MovePosition(transform.position + (SearchPoint - transform.position) * Time.deltaTime);
                    if (Mathf.Abs((SearchPoint - transform.position).magnitude) < 1)
                    {
                        SearchPoint = new Vector3(0, 0, 0);
                        OnPlayerLost();
                    }
                }
                else { CurrentState = States.Returning; }
                break;
            case States.Returning:
                if (Vector3.Distance(transform.position, TerritoryCenter) < 1)
                {
                    CurrentState = States.Idle;
                }
                else
                {
                    rb.MovePosition(transform.position + (TerritoryCenter - transform.position) * Time.deltaTime);
                }
                break;

        }
    }

    void SetTerritory()
    {
        Plane FrontPlane = new Plane(-transform.forward, transform.position + (transform.forward * TerritoryRadius));
        Plane BackPlane = new Plane(transform.forward, transform.position - (transform.forward * TerritoryRadius));
        Plane RightPlane = new Plane(-transform.right, transform.position + (transform.right * TerritoryRadius));
        Plane LeftPlane = new Plane(transform.right, transform.position - (transform.right * TerritoryRadius));
        Plane Floor = new Plane(transform.up, transform.position - (transform.up * TerritoryRadius));
        Plane Roof = new Plane(-transform.up, transform.position + (transform.up * TerritoryRadius));
        TerritoryPlanes = new Plane[] { FrontPlane, BackPlane, RightPlane, LeftPlane, Floor, Roof };

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
        CurrentState = States.Returning;
    }
}
