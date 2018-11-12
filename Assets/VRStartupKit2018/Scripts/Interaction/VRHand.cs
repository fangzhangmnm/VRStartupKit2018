using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRStartupKit2018
{
	public class VRHand : MonoBehaviour {
		public enum WhichHand{Left,Right};
		public WhichHand whichHand;
		public bool isHolding{get{return m_attachedObject;}}
		public VRPickup attachedObject{get{return m_attachedObject;}}
		public Transform attachedObjects;
		public VRPlayerCollisionManager collisionManager;
        public VRPlayerBodyEstimator playerBodyEstimator;
		VRPickup m_attachedObject;
		HashSet<VRPickup> pickupsInTrigger=new HashSet<VRPickup>();
		private string handDesc{get{return whichHand==WhichHand.Left?"Left":"Right";}}
		public Rigidbody rigidbodyToAttach{get{return m_rigidbody;}}
		Rigidbody m_rigidbody;
		void Awake(){
			m_rigidbody=GetComponent<Rigidbody>();
		}
		public bool RequestAttachToHand(VRPickup requster){
			if(attachedObject!=null){
				Debug.LogError(gameObject.name+": Error on attaching pickup to hand: unempty hand");
				return false;
			}
			m_attachedObject=requster;
			return true;
		}
		public bool RequestDetachFromHand(VRPickup requster){
			if(requster==null || requster!=m_attachedObject){
				Debug.LogError(gameObject.name+": Error on detaching pickup from hand: wrong or null pickup");
				return false;
			}
			m_attachedObject=null;
			return true;
		}
		public void AttachToHand(VRPickup pickup){
			if(isHolding){
				Debug.LogAssertion(gameObject.name+": Trying to attach pickup on an unempty hand");
				return;
			}
			pickup.AttachToHand(this);
		}
		public void DetachIfHolding(){
			if(attachedObject!=null)
				attachedObject.DetachFromHand(this);
		}
		void OnTriggerEnter(Collider other){
			VRPickup pickup=other.GetComponentInParent<VRPickup>();
			if(pickup){
				if(pickupsInTrigger.Contains(pickup)){
					Debug.LogError(gameObject.name+": An pickup enters the trigger twice: "+pickup);
					return;
				}
				pickupsInTrigger.Add(pickup);
			}
		}
		void OnTriggerExit(Collider other){
			VRPickup pickup=other.GetComponentInParent<VRPickup>();
			if(pickup){
				if(!pickupsInTrigger.Contains(pickup)){
					Debug.LogError(gameObject.name+": An unlisted pickup exit the trigger: "+pickup);
					return;
				}
				pickupsInTrigger.Remove(pickup);
			}
		}
		void Update(){
			if(!isHolding){
				if(Input.GetButtonDown(handDesc+"TriggerTouch")){}
				foreach(var h in pickupsInTrigger)
					if(!h.isHeld){
						switch(h.lockType){
							case VRPickup.LockType.Touch:
								AttachToHand(h);
								break;
							case VRPickup.LockType.TriggerToLock:
								if(Input.GetButtonDown(handDesc+"TriggerTouch"))
									AttachToHand(h);
								break;
						}
						if(isHolding)
							break;
					}
			}else{//isHelding==true
				switch(attachedObject.unlockType){
					case VRPickup.UnlockType.Holding:
						if(Input.GetButtonUp(handDesc+"TriggerTouch"))
							DetachIfHolding();
						break;
					case VRPickup.UnlockType.TriggerToUnlock:
						if(Input.GetButtonDown(handDesc+"TriggerTouch"))
							DetachIfHolding();
						break;
					case VRPickup.UnlockType.GripAndTriggerToUnlock:
						if(Input.GetAxis(handDesc+"GripSqueeze")>.1f && Input.GetButtonDown(handDesc+"TriggerTouch"))
							DetachIfHolding();
						break;
				}
			}
		}
	}
}
