using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScriptSpace : MonoBehaviour
{
    public GameObject BlueLight;
    public GameObject RedLight;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    public GameObject RedMarker;
    public GameObject BlueMarker;
    public GameObject PlayerOneCheck;
    public GameObject PlayerTwoCheck;

    BoxCollider2D GroundCheckOne;
    BoxCollider2D GroundCheckTwo;
    public bool Turn;
    // Start is called before the first frame update
    void Start()
    {
        RedMarker.SetActive(false);
        GroundCheckOne = PlayerOneCheck.GetComponent<BoxCollider2D>();
        GroundCheckTwo = PlayerTwoCheck.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreCollision(PlayerOne.GetComponent<Collider2D>(), PlayerTwo.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(GroundCheckOne, PlayerTwo.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(GroundCheckTwo, PlayerOne.GetComponent<Collider2D>());


        if (Turn)
        {
            BlueMarker.SetActive(true);
            BlueLight.SetActive(true);
            RedMarker.SetActive(false);
            RedLight.SetActive(false);
            PlayerTwo.GetComponent<PlayerTwoSpace>().MovementSpeed = 10f;
            PlayerOne.GetComponent<PlayerOneSpace>().MovementSpeed = 8f;
        }
        if (!Turn)
        {
            BlueLight.SetActive(false);
            BlueMarker.SetActive(false);
            RedMarker.SetActive(true);
            RedLight.SetActive(true);
            PlayerTwo.GetComponent<PlayerTwoSpace>().MovementSpeed = 10f;
            PlayerOne.GetComponent<PlayerOneSpace>().MovementSpeed = 8f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Turn = !Turn;
        }
    }
}
