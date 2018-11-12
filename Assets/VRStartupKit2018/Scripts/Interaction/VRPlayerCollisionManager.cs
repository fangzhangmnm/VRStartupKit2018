using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRStartupKit2018
{
	public class VRPlayerCollisionManager : MonoBehaviour {
		HashSet<Collider> pickups=new HashSet<Collider>();
		[SerializeField] private Collider[] bodyColliders;
		public bool IsRegistered(Collider h){return pickups.Contains(h);}
		public void RegisterCollider(Collider h){
			if(pickups.Contains(h)){
				Debug.LogError(gameObject.name+": Register a registered collider "+h.gameObject.name);
				return;
			}
			if(h.isTrigger){
				Debug.LogError(gameObject.name+": Register a trigger as a collider "+h.gameObject.name);
				return;
			}
			pickups.Add(h);
			foreach(var hh in bodyColliders)
				Physics.IgnoreCollision(h,hh,true);
		}
		public void UnregisterCollider(Collider h){
			if(!pickups.Contains(h)){
				Debug.LogError(gameObject.name+": Unregister an unregistered gameobject");
				return;
			}
			pickups.Remove(h);
			foreach(var hh in bodyColliders)
				Physics.IgnoreCollision(h,hh,false);
		}
	}
}