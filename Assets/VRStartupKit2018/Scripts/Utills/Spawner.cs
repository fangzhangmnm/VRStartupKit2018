using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018.Utility
{
    public class Spawner : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }
        public void Spawn(GameObject o)
        {
            Instantiate(o, transform.position, transform.rotation);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}