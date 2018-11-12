using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{
	public class CustomCollisionResolver : MonoBehaviour {
		HashSet<Collider> ignoredColliders;
		void FixedUpdate(){
			ignoredColliders=new HashSet<Collider>(gameObject.GetComponentsInChildren<Collider>());
		}
		void OnCollisionStay(Collision collision){
			if(!this.isActiveAndEnabled)
				return;
			if(ignoredColliders.Contains(collision.collider))
				return;
			foreach(var h in collision.contacts){
				transform.Translate(-h.normal*h.separation);
			}
		}
	}
}
