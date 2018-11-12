using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
namespace VRStartupKit2018
{
	public class VRTeleport : MonoBehaviour {
		public Transform teleportTarget;
		public TrackedPoseDriver Left;
		public LineRenderer Line;
		public Transform TargetCircle;
		//public float targetCircleRadius=.3f;
		public float velo=10f;
		public float grav=10f;
		public float dt=0.1f;
		public int seg=40;
		public LayerMask teleportLayer;
		public float surfaceNormalThreshold=0.9f;
		public Color good;
		public Color bad;
		public float trackpadHorizontalThreshold=.3f;
		public float trackpadVerticalThreshold=1f;
		void Awake () {
			Line.gameObject.SetActive(false);
			Line.positionCount=seg;
			TargetCircle.gameObject.SetActive(false);
		}
		void Start(){
			/*
			TargetCircle.positionCount=21;
			for(int i=0;i<21;++i){
				TargetCircle.SetPosition(i,new Vector3(targetCircleRadius*Mathf.Cos(2*Mathf.PI*i/20),0,targetCircleRadius*Mathf.Sin(2*Mathf.PI*i/20)));
			}
			*/
		}
		// Update is called once per frame
		void Update () {
			{
				float lth=Input.GetAxis("LeftTrackpadHorizontal");
				float ltv=Input.GetAxis("LeftTrackpadVertical");
				if(Mathf.Abs(lth)>trackpadHorizontalThreshold||Mathf.Abs(ltv)>trackpadVerticalThreshold){
					Line.gameObject.SetActive(false);
					TargetCircle.gameObject.SetActive(false);
					return;
				}
			}
			if(Input.GetButtonUp("LeftTrackpadPress")  && TargetCircle.gameObject.activeInHierarchy){
				teleportTarget.position=TargetCircle.transform.position;
			}
			if(Input.GetButton("LeftTrackpadPress") ){
				Line.gameObject.SetActive(true);
				Vector3 x=Left.transform.position;
				Vector3 v=Left.transform.forward*velo;
				Vector3 g=Vector3.down*grav;
				Line.SetPosition(0,x);
				Vector3 hitPosition=transform.position;
				bool hitted=false;
				RaycastHit hit=new RaycastHit();
				for(int i =1;i<seg;++i){
					Ray ray=new Ray(x,v);
					if(Physics.Raycast(ray,out hit,dt*v.magnitude*1.01f,teleportLayer)){
						x=hit.point;
						hitted=true;
						hitPosition=x;
						Line.positionCount=i+1;
						Line.SetPosition(i,x);
						break;
					}else{
						x+=v*dt;
						v+=g*dt;
						Line.positionCount=i+1;
						Line.SetPosition(i,x);
					}
				}
				if(hitted && hit.normal.y>surfaceNormalThreshold){
					Line.startColor=Line.endColor=good;
					TargetCircle.gameObject.SetActive(true);
					TargetCircle.transform.position=hitPosition;
				}else{
					Line.startColor=Line.endColor=bad;
					TargetCircle.gameObject.SetActive(false);
				}

			}else{
				Line.gameObject.SetActive(false);
				TargetCircle.gameObject.SetActive(false);
			}
		}
	}
}