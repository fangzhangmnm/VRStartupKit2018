using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018.Examples
{
    public class Shooter : MonoBehaviour
    {
        public float shootingVelocity = 10f;
        public float turnTime = 0.5f;
        public float t00 = 4,t01=6, t1 =0.5f;
        public float recoli = 3f;
        public Transform target;
        public Transform muzzle;
        public Collider muzzleCollider;
        public GameObject projectile;
        public float shootingInaccuracy = 0.02f;
        Quaternion look;
        Vector3 shootingVector;
        bool preparingShooting;
        Rigidbody rb;

        public static Vector3 CalculateShootingVelocity(Vector3 r,float v,float g)
        {
            float h = r.y;
            Vector3 rp = Vector3.ProjectOnPlane(r, Vector3.up);
            float x = rp.magnitude;
            float A = g * g / 4;
            float B = g * h - v * v;
            float C = x * x;
            if (B * B - 4 * A * C < 0)
                return Vector3.zero;
            float tt = (-B - Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
            float cost = x / v / Mathf.Sqrt(tt);
            float sint = (h + g * tt / 2) / v / Mathf.Sqrt(tt);
            return (rp.normalized * cost + Vector3.up * sint)*v;
        }
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            if (target == null)
                target = GameObject.FindGameObjectWithTag("Player").transform;
            StartCoroutine(MainLoop());
            StartCoroutine(LookAt());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        IEnumerator MainLoop()
        {
            look = transform.rotation;
            preparingShooting = false;

            while (true)
            {
                yield return new WaitForSeconds(Random.Range(t00,t01));
                preparingShooting = true;
                yield return new WaitForSeconds(t1);
                if (shootingVector.magnitude>0 && projectile) {
                    var p = Instantiate(projectile, muzzle.position, muzzle.rotation);
                    if(muzzleCollider)
                        Physics.IgnoreCollision(p.GetComponent<Collider>(), muzzleCollider);
                    shootingVector = shootingVector + Random.insideUnitSphere * shootingVelocity*shootingInaccuracy;
                    p.GetComponent<Rigidbody>().AddForce(shootingVector, ForceMode.VelocityChange);
                    rb.AddForce(-shootingVector.normalized * recoli, ForceMode.VelocityChange);
                }
            }
        }
        IEnumerator LookAt()
        {
            while (true)
            {
                Vector3 targetPos = target.position;
                if (target.GetComponent<Collider>())
                    targetPos = target.GetComponent<Collider>().bounds.center;
                shootingVector = CalculateShootingVelocity(targetPos - transform.position, shootingVelocity, Physics.gravity.magnitude);
                if (shootingVector.magnitude > 0 && preparingShooting)
                    look = Quaternion.LookRotation(shootingVector, Vector3.up);
                else if((target.position - transform.position).magnitude>0)
                    look = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime / turnTime);
                yield return null;
            }
        }
    }
}