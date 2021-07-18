using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Jump()
    {
        FindObjectOfType<PlayerOne>().Jump();
        FindObjectOfType<PlayerTwo>().Jump();
    }
    public void MoveLeft()
    {
        FindObjectOfType<PlayerOne>().MoveLeft();
        FindObjectOfType<PlayerTwo>().MoveLeft();

    }
    public void MoveRight()
    {
        FindObjectOfType<PlayerOne>().MoveRight();
        FindObjectOfType<PlayerTwo>().MoveRight();
    }
    public void StopMoving()
    {
        FindObjectOfType<PlayerOne>().StopMoving();
        FindObjectOfType<PlayerTwo>().StopMoving();
    }
}
