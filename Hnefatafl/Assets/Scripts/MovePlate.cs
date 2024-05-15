using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;
    private Rigidbody2D rigidbod;
    private AudioSource captureSound, moveSound;
    private AudioClip captureSoundClip, moveSoundClip;
    GameObject reference = null; // refers to the piece associated with the moveplates created

    // board positions (not world positions)
    int matrixX, matrixY;

    // coordinates for the capturable piece associated with the square (if there is one)
    int[] capturableX = new int[3], capturableY = new int[3];
    int index = 0;
    //int[][,] capturableCoords;

    public bool attack = false; // false if only moving, true if moving and attacking

    public void Start()
    {
        // capturableCoords = new int[3][,];
        //capturableX = new int[3];
        //capturableY = new int[3];
        controller = GameObject.FindGameObjectWithTag("GameController");
        rigidbod = reference.GetComponent<Taflman>().GetComponent<Rigidbody2D>();
        captureSound = GetComponent<AudioSource>();
        captureSoundClip = captureSound.clip;
        moveSound = reference.GetComponent<Taflman>().GetComponent<AudioSource>();
        moveSoundClip = moveSound.clip;
        if (attack)
        {
            // change to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    // controls what happens when one clicks on a piece
    public void OnMouseUp()
    {
        //controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Game>().moving = true;

        if (attack)
        {
            //captureSound.Play();
            AudioSource.PlayClipAtPoint(captureSoundClip, new Vector3(0, 0, -1.0f));
            index = 0;
            while (controller.GetComponent<Game>().GetPosition(capturableX[index], capturableY[index]) != null)
            {
                GameObject tp = controller.GetComponent<Game>().GetPosition(capturableX[index], capturableY[index]);
                Debug.Log("Capturable coordinates: " + capturableX[index] + ", " + capturableY[index]);
                controller.GetComponent<Game>().SetPositionEmpty(capturableX[index], capturableY[index]);
                Destroy(tp);

                if(controller.GetComponent<Game>().GetCurrentPlayer() == "black")
                {
                    controller.GetComponent<Game>().blackScore++;
                }
                else if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
                {
                    controller.GetComponent<Game>().whiteScore++;
                }
                else { Debug.Log("this shouldn't be happening..."); }

                if (tp.name == "KingPiece")
                {
                    controller.GetComponent<Game>().Winner("black"); // black victory
                }
                index++;
            }
        }
        else { AudioSource.PlayClipAtPoint(moveSoundClip, new Vector3(0, 0, -1.0f)); }

        //Vector2 position = new Vector2(reference.GetComponent<Taflman>().GetXBoard(), reference.GetComponent<Taflman>().GetYBoard());
        //Vector2.Lerp(position, new Vector2(matrixX, matrixY), Time.deltaTime * 0.01f);
        //rigidbod.velocity = new Vector2(1, 1);

        // set the reference's original position to empty
        controller.GetComponent<Game>().SetPositionEmpty
            (
            reference.GetComponent<Taflman>().GetXBoard(), reference.GetComponent<Taflman>().GetYBoard()
            );
        reference.GetComponent<Taflman>().SetXBoard(matrixX);
        reference.GetComponent<Taflman>().SetYBoard(matrixY);

        // reference.GetComponent<Taflman>().MovePiece(matrixX, matrixY);
        // StartCoroutine(MoveObject(reference.GetComponent<Taflman>().transform.position, this.transform.position, 1.0f));
        /*
        while (reference.GetComponent<Taflman>().GetXBoard() != matrixX || reference.GetComponent<Taflman>().GetYBoard() != matrixY)
        {
            // reference.GetComponent<Taflman>().MovePieceOnce(matrixX, matrixY);
            StartCoroutine(MoveObject(reference.GetComponent<Taflman>().transform.position, new Vector3(matrixX, matrixY, -1.0f), 1.0f));
            reference.GetComponent<Taflman>().SetCoords();
            controller.GetComponent<Game>().SetPosition(reference);
            // StartCoroutine(TimeDelay());
        }
        */

        // controller.GetComponent<Game>().moving = false;
        reference.GetComponent<Taflman>().SetCoordsMove((float)matrixX, (float)matrixY);

        controller.GetComponent<Game>().SetPosition(reference);

        controller.GetComponent<Game>().moving = false;

        if (reference.GetComponent<Taflman>().name == "KingPiece" &&
            controller.GetComponent<Game>().IsCornerSquare(matrixX, matrixY))
        {
            controller.GetComponent<Game>().Winner("white"); // white victory
        }

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<Taflman>().DestroyMovePlates(); // maybe move this to before the if statement
    }

    /*
    IEnumerator MoveObject(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
    }

    IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(5f);
    }
    */

    public int GetCapturableX()
    {
        return capturableX[index];
    }

    public int GetCapturableY()
    {
        return capturableY[index];
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetCapturableCoords(int x, int y, int index)
    {
        capturableX.SetValue(x, index);
        capturableY.SetValue(y, index);
        Debug.Log(capturableX[index] + ", " + capturableY[index]);
        // capturableCoords[0, index] = x;
        // capturableCoords[1, index] = y;
        // if (index < 2) { index++; }
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

    public bool OnSquare(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        return sc.GetPosition(x, y) == gameObject;
    }
}
