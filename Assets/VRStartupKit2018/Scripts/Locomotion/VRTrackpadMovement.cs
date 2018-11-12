using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
namespace VRStartupKit2018
{
	public class VRTrackpadMovement : MonoBehaviour {
		public Transform moveTarget;
		public TrackedPoseDriver Left;
		public TrackedPoseDriver Right;
		[Tooltip("Movement Speed without ArmSwinger")]
		public float speed=2f;
		public bool useArmSwinger=false;
		public float armSwingerMultiplier=1f;
        public VRHand.WhichHand trackpadHand = VRHand.WhichHand.Left;
        Vector3 oldL,oldR;
		void Start(){
			oldL=Left.transform.localPosition;
			oldR=Right.transform.localPosition;
		}
		float getArmSwingerAmount(){
			Vector3 newL,newR;
			newL=Left.transform.localPosition;
			newR=Right.transform.localPosition;
			float amount=Vector3.Distance(newL,oldL)+Vector3.Distance(newR,oldR);
			oldL=newL;oldR=newR;
			return amount;
		}
		void FixedUpdate ()
        {
            string handName = trackpadHand == VRHand.WhichHand.Left ? "Left" : "Right";
			/* 
            if (Input.GetButton(handName+"TrackpadTouch") && !Input.GetButton(handName+"TrackpadPress")){
				float h=Input.GetAxis(handName+"TrackpadHorizontal");
				float v=Input.GetAxis(handName+"TrackpadVertical");
				VRDebug.Log(h+" "+v+"\n");
				Transform t=Left.transform;
				Vector3 movement=h*t.right+v*t.forward;
				movement=Quaternion.FromToRotation(t.up,moveTarget.up)*movement;
				float armSwingerAmount=getArmSwingerAmount();
				if(useArmSwinger)
					movement*=armSwingerAmount*armSwingerMultiplier;
				else
					movement*=speed*Time.fixedDeltaTime;
				moveTarget.Translate(movement,Space.World);
			}else 
			*/
			if (!Input.GetButton(handName+"TrackpadPress")){
				float h=Input.GetAxis(handName+"ThumbstickHorizontal");
				float v=Input.GetAxis(handName+"ThumbstickVertical");
				if((new Vector2(v,h)).magnitude>0.1f){
					VRDebug.Log(h+" "+v+"\n");
					Transform t=Left.transform;
					Vector3 movement=h*t.right+v*t.forward;
					movement=Quaternion.FromToRotation(t.up,moveTarget.up)*movement;
					float armSwingerAmount=getArmSwingerAmount();
					if(useArmSwinger)
						movement*=armSwingerAmount*armSwingerMultiplier;
					else
						movement*=speed*Time.fixedDeltaTime;
					moveTarget.Translate(movement,Space.World);
				}
			}
		}
	}
}

