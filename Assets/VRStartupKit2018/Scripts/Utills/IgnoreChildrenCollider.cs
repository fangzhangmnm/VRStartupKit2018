using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{
	public class IgnoreChildrenCollider : MonoBehaviour {
		public Collider[] bodyColliders;
		HashSet<Collider> ignored;
		public Collider[] additionalColliders;
		void Start(){
			ignored=new HashSet<Collider>();
		}
		void FixedUpdate(){
			var childrenColliders=GetComponentsInChildren<Collider>();
			var newIgnored=new HashSet<Collider>();
			foreach(var h in childrenColliders)
				if(!h.isTrigger){
					newIgnored.Add(h);
					if(!ignored.Contains(h))
						foreach(var hh in bodyColliders)
							Physics.IgnoreCollision(h,hh,true);
				}
			foreach(var h in additionalColliders)
				if(!h.isTrigger){
					newIgnored.Add(h);
					if(!ignored.Contains(h))
						foreach(var hh in bodyColliders)
							Physics.IgnoreCollision(h,hh,true);
				}
			foreach(var h in ignored)
				if(!newIgnored.Contains(h))
					foreach(var hh in bodyColliders)
						Physics.IgnoreCollision(h,hh,false);
			ignored=newIgnored;
		}
	}
}