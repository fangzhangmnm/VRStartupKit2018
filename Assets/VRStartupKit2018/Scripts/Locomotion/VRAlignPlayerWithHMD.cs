using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRStartupKit2018
{
    //TODO Scale
	public class VRAlignPlayerWithHMD : MonoBehaviour {
		public Transform player;
		public CapsuleCollider playerCollider;
		HashSet<Collider> ignoredColliders;
		public Transform HMD;
		public Transform Basis;
        public float eyeToTop = 0.1f;

		public float climbSlope=1;

		public float pullbackDistance=.4f;
		
		public float pushRate=10f;
		public LayerMask colliderMask;

		public bool preventFalling=true;
		public float maxFallDistance=0.3f;
		public LayerMask preventFallingMask;

        public bool alignRotationY=true;
		void Start(){
			Basis.Translate(Vector3.ProjectOnPlane(player.position-HMD.position,player.up),Space.World);
		}
		void FixedUpdate () {
			ignoredColliders=new HashSet<Collider>( player.gameObject.GetComponentsInChildren<Collider>());
			//try to move the player
			Vector3 dx=Vector3.ProjectOnPlane(HMD.position-player.position,player.up);
			dx*=pushRate*Time.fixedDeltaTime;
			if(ForwardCheck(dx) && FallCheck(dx)){
				player.Translate(dx,Space.World);
				//move back HMD
				Basis.Translate(-dx,Space.World);
			}else{
				Vector3 deltax=Vector3.ProjectOnPlane(player.position-HMD.position,player.up);
				//pull back HMD
				if(deltax.magnitude>pullbackDistance)
					Basis.Translate(deltax,Space.World);
			}
			//try to stretch the capsule
			float dh=Vector3.Dot(HMD.position-Basis.position,player.up)-(playerCollider.height- eyeToTop);
			if(StandUpCheck(dh)){
				playerCollider.height=Mathf.Max(playerCollider.height+dh,2*playerCollider.radius);
				playerCollider.center=new Vector3(0,playerCollider.height/2,0);
			}
			{
				//align HMD with capsule
				float deltah=Vector3.Dot(HMD.position-player.position,player.up)-(playerCollider.height- eyeToTop);
				Basis.Translate(-player.up*deltah);
			}
            //rotate Y axis
            if(alignRotationY){
                //TODO >180
                Quaternion r = HMD.localRotation;
                Quaternion t = Quaternion.FromToRotation(Basis.InverseTransformDirection(HMD.up), Vector3.up);
                r = t * r;
                r= Basis.rotation * r*Quaternion.Inverse( player.transform.rotation);
                Vector3 tmpA = Basis.position;
                Quaternion tmpB = Basis.rotation;
                player.RotateAround(HMD.position, Vector3.up, r.eulerAngles.y);
                Basis.position = tmpA;
                Basis.rotation = tmpB;
            }

		}
		
		public bool ForwardCheck(Vector3 dx){
			Vector3 dir=Vector3.Normalize(dx);
			RaycastHit[] infos;
			infos=Physics.CapsuleCastAll(
				player.position+player.up*playerCollider.radius,
				player.position+player.up*(playerCollider.height-playerCollider.radius),
				playerCollider.radius-dx.magnitude*.01f, dir,dx.magnitude,colliderMask);
			foreach (var h in infos){
				if(ignoredColliders.Contains(h.collider))
					continue;
				float dotThreshold=1/Mathf.Sqrt(1+climbSlope*climbSlope);
				if(Vector3.Dot(h.normal,player.up)<dotThreshold){
					//Debug.Log(h.point+" "+h.normal);
					return false;
				}
			}
			//Debug.Log("Nope");
			return true;
		}
		public bool FallCheck(Vector3 dx){
			RaycastHit[] infos;
			infos=Physics.RaycastAll(player.position+dx*1.1f+player.up*playerCollider.radius,-player.up,playerCollider.radius+maxFallDistance,preventFallingMask);
			foreach (var h in infos){
				if(ignoredColliders.Contains(h.collider))
					continue;
				return true;
			}
			return false;
		}

		public bool StandUpCheck(float dh){
			if(dh<0)return true;
			RaycastHit[] infos;
			infos=Physics.SphereCastAll(player.position+player.up*(Mathf.Max(playerCollider.height-playerCollider.radius,playerCollider.radius)),playerCollider.radius-dh*.01f,player.up,dh,colliderMask);
			foreach (var h in infos){
				if(ignoredColliders.Contains(h.collider))
					continue;
				//Debug.Log(h.point+" "+h.normal);
				return false;
			}
			return true;
		}
		
	}
}