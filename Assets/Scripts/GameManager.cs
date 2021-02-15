using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject board;
    public GameObject puckPrefab;
    public GameObject mainCamera;
    public Material player1Material;
    public Material player2Material;
    private int currentPlayer = 1;
    private int numberPlayers = 2;
    private int pucksLeft = 10;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start the game");
        
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable() {
        Debug.Log("Quit the game");
    }

    // Init game
    void InitGame() {
        numberPlayers = 2;
        currentPlayer = 0;
        pucksLeft = numberPlayers * 5;

        NextPlayer();
    }

    // Create a puck
    void createPuck() {
        // Create a puck
        Vector3 puckSpawnPosition = puckPrefab.GetComponent<PuckController>().getPuckSpawnPosition();
        GameObject puck = Instantiate(puckPrefab, puckSpawnPosition, Quaternion.identity);

        // Associate objects
        PuckController puckController = puck.GetComponent<PuckController>();
        puckController.gameManager = this;
        puckController.player = currentPlayer;

        // Add material to puck
        puck.GetComponent<MeshRenderer>().material = currentPlayer switch {
            1 => player1Material,
            2 => player2Material,
            _ => null,
        };

        // Set camera to follow created puck
        mainCamera.GetComponent<CameraController>().setPuckToFollow(puck);
    }

    // Change player
    void NextPlayer() {
        currentPlayer = (currentPlayer % numberPlayers) + 1;
        Debug.Log("Player:" + currentPlayer);

        createPuck();
    }
}
