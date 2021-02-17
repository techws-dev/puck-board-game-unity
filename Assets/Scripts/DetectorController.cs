using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour
{
    public GameManager gameManager;

    public int value;
    private bool sentinel;

    // Start is called before the first frame update
    void Start()
    {
        sentinel = false;
    }

    // Update is called once per frame
    void Update()
    {
        sentinel = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(!sentinel) {
            sentinel = true;
            if(collider.name == "Puck(Clone)") {
                int playerIndex = collider.GetComponent<PuckController>().player - 1;
                
                (int playerIndex, int value) args = (playerIndex, value);
                gameManager.SendMessage("AddPoints", args);

                Destroy(collider);
            }
        }
    }
}
