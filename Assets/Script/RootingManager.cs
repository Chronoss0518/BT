using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public class Rooting
{
    public GameObject nowPos = null;
    public List<GameObject> nextPos = null;
}

public class RootingManager : MonoBehaviour
{
    public List<Rooting> rootList = new List<Rooting>();

    private Dictionary<string, List<GameObject>> usingRootList = new Dictionary<string, List<GameObject>>();

    public Dictionary<string, List<GameObject>> rootingDictionary { get { return usingRootList; } }

    // Start is called before the first frame update
    public void Start()
    {
        if (usingRootList.Count > 0) return;

        foreach (Rooting rooting in rootList)
        {
            usingRootList[rooting.nowPos.name] = rooting.nextPos;
        }
    }

}
