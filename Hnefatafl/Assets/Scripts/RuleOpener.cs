using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleOpener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp()
    {
        Application.OpenURL("file://" + Application.dataPath + "/Files/HnefataflDocumentation.pdf");
    }
}
