using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public List<GameObject> charactorList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i<charactorList.Count; i++)
        {
            if (charactorList[i] != null) continue;
            charactorList.RemoveAt(i);
            i--;
        }

    }
}
