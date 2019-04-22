using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LOSEnemy : MonoBehaviour
{
    public UnityAction PlayerFoundAction;
    AStarNav Pathfinding;
    GameObject Player;
    Vector3 SearchPoint;
    Rigidbody rb;
    List<Node> SearchPath;
    float DetectRange;
    float FOV;
    States CurrentState;

    enum States
    {
        Idle,
        Search,
        Chasing
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = States.Idle;
        rb = GetComponent<Rigidbody>();
        SearchPoint = new Vector3(0, 0, 0);
        Pathfinding = GameObject.Find("Manager").GetComponent<AStarNav>();
        SearchPath = new List<Node>();
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
                RaycastHit Hitinfo;
                Physics.Raycast(transform.position, Player.transform.position-transform.position, out Hitinfo, DetectRange);
                if (Hitinfo.collider.gameObject.tag == "Player") OnPlayerFound();
            }

        }

        StateUpdate();
    }

    void StateUpdate()
    {
        switch (CurrentState)
        {
            case States.Search:
                if (Vector3.SignedAngle(SearchPoint - transform.position, transform.forward, Vector3.up) != 0)
                {
                    Quaternion NewRotation = Quaternion.Euler(new Vector3(0, -Vector3.SignedAngle(SearchPoint - transform.position, transform.forward, Vector3.up), 0) * Time.deltaTime);
                    rb.MoveRotation(rb.rotation * NewRotation);
                }
                else {
                    CurrentState = States.Idle;
                    SearchPoint = Vector3.zero;
                }

                break;
            case States.Chasing:
                if (SearchPoint.magnitude > 0 && SearchPath.Count > 0)
                {
                    Vector3 Waypoint = SearchPath[0].position; 
                    Quaternion NewRotation = Quaternion.Euler(new Vector3(0, -Vector3.SignedAngle(SearchPoint - transform.position, transform.forward, Vector3.up), 0) * 4 * Time.deltaTime);
                    rb.MoveRotation(rb.rotation * NewRotation);
                    rb.MovePosition(transform.position + (Waypoint - transform.position) * 2.5f * Time.deltaTime);
                    if (Mathf.Abs((Waypoint - transform.position).magnitude) < 3)
                    {
                        SearchPath.RemoveAt(0);
                        if (Mathf.Abs((SearchPoint - transform.position).magnitude) < 1 || SearchPath.Count == 0)
                        {
                            SearchPoint = Vector3.zero;
                            OnPlayerLost();
                        }
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
        SearchPath = Pathfinding.FindPath(transform.position, SearchPoint);
    }

    void OnPlayerLost()
    {
        GetComponent<Renderer>().material.color = Color.black;
        CurrentState = States.Search;
        SearchPoint = transform.position - transform.forward;
        //TODO: A* THE ENEMY BACK TO THEIR STARTING POSITION/NEXT WAYPOINT
    }
}
