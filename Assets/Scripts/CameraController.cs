using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject puck;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(0, 2, -6);
    }

    // Update is called once per frame
    void Update()
    {
        if(puck) {
            // follow the puck on z axis only
            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;
            float z = puck.transform.position.z - 2;

            // don't follow the puck if it goes out of the board
            if(z < 2) {
                gameObject.transform.position = new Vector3(x, y, z);    
            }
        } else {
            gameObject.transform.position = startPosition;
        }
    }

    // Set the puck to follow
    public void setPuckToFollow(GameObject puckToFollow) {
        puck = puckToFollow;
    }
}
