using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    float BaseMoveSpeed, MoveSpeed;
    float SoundRadius;
    CharacterController thisController;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.gray;
        SoundRadius = 0;
        BaseMoveSpeed = 10f;
        thisController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float XMoveSpeed = BaseMoveSpeed * Input.GetAxis("Horizontal");
        float YMoveSpeed = BaseMoveSpeed * Input.GetAxis("Vertical");
        MoveSpeed = BaseMoveSpeed;
        if (Input.GetAxis("Walk") != 0)
        {
            XMoveSpeed /= 2;
            YMoveSpeed /= 2;
        }
        else if (Input.GetAxis("Sprint") != 0)
        {

            XMoveSpeed *= 2;
            YMoveSpeed *= 2;
        }
        Vector3 MoveVector = new Vector3(XMoveSpeed, 0, YMoveSpeed);
        MoveSpeed = MoveVector.magnitude;
        thisController.SimpleMove(MoveVector);
        if (MoveSpeed > BaseMoveSpeed/2)
        {
            SoundRadius = MoveSpeed/2;
        }
        else SoundRadius = 0;
        SendSound();

    }

    void SendSound()
    {
        Collider[] EnemiesToNotify = Physics.OverlapSphere(transform.position, SoundRadius, 1 << LayerMask.NameToLayer("Enemies"));
        Debug.Log(EnemiesToNotify.Length);
        foreach(Collider enemy in EnemiesToNotify)
        {
            SoundEnemy SEScript = enemy.GetComponent<SoundEnemy>();
            SEScript.PlayerFoundAction.Invoke();
        }
    }
}
