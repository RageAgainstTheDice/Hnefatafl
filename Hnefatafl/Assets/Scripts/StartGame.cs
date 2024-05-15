using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp()
    {
        StartCoroutine(ButtonClick());
        //SceneManager.LoadScene("GameScreen");
    }

    IEnumerator ButtonClick()
    {
        yield return new WaitForSeconds(1.46f);
        SceneManager.LoadScene("GameScreen");
    }
}
