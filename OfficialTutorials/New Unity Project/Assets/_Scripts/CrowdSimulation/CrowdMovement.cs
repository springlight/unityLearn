using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovement : MonoBehaviour
{
    public float AvatarRange = 25;
    private Animator animator;
    private float SpeedDampTime = 0.25f;
    private float DirectionDampTime = 0.25f;
    private Vector3 TargetPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (animator == null) return;
        int r = Random.Range(0, 50);
        animator.SetBool("Jump", r == 20);
        animator.SetBool("Dive", r == 30);
        if(Vector3.Distance(TargetPosition,animator.rootPosition) > 5)
        {
            animator.SetFloat("Speed", 1, SpeedDampTime, Time.deltaTime);
            Vector3 currentDir = animator.rootRotation * Vector3.forward;
            Vector3 wantedDir = (TargetPosition - animator.rootPosition).normalized;
            if(Vector3.Dot(currentDir,wantedDir) > 0)
            {
                animator.SetFloat("Direction", Vector3.Cross(currentDir, wantedDir).y, DirectionDampTime, Time.deltaTime);
            }
            else
            {
                animator.SetFloat("Direction", Vector3.Cross(currentDir, wantedDir).y > 0 ? 1 : -1, DirectionDampTime, Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0, SpeedDampTime, Time.deltaTime);
            if(animator.GetFloat("Speed") < 0.01f)
            {
                TargetPosition = new Vector3(Random.Range(-AvatarRange, AvatarRange), 0, Random.Range(-AvatarRange, AvatarRange));
            }
        }
	}
}
