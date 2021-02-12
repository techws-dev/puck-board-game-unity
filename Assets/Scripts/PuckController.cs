using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckController : MonoBehaviour
{
    public GameObject shootingLinePrefab;
    private GameObject shootingLine;
    public GameManager gameManager;
    public int player;
    private bool launched;
    private DateTime launchTime;
    private bool ended;
    private float distToGround;
    private bool isReady;
    private bool readyToLaunch;
    private float maxForce = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        launched = false;
        ended = false;
        isReady = false;
        readyToLaunch = false;
        distToGround = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isReady) {
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
            } else if(isGrounded() && Input.GetButtonDown("Jump")) {
                readyToLaunch = true;
                shootingLine.SendMessage("Grow");
            } else if(readyToLaunch && Input.GetButtonUp("Jump")) {
                launchTime = DateTime.Now;

                float angleY = shootingLine.transform.eulerAngles.y;
                float force = maxForce * shootingLine.transform.localScale.z;
                Destroy(shootingLine);

                Vector3 forceVector = Quaternion.Euler(0, angleY, 0) * (transform.forward * force);
                gameObject.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
                launched = true;
            }
        } else if(isGrounded()) {
            // Create the shooting line
            shootingLine = Instantiate(shootingLinePrefab, new Vector3(0, 0.02f, -3), Quaternion.identity);

            isReady = true;
        }
        
    }

    // Check if is grounded
    private bool isGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.05f);
    }
}
