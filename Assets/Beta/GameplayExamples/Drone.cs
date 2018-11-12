using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018.Examples
{
    public class Drone : MonoBehaviour
    {
        public Transform target;
        public float sector = 30;
        public float h0 = 1f;
        public float h1 = 3f;
        public float r0=3f;
        public float r1 = 4f;
        public float t0 = 2f;
        public float t1 = 5f;
        public float kP = 4, kI = 2, kD = -1;
        public float acceleration = 15f;
        public float drag = 3f;
        public float noiseAcceleration=2f;
        public float noiseFreq = 0.5f;
        public float apporachingDestRange = 1f;
        public float collisionBack = 2f;
        Vector3 impulse=Vector3.zero;
        Rigidbody rb;

        Vector3 dest;

        bool moving = false;
        Vector3 GetNextDest()
        {
            Vector3 r = target.position + Random.Range(h0,h1)*transform.up;
            float ang = Random.Range(-sector, sector)*Mathf.Deg2Rad;
            r += (target.forward * Mathf.Cos(ang) + target.right * Mathf.Sin(ang)) * Random.Range(r0,r1);
            return r;
        }
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            if (target == null)
                target = GameObject.FindGameObjectWithTag("Player").transform;
            StartCoroutine(MainLoop());
            StartCoroutine(PIDLoop());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        IEnumerator MainLoop()
        {
            while (true)
            {
                dest=GetNextDest();
                while ((dest - transform.position).magnitude > apporachingDestRange)
                {
                    if (impulse.magnitude > 0)
                    {
                        dest = transform.position + impulse.normalized * collisionBack;
                        yield return new WaitForSeconds(0.25f);
                        dest = GetNextDest();
                    }
                    impulse = Vector3.zero;
                    yield return null;
                }
                yield return new WaitForSeconds(Random.Range(t0,t1));
            }
        }
        IEnumerator PIDLoop()
        {
            Vector3 i = Physics.gravity/kI;
            while (true)
            {
                Vector3 r = rb.position - dest;
                Vector3 v = rb.velocity;
                i += r * Time.deltaTime;
                // d^2x/dt^2+2*ksi*omega^2*dx/dt+x=0 ksi=1 critical
                Vector3 a = -kD*v - kP*r-kI*i;
                rb.AddForce(Vector3.ClampMagnitude(a, acceleration),ForceMode.Acceleration);
                float t = Time.time * noiseFreq;
                Vector3 noise = new Vector3(.2f*Mathf.PerlinNoise(t, 0f), Mathf.PerlinNoise(t, 1f), .2f*Mathf.PerlinNoise(t, 2f))*2-Vector3.one;
                rb.AddForce(noise * noiseAcceleration, ForceMode.Acceleration);
                rb.AddForce(-v*drag, ForceMode.Acceleration);
                yield return null;
               }
        }
        private void OnCollisionStay(Collision collision)
        {
            impulse += collision.impulse;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color=Color.red;
            Gizmos.DrawWireSphere(dest, .1f);
        }
    }
}