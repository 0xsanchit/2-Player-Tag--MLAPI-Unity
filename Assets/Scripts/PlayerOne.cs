using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.SceneManagement;

public class PlayerOne : NetworkBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;

    public float NormalSpeed = 15;
    public float TagSpeed = 20;
    public GameObject BlueLight;
    public GameObject BlueMarker;
    public GameObject PlayerOneCheck;

    public Rigidbody2D PlayerRigidBody;
    public float MovementSpeed = 15;
    public float JumpForce = 10;    
    public float MovementForce = 100;
    public bool isJumping;
    public float jumpVelocity;
    public BoxCollider2D boxCollider2D;
    public float hangTime = .2f;
    private float hangCounter;
    public float jumpBufferLength = .5f;
    private float jumpBufferCounter;

    public NetworkVariable<bool> On = new NetworkVariable<bool>(true);
    public NetworkVariable<float> OnTime1 = new NetworkVariable<float>();



    void Start()
    {
        On.Settings.WritePermission = NetworkVariablePermission.Everyone;
        On.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        OnTime1.Settings.WritePermission = NetworkVariablePermission.Everyone;
        OnTime1.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        isJumping = false;
        BlueMarker.SetActive(false);
        BlueLight.SetActive(false);
    }

    void Update()
    {
        OnTime1.Value = OnTime1.Value - Time.deltaTime;
        if (!FindObjectOfType<NetworkGameManager>().tagged.Value)
        {
            SetMarker(true);
            SetLight(true);
            MovementSpeed = TagSpeed;
        }
        else
        {
            SetMarker(false);
            SetLight(false);
            MovementSpeed = NormalSpeed;
        }

        if (isGrounded())
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
    }
    public void MoveLeft()
    {
        if (-PlayerRigidBody.velocity.x < MovementSpeed)
        {
            PlayerRigidBody.velocity = new Vector2(-MovementSpeed, PlayerRigidBody.velocity.y);

        }
    }
    public void MoveRight()
    {
        if (PlayerRigidBody.velocity.x < MovementSpeed)
        {
            PlayerRigidBody.velocity = new Vector2(MovementSpeed, PlayerRigidBody.velocity.y);
        }
    }
    public void Jump()
    {
        if (hangCounter > 0f)
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, jumpVelocity);
            hangCounter = 0f;
        }
    }
    public void StopMoving()
    {
        PlayerRigidBody.velocity = new Vector2(0f, PlayerRigidBody.velocity.y);
    }

    private bool isGrounded()
    {
        float extraHeight = .1f;
        Color raycolor;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraHeight,groundLayerMask);
        if (raycastHit2D.collider != null)
        {
            raycolor = Color.green;
        }
        else
        {
            raycolor = Color.red;
        }
        Debug.DrawRay(new Vector3(boxCollider2D.bounds.center.x - boxCollider2D.bounds.extents.x, boxCollider2D.bounds.center.y, boxCollider2D.bounds.center.z), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeight), raycolor);
        RaycastHit2D raycastHit2D1 = Physics2D.Raycast(new Vector3(boxCollider2D.bounds.center.x - boxCollider2D.bounds.extents.x,boxCollider2D.bounds.center.y,boxCollider2D.bounds.center.z), Vector2.down, boxCollider2D.bounds.extents.y + extraHeight, groundLayerMask);
        if (raycastHit2D1.collider != null)
        {
            raycolor = Color.green;
        }
        else
        {
            raycolor = Color.red;
        }
        Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (boxCollider2D.bounds.extents.y + extraHeight), raycolor);
        RaycastHit2D raycastHit2D2 = Physics2D.Raycast(new Vector3(boxCollider2D.bounds.center.x + boxCollider2D.bounds.extents.x, boxCollider2D.bounds.center.y, boxCollider2D.bounds.center.z), Vector2.down, boxCollider2D.bounds.extents.y + extraHeight, groundLayerMask);
        if (raycastHit2D2.collider != null)
        {
            raycolor = Color.green;
        }
        else
        {
            raycolor = Color.red;
        }
        Debug.DrawRay(new Vector3(boxCollider2D.bounds.center.x + boxCollider2D.bounds.extents.x, boxCollider2D.bounds.center.y, boxCollider2D.bounds.center.z), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeight), raycolor);
        return (raycastHit2D.collider != null || raycastHit2D1.collider != null || raycastHit2D2.collider != null);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("Blue Colliding" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && OnTime1.Value <=0)
        {
            Debug.Log("Toggling Blue Trigger" + collision.gameObject.tag);
            On.Value = !On.Value;
            OnTime1.Value = 0.2f;
            FindObjectOfType<NetworkGameManager>().tagged.Value = !FindObjectOfType<NetworkGameManager>().tagged.Value;

            /*            BlueMarker.SetActive(!BlueMarker.activeSelf);
                        BlueLight.SetActive(!BlueLight.activeSelf);*/


            // FindObjectOfType<NetworkGameManager>().Turn.Value = !FindObjectOfType<NetworkGameManager>().Turn.Value;
            // Debug.Log("Turn" + FindObjectOfType<NetworkGameManager>().Turn.Value);
        }
    }

    public void SetMarker(bool active)
    {
        BlueMarker.SetActive(active);
    }

    public void SetLight(bool active)
    {
        BlueLight.SetActive(active);
    }



}
