using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyIK : MonoBehaviour {

    public Transform bodyObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;
    public Transform leftHandObj = null;
    public Transform rightHandObj = null;
    public Transform lookAtObj = null;
    private Animator avatar;
    public bool ikActive = false;
	// Use this for initialization
	void Start () {
        avatar = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!ikActive)
        {
            if(bodyObj != null)
            {
                bodyObj.position = avatar.bodyPosition;
                bodyObj.rotation = avatar.bodyRotation;
            }
            if(leftFootObj != null)
            {
                leftFootObj.position = avatar.GetIKPosition(AvatarIKGoal.LeftFoot);
                leftFootObj.rotation = avatar.GetIKRotation(AvatarIKGoal.LeftFoot);
            }
            if(rightFootObj != null)
            {
                rightFootObj.position = avatar.GetIKPosition(AvatarIKGoal.RightFoot);
                rightFootObj.rotation = avatar.GetIKRotation(AvatarIKGoal.RightFoot);
            }

            if(leftHandObj != null)
            {
                leftHandObj.position = avatar.GetIKPosition(AvatarIKGoal.LeftHand);
                leftHandObj.rotation = avatar.GetIKRotation(AvatarIKGoal.LeftHand);
            }
            if(rightHandObj != null)
            {
                rightHandObj.position = avatar.GetIKPosition(AvatarIKGoal.RightHand);
                rightHandObj.rotation = avatar.GetIKRotation(AvatarIKGoal.RightHand);
            }
            if(lookAtObj != null)
            {
                lookAtObj.position = avatar.bodyPosition + avatar.bodyRotation * new Vector3(0, 0.5f, 1);
            }
        }
		
	}

    private void OnAnimatorIK(int layerIndex)
    {
        if (avatar == null) return;
        if (ikActive)
        {
            avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
            avatar.SetLookAtWeight(1.0f, 0.3f, 0.6f, 1.0f, 0.5f);
            if(bodyObj != null)
            {
                avatar.bodyPosition = bodyObj.position;
                avatar.bodyRotation = bodyObj.rotation;
            }
            if(leftFootObj != null)
            {
                avatar.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
                avatar.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
            }
            if (lookAtObj != null)
                avatar.SetLookAtPosition(lookAtObj.position);

            avatar.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);
            if(rightFootObj != null)
            {
                avatar.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
                avatar.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);
            }

            avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            if (leftHandObj != null)
            {
                avatar.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                avatar.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
            }

            avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            if (rightHandObj != null)
            {
                avatar.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                avatar.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
            }


        }
        else
        {
            avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.0f);

            avatar.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.0f);

            avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);

            avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);
            avatar.SetLookAtWeight(0.0f);
        }
    }
}
