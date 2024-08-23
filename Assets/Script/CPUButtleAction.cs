using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUButtleAction : MonoBehaviour
{
    public CPULookTarget look = null;

    public CPURootingScript rooting = null;

    public ChUnity.Transform.ObjectMove move = null;
    public ChUnity.Transform.ObjectRotate rotate = null;

    public ChUnity.Common.CountDownEvent shot = null;

    public float disarmamentByLostPosLen = 0.0f;

    [System.NonSerialized]
    public bool floatingEnemyFlg = false;

    Vector3 lastLookPos = Vector3.zero;

    bool initFlg = false;
    bool cowardlyFlg = false;

    public int cowardlyParcec = 5;
    public static int cowardlyMaxParcec = 10;

    public CollisionEventBase damageData = null;
    public void ButtleByDamage()
    {
        lastLookPos = move.transform.position + (damageData.hitObjectDirection * Vector3.back * 0.5f);
        lastLookPos.y = move.transform.position.y;

        //GameObject tmp = new GameObject();
        //tmp.transform.position = lastLookPos;

        floatingEnemyFlg = true;
    }

    private void Start()
    {
        if (cowardlyParcec >= cowardlyMaxParcec) cowardlyParcec = cowardlyMaxParcec-1;
    }

    // Update is called once per frame
    void Update()
    {
        if (look == null) return;
        if (rooting == null) return;
        if (shot == null) return;
        if (move == null) return;
        if (rotate == null) return;

        if (look.lookTarget.Count <= 0 && !floatingEnemyFlg)
        {
            rooting.Start();
            look.battleFlg = false;
            return;
        }

        Vector3 lastLookDir = lastLookPos - move.transform.position;
        float distance =
            Mathf.Abs(lastLookDir.x) +
            Mathf.Abs(lastLookDir.y) +
            Mathf.Abs(lastLookDir.z);

        floatingEnemyFlg = true;

        if (disarmamentByLostPosLen >= distance)
        {
            rooting.nowPos = null;
            rooting.nextPos = null;
            floatingEnemyFlg = false;
            initFlg = false;
            return;
        }

        look.battleFlg = true;

        if (!initFlg)
        {
            cowardlyFlg = cowardlyParcec > Random.Range(0, cowardlyMaxParcec) ? true : false;
            initFlg = true;
        }

        float near = 1000.0f;

        bool lookFlg = false;

        Vector3 to = Vector3.zero;

        foreach (var look in look.lookTarget)
        {
            if (look == null) continue;

            Vector3 tmp = look.transform.position - move.transform.position;
            distance = Mathf.Abs(to.x) + Mathf.Abs(to.y) + Mathf.Abs(to.z);

            if (near < distance) continue;

            near = distance;
            to = tmp;
            lastLookPos = look.transform.position;
            lookFlg = true;
        }

        if (!lookFlg) to = lastLookPos - move.transform.position;

        float rot = 0.0f;

        Vector3 from = move.transform.forward;

        rot = Vector3.SignedAngle(from, to.normalized, Vector3.down);

        if (Mathf.Abs(rot) < (rotate.rotSize))
        {
            if (lookFlg)
            {
                shot.CountDown();
                if (!cowardlyFlg) move.MoveFoward();
                else move.MoveBack();
            }
            else
            {
                move.MoveFoward();
            }

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
