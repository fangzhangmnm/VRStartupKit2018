using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{
    [RequireComponent(typeof(VelocityEstimator))]
    public class VelocityMeasurement : MonoBehaviour
    {
        public float maxSpeed;
        public float maxAngularSpeed;
        VelocityEstimator ve;

        void Start()
        {
            StartCoroutine(ClearRecord());
            ve = GetComponent<VelocityEstimator>();
        }
        IEnumerator ClearRecord()
        {
            while (true)
            {
                maxSpeed = maxAngularSpeed = 0;
                yield return new WaitForSeconds(1);
            }
        }
        void Update()
        {
            float speed = ve.estimatedVelocity.magnitude;
            float angularSpeed = ve.estimatedAngularVelocity.magnitude;
            maxSpeed = Mathf.Max(maxSpeed, speed);
            maxAngularSpeed = Mathf.Max(maxAngularSpeed, angularSpeed);
            VRDebug.Log(string.Format("{0}\tspeed: {1:0.0}\tangspeed: {2:0.0}", name, maxSpeed, maxAngularSpeed));
        }
    }
}
