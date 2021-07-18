using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoSpace : MonoBehaviour
{
    public Rigidbody2D PlayerRigidBody;
    public float MovementSpeed = 5;
    public float jumpVelocity;
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && PlayerRigidBody.velocity.x < MovementSpeed)
        {
            PlayerRigidBody.velocity = new Vector2(MovementSpeed, PlayerRigidBody.velocity.y);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && -PlayerRigidBody.velocity.x < MovementSpeed)
        {
            PlayerRigidBody.velocity = new Vector2(-MovementSpeed, PlayerRigidBody.velocity.y);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, jumpVelocity);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, -jumpVelocity);
        }
    }

}