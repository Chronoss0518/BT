using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : CollisionEventBase
{
    public GameObject targetObject = null;

    protected override bool IsTargetObject(GameObject _obj)
    {
        return ReferenceEquals(_obj,targetObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        base.CollisionStay(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        base.CollisionExit(collision);
    }

    void OnTriggerEnter(Collider collision)
    {
        base.TriggerEnter(collision);
    }

    void OnTriggerStay(Collider collision)
    {
        base.TriggerStay(collision);
    }

    void OnTriggerExit(Collider collision)
    {
        base.TriggerExit(collision);
    }

}
