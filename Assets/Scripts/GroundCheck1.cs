using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck1 : MonoBehaviour
{
    public bool GrounCheck1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag != "Player")
        {
            GrounCheck1 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GrounCheck1 = false;
    }
}
