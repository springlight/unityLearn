using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMove : MonoBehaviour {

    public float DirectionDampTime = 0.25f;
    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(animator == null)
        {
            return;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Base Layer.Run"))
        {
            if (Input.GetButton("Fire1"))
            {
                animator.SetBool("Jump", true);
            }
        }
        else
            animator.SetBool("Jump", false);

        if(Input.GetButtonDown("Fire2") && animator.layerCount >= 2)
        {
            animator.SetBool("Hi", !animator.GetBool("Hi"));
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        animator.SetFloat("Speed", h * h + v * v);
        animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);
	}
}
