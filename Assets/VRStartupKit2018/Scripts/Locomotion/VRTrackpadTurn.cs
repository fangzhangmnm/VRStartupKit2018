using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRStartupKit2018
{
	public class VRTrackpadTurn : MonoBehaviour {
		public Transform turnTarget;
		public float degree=45;
		public float trackpadHorizontalThreshold=.4f;
		public float trackpadVerticalThreshold=.4f;
        public VRHand.WhichHand trackpadHand = VRHand.WhichHand.Right;
		private bool turned=false;
        void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
            string handName = trackpadHand == VRHand.WhichHand.Left ? "Left" : "Right";
            if (Input.GetButtonUp(handName+"TrackpadPress")){
				float h=Input.GetAxis(handName+"TrackpadHorizontal");
				float v=Input.GetAxis(handName+"TrackpadVertical");
				if(Mathf.Abs(v)<trackpadVerticalThreshold){
					if(h>trackpadHorizontalThreshold)
						turnTarget.Rotate(turnTarget.up,degree);
					if(h<-trackpadHorizontalThreshold)
						turnTarget.Rotate(turnTarget.up,-degree);
				}
			}
			{//Windows MR adaptive
				float h=Input.GetAxis(handName+"ThumbstickHorizontal");
				float v=Input.GetAxis(handName+"ThumbstickVertical");
				if(Mathf.Abs(h)<=trackpadHorizontalThreshold){
					turned=false;
				}else if(Mathf.Abs(v)<trackpadVerticalThreshold &&!turned && !Input.GetButtonDown(handName+"TrackpadTouch")){
					if(h>trackpadHorizontalThreshold)
						turnTarget.Rotate(turnTarget.up,degree);
					if(h<-trackpadHorizontalThreshold)
						turnTarget.Rotate(turnTarget.up,-degree);
					turned=true;
				}
			}
			
		}
	}
}