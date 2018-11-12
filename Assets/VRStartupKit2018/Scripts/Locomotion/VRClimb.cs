using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

namespace VRStartupKit2018
{
	public class VRClimb : MonoBehaviour {
		public Transform player;
		public Rigidbody playerRigidbody;
		public TrackedPoseDriver Left;
		public TrackedPoseDriver Right;
		public float handRadius=.05f;
		public bool isClimbing{get{return leftClimbing||rightClimbing;}}
		

		bool leftClimbing,rightClimbing;
		Transform leftClimbingTarget,rightClimbingTarget;
		Vector3 oldLeftLocal,oldRightLocal;
		
		void Start(){
			leftClimbing=rightClimbing=false;
		}
		void Update () {
			if(!Input.GetButton("LeftTriggerTouch") || leftClimbingTarget==null)
				leftClimbing=false;
			if(!Input.GetButton("RightTriggerTouch") || leftClimbingTarget==null)
				rightClimbing=false;
			if(Input.GetButtonDown("LeftTriggerTouch")){
				Collider[] info=Physics.OverlapSphere(Left.transform.position-Left.transform.up*handRadius,	handRadius*1.01f);
				foreach(var h in info){
					if(h.GetComponent<VRClimbableSurface>()!=null){
						leftClimbing=true;
						leftClimbingTarget=h.transform;
						oldLeftLocal=leftClimbingTarget.InverseTransformPoint(Left.transform.position);
						break;
					}
				}
			}
			if(Input.GetButtonDown("RightTriggerTouch")){
				Collider[] info=Physics.OverlapSphere(Right.transform.position-Right.transform.up*handRadius,handRadius*1.01f);
				foreach(var h in info){
					if(h.GetComponent<VRClimbableSurface>()!=null){
						rightClimbing=true;
						rightClimbingTarget=h.transform;
						oldRightLocal=rightClimbingTarget.InverseTransformPoint(Right.transform.position);
						break;
					}
				}
			}
			if(leftClimbing || rightClimbing){
				playerRigidbody.isKinematic=true;
				Vector3 dx=Vector3.zero;
				if(leftClimbing)
					dx+=leftClimbingTarget.TransformPoint(oldLeftLocal)-Left.transform.position;
				if(rightClimbing)
					dx+=rightClimbingTarget.TransformPoint(oldRightLocal)-Right.transform.position;
				if(leftClimbing && rightClimbing)
					dx/=2;
				player.Translate(dx,Space.World);
				if(leftClimbing)
					oldLeftLocal=leftClimbingTarget.InverseTransformPoint(Left.transform.position);
				if(rightClimbing)
					oldRightLocal=rightClimbingTarget.InverseTransformPoint(Right.transform.position);
			}else{
				playerRigidbody.isKinematic=false;
			}
		}
	}
}