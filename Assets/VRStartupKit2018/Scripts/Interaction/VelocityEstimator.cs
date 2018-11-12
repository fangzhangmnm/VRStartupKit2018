using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{

	public class VelocityEstimator : MonoBehaviour {
		public Vector3 estimatedVelocity;
		public Vector3 estimatedAngularVelocity;
		Vector3 lastX;
		Quaternion lastR;
		const float lerpFactor=.5f;
		Rigidbody rigid;
		void Start(){
			rigid=GetComponent<Rigidbody>();
		}
		void Update(){
            //if(!rigid || rigid.isKinematic==true){
            Vector3 X;
            Quaternion R;
            if (rigid)
            {
                X = rigid.position;
                R = rigid.rotation;
            }
            else
            {
                X = transform.position;
                R = transform.rotation;
            }
            Vector3 V=(X-lastX)/Time.deltaTime;lastX=X;
				estimatedVelocity=Vector3.Lerp(estimatedVelocity,V,lerpFactor);
				Vector3 W=(R*Quaternion.Inverse(lastR)).eulerAngles;lastR=R;
				if(W.x>180)W.x-=360;
				if(W.y>180)W.y-=360;
				if(W.z>180)W.z-=360;
				W/=Time.deltaTime;
				estimatedAngularVelocity=Vector3.Lerp(estimatedAngularVelocity,W,lerpFactor);
			//}else{
			//	estimatedVelocity=rigid.velocity;
			//	estimatedAngularVelocity=rigid.angularVelocity;
			//}
		}
	}
}