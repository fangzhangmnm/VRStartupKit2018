using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018.Weaponry
{
    public class Projectile : MonoBehaviour
    {
        public float attackValue = 10f;
        public string[] excludeFraction;
        public GameObject sparkPrefab;
        private void OnCollisionEnter(Collision collision)
        {
            var other=collision.gameObject.GetComponentInParent<Damageable>();
            if(other)
                if (!((IList)excludeFraction).Contains(other.fraction))
                    other.TakeDamage(attackValue);
            if (sparkPrefab)
                Instantiate(sparkPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}