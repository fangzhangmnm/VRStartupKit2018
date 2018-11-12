using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018.Weaponry
{
    [RequireComponent(typeof(VRPickup))]
    [RequireComponent(typeof(Rigidbody))]
    public class BulletGun : MonoBehaviour
    {
        VRPickup vrPickup;
        bool isWorking { get { return isActiveAndEnabled && attachedHand != null; } }
        public VRHand attachedHand { get { return vrPickup ? vrPickup.attachedHand : null; } }
        public Transform muzzle;
        public Collider muzzleCollider;
        public GameObject projectile;
        public float shootingVelocity = 10f;
        public float shootingInaccuracy = 0.02f;

        void OnEnable()
        {
            vrPickup = GetComponent<VRPickup>();
        }
        void Shoot()
        {
            var p = Instantiate(projectile, muzzle.position, transform.rotation);
            if (muzzleCollider)
                Physics.IgnoreCollision(p.GetComponent<Collider>(), muzzleCollider);
            Vector3 shootingVector = muzzle.forward* shootingVelocity;
            shootingVector = shootingVector + Random.insideUnitSphere * shootingVelocity * shootingInaccuracy;
            p.GetComponent<Rigidbody>().AddForce(shootingVector, ForceMode.VelocityChange);

        }
        void Update()
        {
            if (!isWorking) return;
            string handName = attachedHand.whichHand == VRHand.WhichHand.Left ? "Left" : "Right";
            if (Input.GetButtonDown(handName + "TriggerTouch"))
                Shoot();
        }
    }
}
