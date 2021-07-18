using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class NetworkVar : NetworkBehaviour
{

    public NetworkVariable<float> CurrentTime = new NetworkVariable<float>();
    public NetworkVariable<float> Counter = new NetworkVariable<float>();
    // public float Counter;
    // public float MaximumTime;
    // Start is called before the first frame update
    void Start()
    {
            // MaximumTime = FindObjectOfType<Values>().MaximumTime;
            // CurrentTime.Value = MaximumTime;
            // Counter = CurrentTime.Value;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsHost)
        {
        Debug.Log("Yeah" + CurrentTime.Value);
        Counter.Value = Counter.Value + 1f * Time.deltaTime;
        CurrentTime.Value = Mathf.RoundToInt(Counter.Value);
        }
    }
}
