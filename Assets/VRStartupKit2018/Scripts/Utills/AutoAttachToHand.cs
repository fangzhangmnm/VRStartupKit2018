using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{
    [RequireComponent(typeof(VRPickup))]
    public class AutoAttachToHand : MonoBehaviour
    {
        VRPickup vp;
        public VRHand hand;
        void Start()
        {
            vp = GetComponent<VRPickup>();
            if (hand && hand.attachedObject == null)
                hand.AttachToHand(vp);
        }
        void Update()
        {
            if (hand && hand.attachedObject == null)
                hand.AttachToHand(vp);

        }
    }
}