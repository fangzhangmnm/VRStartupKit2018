using UnityEngine;
using System.Collections;
 
namespace VRStartupKit2018
{
    [ExecuteInEditMode]
    public class OverlapSphereVisualizer : MonoBehaviour {
    
        [SerializeField]
        float radius;
    
        bool contact;
    
        // Update is called once per frame
        void Update()
        {
            contact = false;
    
            foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
            {
                contact = true;
            }
        }
    
        void OnDrawGizmos()
        {
            Gizmos.color = contact ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}