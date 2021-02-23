using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject board;
    public GameObject puckPrefab;
    public GameObject mainCamera;

    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text resultText;

    public Material player1Material;
    public Material player2Material;
    private int currentPlayer = 1;
    private int numberPlayers = 2;
    private int pucksLeft = 10;
    private int[] scores;

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
        scores = new int[numberPlayers];

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
        if( pucksLeft == 0 ) {
            EndOfGame();
        }

        // Decrement pucks count
        pucksLeft--;

        int index=1;
        foreach (int score in scores) {
            Debug.Log("Score Player " + index + ": " + score);
            index++;
        }

        player1ScoreText.text = scores[0].ToString();
        player2ScoreText.text = scores[1].ToString();

        currentPlayer = (currentPlayer % numberPlayers) + 1;
        Debug.Log("Player:" + currentPlayer);

        createPuck();
    }

    // Add points to player
    void AddPoints((int playerIndex, int value) args) {
        scores[args.playerIndex] += args.value;
    }

    // End of game : show the winner or tied
    void EndOfGame() {
        if (scores[0] > scores[1]) {
            resultText.text = "Player 1 wins !!!";
        } else if (scores[1] > scores[0]) {
            resultText.text = "Player 2 wins !!!";
        } else {
            resultText.text = "Players are tied.";
        }
    }
}
