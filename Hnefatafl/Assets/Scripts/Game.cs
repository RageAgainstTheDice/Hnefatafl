using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    public GameObject taflPiece;
    private GameObject[,] positions = new GameObject[11, 11];
    private GameObject[] playerBlack = new GameObject[24];
    private GameObject[] playerWhite = new GameObject[13];
    private GameObject pieceParent;
    private AudioSource winSound, bgm;
    private AudioClip winSoundClip;
    private string currentPlayer = "black";
    private bool gameOver = false;
    public bool moving = false; // true when a piece is moving
    public int blackScore = 0, whiteScore = 0; // number of pieces captured by ecach side

    // Start is called before the first frame update
    void Start()
    {
        pieceParent = GameObject.Find("GamePieces");
        winSound = GetComponent<AudioSource>();
        winSoundClip = winSound.clip;
        bgm = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        //Instantiate(taflPiece, new Vector3(0, 0, -1), Quaternion.identity);
        playerWhite = new GameObject[]
        {
            Create("KingPiece", 5, 5), Create("WhitePiece", 3, 5), Create("WhitePiece", 7, 5),
            Create("WhitePiece", 5, 3), Create("WhitePiece", 5, 4), Create("WhitePiece", 5, 7),
            Create("WhitePieceVar", 4, 4), Create("WhitePieceVar", 6, 4), Create("WhitePieceVar", 4, 5),
            Create("WhitePieceVar", 6, 5), Create("WhitePieceVar", 4, 6), Create("WhitePieceVar", 5, 6),
            Create("WhitePieceVar", 6, 6)
        };
        playerBlack = new GameObject[]
        {
            Create("BlackPiece", 3, 0), Create("BlackPieceVar", 4, 0), Create("BlackPieceVar", 5, 0),
            Create("BlackPieceVar", 6, 0), Create("BlackPiece", 7, 0), Create("BlackPiece", 5, 1),
            Create("BlackPieceVar", 0, 3), Create("BlackPieceVar", 0, 4), Create("BlackPieceVar", 0, 5),
            Create("BlackPieceVar", 0, 6), Create("BlackPiece", 0, 7), Create("BlackPiece", 1, 5),
            Create("BlackPiece", 9, 5), Create("BlackPiece", 10, 3), Create("BlackPieceVar", 10, 4),
            Create("BlackPieceVar", 10, 5), Create("BlackPieceVar", 10, 6), Create("BlackPiece", 10, 7),
            Create("BlackPiece", 5, 9), Create("BlackPiece", 3, 10), Create("BlackPieceVar", 4, 10),
            Create("BlackPieceVar", 5, 10), Create("BlackPieceVar", 6, 10), Create("BlackPiece", 7, 10)
        };

        // set all the piece positions on the position board
        for (int i = 0; i < playerBlack.Length; i++) {SetPosition(playerBlack[i]);}
        for (int i = 0; i < playerWhite.Length; i++) {SetPosition(playerWhite[i]);}

        GameObject.FindGameObjectWithTag("BlackTurnText").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("BlackScore").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("WhiteScore").GetComponent<TextMeshProUGUI>().enabled = true;
    }

    // fabricates an instance of a prefab/object
    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(taflPiece, new Vector3(0, 0, -1), Quaternion.identity, pieceParent.transform);
        Taflman tm = obj.GetComponent<Taflman>();
        tm.name = name;
        tm.SetXBoard(x);
        tm.SetYBoard(y);
        tm.Activate();
        return obj;
    }

    // sets it to the desired position
    public void SetPosition(GameObject obj)
    {
        Taflman tm = obj.GetComponent<Taflman>();
        positions[tm.GetXBoard(), tm.GetYBoard()] = obj;
    }

    // makes its position "empty"
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    // return a game object at a specific position
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    // check if the position is on the board
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) { return false; }
        else {return true;}
    }

    public bool IsKingSquare(int x, int y)
    {
        if ((x == 0 && y == 0) || (x == 0 && y == 10) || (x == 10 && y == 0) || (x == 10 && y == 10) || (x == 5 && y == 5))
        {
            return true;
        }
        else { return false; }
    }

    public bool IsCornerSquare(int x, int y)
    {
        if ((x == 0 && y == 0) || (x == 0 && y == 10) || (x == 10 && y == 0) || (x == 10 && y == 10))
        {
            return true;
        }
        else { return false; }
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameover()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        GameObject.FindGameObjectWithTag("BlackScore").GetComponent<TextMeshProUGUI>().text = "Number of white pieces captured: " + blackScore + "/13";
        GameObject.FindGameObjectWithTag("WhiteScore").GetComponent<TextMeshProUGUI>().text = "Number of black pieces captured: " + whiteScore + "/24";
        if (currentPlayer == "white")
        {
            GameObject.FindGameObjectWithTag("WhiteTurnText").GetComponent<TextMeshProUGUI>().enabled = false;
            GameObject.FindGameObjectWithTag("BlackTurnText").GetComponent<TextMeshProUGUI>().enabled = true;
            currentPlayer = "black";
        }
        else
        {
            GameObject.FindGameObjectWithTag("BlackTurnText").GetComponent<TextMeshProUGUI>().enabled = false;
            GameObject.FindGameObjectWithTag("WhiteTurnText").GetComponent<TextMeshProUGUI>().enabled = true;
            currentPlayer = "white";
        }
        Debug.Log(currentPlayer + "'s turn");
    }

    // gets called once per frame
    public void Update()
    {
        // allows the player to restart the game once it has finished
        if(gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("TitleScreen");
        }
    }

    public void Winner(string PlayerWinner)
    {
        bgm.Stop();
        AudioSource.PlayClipAtPoint(winSoundClip, new Vector3(0, 0, -1.0f));
        gameOver = true;

        GameObject.FindGameObjectWithTag("BlackTurnText").GetComponent<TextMeshProUGUI>().enabled = false;
        GameObject.FindGameObjectWithTag("WhiteTurnText").GetComponent<TextMeshProUGUI>().enabled = false;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<TextMeshProUGUI>().text = PlayerWinner + " wins!";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<TextMeshProUGUI>().enabled = true;
    }
}