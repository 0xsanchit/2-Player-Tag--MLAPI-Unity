using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public Camera cam;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;

    private void Update()
    {
        List < GameObject > players = new List<GameObject>();
        players.Add(PlayerOne);
        players.Add(PlayerTwo);

        var targets = players;
        var minX = targets.Min(t => t.transform.position.x);
        var maxX = targets.Max(t => t.transform.position.x);
        var minY = targets.Min(t => t.transform.position.y);
        var maxY = targets.Max(t => t.transform.position.y);
        var desiredWidth = maxX - minX;
        var desiredHeight = maxY - minY;
        var currentWidth = Screen.width;
        var currentHeight = Screen.height;
        var targetSize
            = desiredWidth > desiredHeight
            ? ((desiredWidth / currentWidth) * currentHeight) / 2.0f
            : ((desiredHeight / currentHeight) * currentWidth) / 2.0f
            ;
        targetSize += 1.0f;
        if(targetSize<8)
        {
            this.cam.orthographicSize = 8;
        }
        else
        {
            this.cam.orthographicSize = Mathf.Lerp(this.cam.orthographicSize, targetSize, Time.deltaTime * 100);
        }

        var position = this.cam.transform.position;
        position.x = maxX * 0.5f + minX * 0.5f;
        position.y = maxY * 0.5f + minY * 0.5f;
        this.cam.transform.position = position;
    }
}