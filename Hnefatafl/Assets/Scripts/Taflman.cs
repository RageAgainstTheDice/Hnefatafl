using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taflman : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject movePlate;
    public GameObject attackPlate;
    private GameObject mpParent;

    // Positions
    private int xBoard = -1, yBoard = -1 /*, capturableX = -1, capturableY = -1*/;

    // Variable to keep track of "black" player or "white" player
    private string player;

    // References for all the sprites that the taflPiece can be
    public Sprite KingPiece, WhitePiece, WhitePieceVar;
    public Sprite BlackPiece, BlackPieceVar;

    public void Activate()
    {
        mpParent = GameObject.Find("MovePlates");
        controller = GameObject.FindGameObjectWithTag("GameController");

        // take the instantiated location and adjust the transform
        SetCoords();

        switch(this.name)
        {
            case "KingPiece":
                this.GetComponent<SpriteRenderer>().sprite = KingPiece;
                player = "white";
                break;
            case "WhitePiece":
                this.GetComponent<SpriteRenderer>().sprite = WhitePiece;
                player = "white";
                break;
            case "WhitePieceVar":
                this.GetComponent<SpriteRenderer>().sprite = WhitePieceVar;
                player = "white";
                break;

            case "BlackPiece":
                this.GetComponent<SpriteRenderer>().sprite = BlackPiece;
                player = "black";
                break;
            case "BlackPieceVar":
                this.GetComponent<SpriteRenderer>().sprite = BlackPieceVar;
                player = "black";
                break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard, y = yBoard;

        x *= 0.74f;
        y *= 0.74f;

        x += -3.7f;
        y += -3.7f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public void SetCoordsMove(float m, float n)
    {
        float startTime = Time.time;
        float x = xBoard, y = yBoard;

        x *= 0.74f;
        y *= 0.74f;
        m *= 0.74f;
        n *= 0.74f;

        x += -3.7f;
        y += -3.7f;
        m += -3.7f;
        n += -3.7f;

        Vector3 start = new Vector3(x, y, -1.0f);
        Vector3 destination = new Vector3(m, n, -1.0f);

        //this.transform.position = new Vector3(x, y, -1.0f);
        while ((x != m) || (y != n))
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(m - x, n - y);
            //this.transform.position = Vector3.Lerp(new Vector3(x, y, -1.0f), new Vector3(m, n, -1.0f), Time.deltaTime * 1.0f);
        }
        this.transform.position = Vector3.Lerp(start, destination, (Time.time - startTime)/1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    /*
    public int GetCapturableX()
    {
        return capturableX;
    }

    public int GetCapturableY()
    {
        return capturableY;
    }
    */

    public void SetXBoard(int x)
    {
        xBoard = x;
        // xBoard = x + (xBoard - x) * (int)(Time.deltaTime * 0.0001f);
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
        // yBoard = y + (yBoard - y) * (int)(Time.deltaTime * 0.0001f);
    }

    public bool OnSquare()
    {
        int x = xBoard, y = yBoard;
        Game sc = controller.GetComponent<Game>();
        return sc.GetPosition(x, y) == gameObject;
    }

    /*
    // Changes the piece's position to the desired place
    public void MovePiece(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        int xdif = x - this.xBoard, ydif = y - this.yBoard;

        if (xdif > 0 && ydif == 0)
        {
            while(this.xBoard != x)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(xdif, ydif);
                // StartCoroutine(TimeDelay());
                this.xBoard = x;
            }
        }
        else if(xdif < 0 && ydif == 0)
        {
            while(this.xBoard != x) { this.xBoard--; SetCoords();/*sc.SetPosition(gameObject); }
        }
        else if(xdif == 0 && ydif > 0)
        {
            while(this.yBoard != y) { this.yBoard++; SetCoords();/*sc.SetPosition(gameObject); }
        }
        else if(xdif == 0 && ydif < 0)
        {
            while(this.yBoard != y) { this.yBoard--; SetCoords();/*sc.SetPosition(gameObject); }
        }
        else
        {
            Debug.Log("this shouldn't be happening");
        }

        //this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        // StopAllCoroutines();
        SetCoords();

        /*
        transform.position = Vector3.Lerp(this.transform.position, new Vector3(x, y, -1.0f), Time.deltaTime * 1.0f);
        SetCoords();

        Vector2 coordinates = new Vector2(xBoard, yBoard);
        coordinates = Vector2.Lerp(coordinates, new Vector2(x, y), Time.deltaTime * 1.0f);
    }

    /*
    // moves the piece exactly one square
    public void MovePieceOnce(int x, int y)
    {
        int xdif = x - this.xBoard, ydif = y - this.yBoard;

        if (xdif > 0 && ydif == 0) { this.xBoard++; }
        else if (xdif < 0 && ydif == 0) { this.xBoard--; }
        else if (xdif == 0 && ydif > 0) { this.yBoard++; }
        else if (xdif == 0 && ydif < 0) { this.yBoard--; }
        else { Debug.Log("this shouldn't be happening"); }
    }

    IEnumerator TimeDelay()
    {
        //float t = this.GetComponent<Rigidbody2D>().velocity
        yield return new WaitForSeconds(5f);
    }
    */

    // see if there is a capturable above or below the sqaure with coordinates x,y (with offset n)
    public bool CapturableVert(int x, int y, int n)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y + n) && sc.GetPosition(x, y + n) != null && sc.GetPosition(x, y + n) != gameObject)
        {
            if (sc.GetPosition(x, y + n).GetComponent<Taflman>().name != "KingPiece") // capture conditions for non-king pieces
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x, y + n) &&
                    sc.PositionOnBoard(x, y + (2 * n)) && sc.GetPosition(x, y + n) != null &&
                    ((sc.GetPosition(x, y + (2 * n)) != null && !sc.IsCornerSquare(x, y + (2 * n))) ||
                    (sc.GetPosition(x, y + (2 * n)) == null && sc.IsCornerSquare(x, y + (2 * n)))))
                {
                    if ((sc.GetPosition(x, y + (2 * n)) != null && !sc.IsCornerSquare(x, y + (2 * n))))
                    {
                        return (sc.GetPosition(x, y + n).GetComponent<Taflman>().player != player &&
                            sc.GetPosition(x, y + (2 * n)).GetComponent<Taflman>().player == player);
                    }
                    else if (sc.GetPosition(x, y + (2 * n)) == null && sc.IsCornerSquare(x, y + (2 * n)))
                    {
                        return (sc.GetPosition(x, y + n).GetComponent<Taflman>().player != player &&
                            sc.IsCornerSquare(x, y + (2 * n)));
                    }
                    else
                    {
                        Debug.Log("this shouldn't be happening");
                        return false;
                    }
                }
                else { return false; }
            }
            else // capture conditions for the king piece
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x, y + n) && sc.PositionOnBoard(x, y + (2 * n)) &&
                        sc.PositionOnBoard(x - 1, y + n) && sc.PositionOnBoard(x + 1, y + n) &&
                        sc.GetPosition(x, y + n) != null && sc.GetPosition(x, y + (2 * n)) != null &&
                        sc.GetPosition(x - 1, y + n) != null && sc.GetPosition(x + 1, y + n) != null)
                {
                    if (sc.GetPosition(x, y + n).GetComponent<Taflman>().player != player &&
                        sc.GetPosition(x, y + (2 * n)).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x - 1, y + n).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x + 1, y + n).GetComponent<Taflman>().player == player) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
        } else { return false; }
    }

    /*
    // see if there is a capturable below the sqaure with coordinates x,y
    public bool CapturableBelow(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y - 1) && sc.GetPosition(x, y - 1) != null && sc.GetPosition(x, y - 1) != gameObject)
        {
            if (sc.GetPosition(x, y - 1).GetComponent<Taflman>().name != "KingPiece") // capture conditions for non-king pieces
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x, y - 1) && sc.PositionOnBoard(x, y - 2) &&
                        sc.GetPosition(x, y - 1) != null &&
                        (sc.GetPosition(x, y - 2) != null || sc.IsCornerSquare(x, y - 2)))
                {
                    if (sc.GetPosition(x, y - 1).GetComponent<Taflman>().player != player &&
                        (sc.GetPosition(x, y - 2).GetComponent<Taflman>().player == player) ||
                        sc.IsCornerSquare(x, y - 2)) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else // capture conditions for the king piece
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x, y - 1) && sc.PositionOnBoard(x, y - 2) &&
                    sc.PositionOnBoard(x - 1, y - 1) && sc.PositionOnBoard(x + 1, y - 1) &&
                    sc.GetPosition(x, y - 1) != null && sc.GetPosition(x, y - 2) != null &&
                    sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x + 1, y - 1) != null)
                {
                    if (sc.GetPosition(x, y - 1).GetComponent<Taflman>().player != player &&
                        sc.GetPosition(x, y - 2).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x - 1, y - 1).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x + 1, y - 1).GetComponent<Taflman>().player == player) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
        } else { return false; }
    }

    // see if there is a capturable to the left of the sqaure with coordinates x,y
    public bool CapturableLeft(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y) != gameObject)
        {
            if (sc.GetPosition(x - 1, y).GetComponent<Taflman>().name != "KingPiece") // capture conditions for non-king pieces
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x - 1, y) && sc.PositionOnBoard(x - 2, y) &&
                        sc.GetPosition(x - 1, y) != null &&
                        (sc.GetPosition(x - 2, y) != null || sc.IsCornerSquare(x - 2, y)))
                {
                    if (sc.GetPosition(x - 1, y).GetComponent<Taflman>().player != player &&
                        (sc.GetPosition(x - 2, y).GetComponent<Taflman>().player == player) ||
                        sc.IsCornerSquare(x - 2, y)) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else // capture conditions for the king piece
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x - 1, y) && sc.PositionOnBoard(x - 2, y) &&
                    sc.PositionOnBoard(x - 1, y - 1) && sc.PositionOnBoard(x - 1, y + 1) &&
                    sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 2, y) != null &&
                    sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 1, y + 1) != null)
                {
                    if (sc.GetPosition(x - 1, y).GetComponent<Taflman>().player != player &&
                        sc.GetPosition(x - 2, y).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x - 1, y - 1).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x - 1, y + 1).GetComponent<Taflman>().player == player) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
        } else { return false; }
    }
    */

    // see if there is a capturable to the left or right of the sqaure with coordinates x,y (with offset n)
    public bool CapturableHorz(int x, int y, int n)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x + n, y) && sc.GetPosition(x + n, y) != null && sc.GetPosition(x + n, y) != gameObject)
        {
            if (sc.GetPosition(x + n, y).GetComponent<Taflman>().name != "KingPiece") // capture conditions for non-king pieces
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x + n, y) &&
                    sc.PositionOnBoard(x + (2 * n), y) && sc.GetPosition(x + n, y) != null &&
                    ((sc.GetPosition(x + (2 * n), y) != null && !sc.IsCornerSquare(x + (2 * n), y)) ||
                    (sc.GetPosition(x + (2 * n), y) == null && sc.IsCornerSquare(x + (2 * n), y))))
                {
                    if (sc.GetPosition(x + (2 * n), y) != null && !sc.IsCornerSquare(x + (2 * n), y))
                    {
                        return (sc.GetPosition(x + n, y).GetComponent<Taflman>().player != player &&
                            sc.GetPosition(x + (2 * n), y).GetComponent<Taflman>().player == player);
                    }
                    else if (sc.GetPosition(x + (2 * n), y) == null && sc.IsCornerSquare(x + (2 * n), y))
                    {
                        return (sc.GetPosition(x + n, y).GetComponent<Taflman>().player != player &&
                            sc.IsCornerSquare(x + (2 * n), y));
                    }
                    else
                    {
                        Debug.Log("this shouldn't be happening...");
                        return false;
                    }
                }
                else { return false; }
            }
            else // capture conditions for the king piece
            {
                if (sc.PositionOnBoard(x, y) && sc.PositionOnBoard(x + n, y) && sc.PositionOnBoard(x + (2 * n), y) &&
                    sc.PositionOnBoard(x + n, y - 1) && sc.PositionOnBoard(x + n, y + 1) &&
                    sc.GetPosition(x + n, y) != null && sc.GetPosition(x + (2 * n), y) != null &&
                    sc.GetPosition(x + n, y - 1) != null && sc.GetPosition(x + n, y + 1) != null)
                {
                    if (sc.GetPosition(x + n, y).GetComponent<Taflman>().player != player &&
                        sc.GetPosition(x + (2 * n), y).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x + n, y - 1).GetComponent<Taflman>().player == player &&
                        sc.GetPosition(x + n, y + 1).GetComponent<Taflman>().player == player) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
        } else { return false; }
    }

    // makes it so that the moveplates are generated upon clicking a piece
    private void OnMouseUp()
    {
        if(!controller.GetComponent<Game>().IsGameover() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            //InitiateMovePlates(xBoard, yBoard);
            LineMovePlate(1, 0);
            LineMovePlate(0, 1);
            LineMovePlate(-1, 0);
            LineMovePlate(0, -1);
        }
    }

    // de-generates the moveplates
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    // generates the moveplates in a linear fashion
    public void LineMovePlate(int xUp, int yUp)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xUp, y = yBoard + yUp;

        while(sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null /*&& !sc.IsCornerSquare(x, y)*/)
        {
            // if the square in question is a king square, skip that turn of the loop
            if (sc.IsKingSquare(x, y) && this.name != "KingPiece")
            {
                x += xUp;
                y += yUp;
                continue;
            }
            InitiateMovePlates(x, y);
            x += xUp;
            y += yUp;

            /*
            // indicate if a piece above the current move square is capturable
            if (CapturableAbove(x, y)) {InitiateAttackPlates(capturableX, capturableY);}
            // indicate if a piece below the current move square is capturable
            if (CapturableBelow(x, y)) {InitiateAttackPlates(capturableX, capturableY);}
            // indicate if a piece to the left of the current move square is capturable
            if (CapturableLeft(x, y)) {InitiateAttackPlates(capturableX, capturableY);}
            // indicate if a piece to the right of the current move square is capturable
            if (CapturableRight(x, y)) {InitiateAttackPlates(capturableX, capturableY);}
            */
        }
    }

    // instantiates the moveplates on the board
    public void InitiateMovePlates(int matrixX, int matrixY)
    {
        // Game sc = controller.GetComponent<Game>();
        float x = matrixX, y = matrixY;
        int index = 0; //index of the coordinate arrays

        x *= 0.74f;
        y *= 0.74f;

        x += -3.7f;
        y += -3.7f;

        // sets the position on the unity worldspace
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity, mpParent.transform);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        // sets the position on our 2D array
        mpScript.SetCoords(matrixX, matrixY);

        // indicate if a piece above the current move square is capturable
        if (CapturableVert(matrixX, matrixY, 1))
        {
            mpScript.attack = true;
            mpScript.SetCapturableCoords(matrixX, matrixY + 1, index++) ;
            // capturableX = matrixX;
            // capturableY = matrixY + 1;
            Debug.Log(mpScript.GetCapturableX() + ", " + mpScript.GetCapturableY());
            // return;
            
        }
        // indicate if a piece below the current move square is capturable
        if (CapturableVert(matrixX, matrixY, -1))
        {
            mpScript.attack = true;
            mpScript.SetCapturableCoords(matrixX, matrixY - 1, index++);
            // capturableX = matrixX;
            // capturableY = matrixY - 1;
            Debug.Log(mpScript.GetCapturableX() + ", " + mpScript.GetCapturableY());
            // return;
        }
        // indicate if a piece to the left of the current move square is capturable
        if (CapturableHorz(matrixX, matrixY, -1))
        {
            mpScript.attack = true;
            mpScript.SetCapturableCoords(matrixX - 1, matrixY, index++);
            // capturableX = matrixX - 1;
            // capturableY = matrixY;
            Debug.Log(mpScript.GetCapturableX() + ", " + mpScript.GetCapturableY());
            // return;
        }
        // indicate if a piece to the right of the current move square is capturable
        if (CapturableHorz(matrixX, matrixY, 1))
        {
            mpScript.attack = true;
            mpScript.SetCapturableCoords(matrixX + 1, matrixY, index++);
            // capturableX = matrixX + 1;
            // capturableY = matrixY;
            Debug.Log(mpScript.GetCapturableX() + ", " + mpScript.GetCapturableY());
            // return;
        }
    }

    /*
    // instantiates the attack moveplates on the board
    public void InitiateAttackPlates(int matrixX, int matrixY)
    {

        float x = matrixX, y = matrixY;

        x *= 0.74f;
        y *= 0.74f;

        x += -3.7f;
        y += -3.7f;

        // sets the position on the unity worldspace
        GameObject ap = Instantiate(attackPlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        AttackPlate apScript = ap.GetComponent<AttackPlate>();
        // apScript.attack = true;
        apScript.SetReference(gameObject);
        // sets the position on our 2D array
        apScript.SetCoords(matrixX, matrixY);
    }
    */
}
