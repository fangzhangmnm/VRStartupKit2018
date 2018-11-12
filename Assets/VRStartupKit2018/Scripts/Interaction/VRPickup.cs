using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{
    [RequireComponent(typeof(Rigidbody))]
	public class VRPickup : MonoBehaviour {
		public Transform handle;
		public bool alignRotation=true;
		public bool alignPosition=true;
		public enum LockType{None,Touch,TriggerToLock}
		public enum UnlockType{None,Holding,TriggerToUnlock,GripAndTriggerToUnlock};
		public enum LockPolicy{Transform,Joint};
		public LockType lockType=LockType.TriggerToLock;
		public UnlockType unlockType=UnlockType.TriggerToUnlock;
		public LockPolicy lockPolicy=LockPolicy.Transform;
		public bool isHeld{get{return m_attachedHand;}}
		public VRHand attachedHand{get{return m_attachedHand;}}
		private VRHand m_attachedHand;
		Rigidbody rigid;
		FixedJoint joint;
		bool oldIsKinematic;
		void Start()
		{
			m_attachedHand=null;
			rigid=GetComponent<Rigidbody>();
			if(!handle)
				handle=this.transform;
		}
		public void AttachToHand(VRHand hand){
			if(isHeld){
				Debug.LogAssertion(gameObject.name+": Trying to attach an already held pickup");
				return;
			}
			if(hand.attachedObject!=null){
				Debug.LogAssertion(gameObject.name+": Trying to attach pickup to an unempty hand "+hand.gameObject.name);
				return;
			}
			if(!hand.RequestAttachToHand(this)){
				Debug.LogError(gameObject.name+": Error during attaching pickup to hand");
				return;
			}
			m_attachedHand=hand;
			if(alignRotation){
				Vector3 handLocal=transform.InverseTransformPoint(hand.transform.position);
				transform.rotation=hand.transform.rotation*Quaternion.Inverse(handle.localRotation);
				transform.Translate(hand.transform.position-transform.TransformPoint(handLocal),Space.World);
			}
			if(alignPosition){
				transform.Translate(hand.transform.position-handle.position,Space.World);
			}
			if(rigid){
				oldIsKinematic=rigid.isKinematic;
				if(!rigid.isKinematic && !GetComponent<VelocityEstimator>())
					gameObject.AddComponent<VelocityEstimator>();
			}
			switch(lockPolicy){
				case LockPolicy.Transform:
					if(rigid){
						rigid.isKinematic=true;
					}
					transform.parent=hand.attachedObjects;
					break;
				case LockPolicy.Joint:
					if(rigid){
						rigid.isKinematic=false;
					}
					joint=handle.gameObject.AddComponent<FixedJoint>();
					joint.connectedBody=hand.rigidbodyToAttach;
					joint.anchor=hand.transform.position;
					
					break;
			}
			foreach(var h in GetComponentsInChildren<Collider>())
				if(!h.isTrigger)
					hand.collisionManager.RegisterCollider(h);
			gameObject.BroadcastMessage("OnAttachToHand",hand,SendMessageOptions.DontRequireReceiver);
		}
		public void DetachFromHand(VRHand hand,Transform newParent=null){
			if(!isHeld){
				Debug.LogAssertion(gameObject.name+": Trying to detach a deteached pickup");
				return;
			}
			if(hand!=attachedHand || hand==null){
				Debug.LogAssertion(gameObject.name+": Trying to detach pickup from a wrong or null hand");
				return;
			}
			if(!hand.RequestDetachFromHand(this)){
				Debug.LogError(gameObject.name+": Error during detaching pickup from hand");
				return;
			}
			switch(lockPolicy){
				case LockPolicy.Transform:
					transform.parent=newParent;
					break;
				case LockPolicy.Joint:
					Destroy(joint);
					joint=null;
					break;
			}
			if(rigid){
				rigid.isKinematic=oldIsKinematic;
				VelocityEstimator ve=GetComponent<VelocityEstimator>();
				if(ve){
					rigid.velocity=ve.estimatedVelocity;
					rigid.angularVelocity=ve.estimatedAngularVelocity;
				}
			}
			foreach(var h in GetComponentsInChildren<Collider>())
				if(!h.isTrigger)
					hand.collisionManager.UnregisterCollider(h);
			m_attachedHand=null;
			gameObject.BroadcastMessage("OnDetachFromHand",hand,SendMessageOptions.DontRequireReceiver);
		}
		public void DetachIfHeld(Transform newParent=null){
			if(isHeld)
				DetachFromHand(attachedHand,newParent);
		}
		
	}
}