using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{
	public class WASDMovement : MonoBehaviour {
		public Transform moveTarget;
		public Transform referenceView;
		public float speed=2f;
		public float jumpHeight=1f;

		// Use this for initialization
		void Start () {
			
		}
		// Update is called once per frame
		void FixedUpdate () {
			Vector3 movement=Vector3.zero;
			if(Input.GetKey(KeyCode.W))
				movement+=referenceView.forward;
			if(Input.GetKey(KeyCode.S))
				movement-=referenceView.forward;
			if(Input.GetKey(KeyCode.D))
				movement+=referenceView.right;
			if(Input.GetKey(KeyCode.A))
				movement-=referenceView.right;
			movement=Quaternion.FromToRotation(referenceView.up,moveTarget.transform.up)*movement;
			movement*=speed*Time.fixedDeltaTime;
			moveTarget.transform.Translate(movement,Space.World);
			//if(Input.GetKeyDown(KeyCode.Space)){
			//	moveTarget.AddForce(Mathf.Sqrt(2*10*jumpHeight)*moveTarget.transform.up,ForceMode.VelocityChange);
			//}
		}
	}
}