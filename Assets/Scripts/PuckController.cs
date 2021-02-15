using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuckController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameManager gameManager;

    public int player;
    private bool launched;
    private DateTime launchTime;
    private bool ended;
    private float distToGround;

    private bool isDragable;

    private Vector3 puckSpawnPosition = new Vector3(0, 0.28f, -3);

    public Vector3 getPuckSpawnPosition() {
        return puckSpawnPosition;
    }

    public bool isLaunched() {
        return launched;
    }

    // Start is called before the first frame update
    void Start()
    {
        launched = false;
        ended = false;
        distToGround = 0.22f;
        isDragable = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(!launched && isGrounded()) {
            isDragable = true;
        }

        if(launched) {
            // Check for and object being out of the board
            float objectPositionY = transform.position.y;

            if(!ended) {
                // Check for end of movement
                if((DateTime.Compare(DateTime.Now, launchTime.AddSeconds(1)) > 0
                    && gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude == 0)
                    || objectPositionY <= -1) {
                    ended = true;
                    gameManager.SendMessage("NextPlayer");
                }
            }

            if(objectPositionY <= -1) {
                Destroy(gameObject);
            }
        }
        
    }

    // Check if is grounded
    private bool isGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.005f);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(!isDragable) return;

        updatePuckPosition(eventData.position);
    }

    public void OnDrag(PointerEventData eventData) {
        if(!isDragable) return;

        updatePuckPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if(!isDragable) return;

        float distanceX = puckSpawnPosition.x - transform.position.x;
        float distanceZ = puckSpawnPosition.z - transform.position.z;
        float distance = Mathf.Sqrt(distanceX*distanceX + distanceZ*distanceZ);

        if ( distance <= 0.1f ) {
            transform.position = puckSpawnPosition;
        } else {
            Vector3 force = new Vector3(distanceX, 0, distanceZ) * 1200.0f;
            Debug.Log(force);
            gameObject.GetComponent<Rigidbody>().AddForce(force);
            launched = true;
            isDragable = false;
        }
    }

    // Set puck when dragged
    private void updatePuckPosition(Vector3 inputPosition) {
        int layerMask = LayerMask.GetMask("Board");

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        
        if (Physics.Raycast(ray, out hit, 100.0f, layerMask)) {

            Vector3 puckDrag = new Vector3(
                hit.point.x,
                puckSpawnPosition.y,
                hit.point.z
            );

            // check puck is in a circle
            float distanceX = puckSpawnPosition.x - puckDrag.x;
            float distanceZ = puckSpawnPosition.z - puckDrag.z;
            float distance = Mathf.Sqrt(distanceX*distanceX + distanceZ*distanceZ);

            if ( distance > 0.8f ) {
                return;
            } else if ( distance > 0.6f  ) {
                float ratio = 0.6f / distance;
                puckDrag.x = puckSpawnPosition.x - (distanceX * ratio);
                puckDrag.z = puckSpawnPosition.z - (distanceZ * ratio);
            }

            transform.position = puckDrag;
        }
    }
}
