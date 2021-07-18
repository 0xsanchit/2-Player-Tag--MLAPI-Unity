using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneSpace : MonoBehaviour
{
    public Rigidbody2D PlayerRigidBody;
    public float MovementSpeed = 5;
    public float jumpVelocity;

    void Start()
    {

        
    }

    void Update()
    {

        if (Input.GetKey("d") && PlayerRigidBody.velocity.x < MovementSpeed)
        {
            PlayerRigidBody.velocity = new Vector2(MovementSpeed, PlayerRigidBody.velocity.y);
        }


        if (Input.GetKey("a") && -PlayerRigidBody.velocity.x < MovementSpeed)
        {
            PlayerRigidBody.velocity = new Vector2(-MovementSpeed, PlayerRigidBody.velocity.y);
        }

        if (Input.GetKey("s"))
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, -jumpVelocity);
        }

        if (Input.GetKey("w"))
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, jumpVelocity);       
        }
    }
}

