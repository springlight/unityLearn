using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    [SerializeField]
    float dyingDuration;
    private void OnTriggerExit(Collider other)
    {
        var shape = other.GetComponent<Shape>();
        if(shape)
        {
            if(dyingDuration <= 0f)
            {
                shape.Die();
            }
            else if(!shape.IsMarkedAsDying)
            {
                shape.AddBehavior<DyingShapeBehavior>().Initiallize(shape, dyingDuration);
            }
           
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //     Gizmos.matrix = transform.localToWorldMatrix;
        var c = GetComponent<Collider>();
        var b = c as BoxCollider;
        if (b != null)
        {
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position, transform.rotation, transform.lossyScale
            );
            Gizmos.DrawWireCube(b.center, b.size);
            return;
        }
        var s = c as SphereCollider;
        if (s != null)
        {
            Vector3 scale = transform.lossyScale;
            scale = Vector3.one * Mathf.Max(
                Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z)
            );
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position, transform.rotation, scale
            );
            Gizmos.DrawWireSphere(s.center, s.radius);
            return;
        }
    }
}
