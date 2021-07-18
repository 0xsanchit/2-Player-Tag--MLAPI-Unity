using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck2 : MonoBehaviour
{
    public bool GrounCheck2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        GrounCheck2 = true;
        if (collision.collider.tag == "Wall")
        {
            GrounCheck2 = false;
        }      
       
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            GrounCheck2 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GrounCheck2 = false;
    }
}
