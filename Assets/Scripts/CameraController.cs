using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject puck;
    private PuckController puckController;
    private Vector3 startPosition;
    private float cameraOffsetZ;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(0, 2, -4.5f);
        cameraOffsetZ = -1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(puckController.isLaunched()) {
            // follow the puck on z axis only
            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;
            float z = puck.transform.position.z + cameraOffsetZ;

            // don't follow the puck if it goes out of the board
            if(z > startPosition.z && z < 2) {
                gameObject.transform.position = new Vector3(x, y, z);    
            }
        }
    }

    // Set the puck to follow
    public void setPuckToFollow(GameObject puckToFollow) {
        puck = puckToFollow;
        puckController = puck.GetComponent<PuckController>();
        gameObject.transform.position = startPosition;
    }
}
