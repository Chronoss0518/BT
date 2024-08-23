using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CPULookTarget : MonoBehaviour
{
    static Vector3[] mulPos = new Vector3[4]
    {
        new Vector3(-1.0f,1.0f,-1.0f),
        new Vector3(1.0f,1.0f,-1.0f),
        new Vector3(-1.0f,1.0f,1.0f),
        new Vector3(1.0f,1.0f,1.0f)
    };


    public Master master = null;
    public ChUnity.Transform.ObjectMove move = null;
    public Camera targetCamera = null;

    [System.NonSerialized]
    public bool battleFlg = false;

    List<GameObject> targetList = new List<GameObject>();

    [System.NonSerialized]
    public List<GameObject> lookTarget = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (master == null) return;
        if (targetList.Count > 0) return;

        foreach (GameObject go in master.charactorList)
        {
            if (go.name ==  move.gameObject.name) continue;
            targetList.Add(go);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Start();

        lookTarget.Clear();

        if (move == null) return;
        if (targetList.Count <= 0) return;

        Matrix4x4 viewMat = targetCamera.worldToCameraMatrix;

        Matrix4x4 projMat = GL.GetGPUProjectionMatrix(targetCamera.projectionMatrix,false);

        for (int i = 0; i<targetList.Count; i++)
        {

            if (targetList[i] == null)
            {
                targetList.RemoveAt(i);
                i -= 1;
                continue;

            }

            Matrix4x4 worldMat = targetList[i].transform.localToWorldMatrix;

            var col = targetList[i].transform.GetChild(0).GetComponent<BoxCollider>();

            for(int j = 0;j<mulPos.Length; j++)
            {
                Vector4 pos = Vector4.zero;
                Vector3 worldPos = Vector3.zero;

                pos = mulPos[j];
                pos.x *= col.size.x / 2.0f;
                pos.y *= col.size.y / 2.0f;
                pos.z *= col.size.z / 2.0f;

                pos.x += col.center.x;
                pos.y += col.center.y;
                pos.z += col.center.z;
                pos.w = 1.0f;

                pos = worldMat * pos;
                worldPos = pos;
                pos = viewMat * pos;
                pos = projMat * pos;
                
                float z = pos.z / pos.w;

                if (Mathf.Abs(pos.x / pos.w) > 1.0f ||
                    Mathf.Abs(pos.y / pos.w) > 1.0f ||
                    z > 1.0f || z  < 0.0f) continue;


                RaycastHit[] hit;

                Vector3 tmpPos = targetCamera.transform.position;

                Vector3 tmp = worldPos - targetCamera.transform.position;

                hit = Physics.RaycastAll(tmpPos, tmp.normalized);

                if (hit.Length <= 0) continue;

                float minLen = 10000.0f;


                GameObject target = null;

                for (int k = 0; k<hit.Length; k++)
                {
                    if (hit[k].collider.gameObject.tag == "RootObject") continue;
                    if (hit[k].collider.name == move.name) continue;
                    

                    if (minLen < hit[k].distance) continue;

                    minLen = hit[k].distance;
                    target = hit[k].collider.gameObject;
                    
                }


                if (target.name != targetList[i].name) continue;

                lookTarget.Add(targetList[i]);

                break;

            }

        }


    }
}
