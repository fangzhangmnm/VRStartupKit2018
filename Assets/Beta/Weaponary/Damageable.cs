using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRStartupKit2018.Weaponry
{
    public class Damageable : MonoBehaviour
    {
        public string fraction = "Neutral";
        public float maxHp=100;
        public float hp = 100;
        public MonoBehaviour[] ais;
        public UnityEvent onTakenDamage;
        public UnityEvent onDeath;

        public void TakeDamage(float amount)
        {
            hp -= amount;
            onTakenDamage.Invoke();
            if (hp <= 0)
            {
                hp = 0;
                Die();
            }
        }
        void Die()
        {
            onDeath.Invoke();
            foreach(var ai in ais)
            {
                ai.enabled = false;
            }
            this.enabled = false;
        }
    }
}
