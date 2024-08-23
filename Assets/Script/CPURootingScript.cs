using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CPURootingScript : MonoBehaviour
{
    public RootingManager manager = null;

    public float useRootingLen = 0.01f;

    public ChUnity.Transform.ObjectMove move = null;
    public ChUnity.Transform.ObjectRotate rotate = null;

    public CPULookTarget look = null;

    [System.NonSerialized]
    public GameObject nowPos = null;

    [System.NonSerialized]
    public GameObject nextPos = null;

    // Start is called before the first frame update
    public void Start()
    {

        if (manager == null) return;
        if (nowPos != null) return;

        Vector3 tmp = Vector3.zero;
        float len = 100000.0f;

        foreach (var rooting in manager.rootList)
        {
            tmp = rooting.nowPos.transform.position - move.transform.position;

            if (len < tmp.magnitude) continue;

            len = tmp.magnitude;
            nowPos = rooting.nowPos;
        }

        Rooting();
    }

    public void Rooting()
    {
        if (manager == null) return;
        if (nowPos == null) return;

        manager.Start();

        if (nextPos == null)
        {
            var startlist = manager.rootingDictionary[nowPos.name];

            nextPos = startlist[Random.Range(0,startlist.Count)];

            return;
        }

        Vector3 tmp = nextPos.transform.position - move.transform.position;

        if (Mathf.Abs(tmp.x) + Mathf.Abs(tmp.y) + Mathf.Abs(tmp.z) > useRootingLen) return;

        nowPos = nextPos;

        var list = manager.rootingDictionary[nowPos.name];
        nextPos = list[Random.Range(0, list.Count)];

    }

    // Update is called once per frame
    void Update()
    {
        if (look == null) return;
        if (move == null) return;

        Start();

        Rooting();

        if (manager == null) return;
        if (nextPos == null) return;
        if (look.battleFlg) return;

        float rot = 0.0f;

        Vector3 from = move.transform.forward;
        Vector3 to = nextPos.transform.position - move.transform.position;

        rot = Vector3.SignedAngle(from, to.normalized,Vector3.down);

        if (Mathf.Abs(rot) < (rotate.rotSize))
        {
            move.MoveFoward();
            return;
        }

        if (rot > 0.0f)
        {
            rotate.RotAxisYInverse();
        }
        else
        {
            rotate.RotAxisY();
        }
    }
}
