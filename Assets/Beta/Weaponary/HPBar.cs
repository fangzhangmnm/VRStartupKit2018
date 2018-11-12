using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRStartupKit2018.Weaponry
{
    [ExecuteInEditMode]
    public class HPBar : MonoBehaviour
    {
        Transform mc;
        Scrollbar sb;
        Damageable db;
        // Use this for initialization
        void OnEnable()
        {
            mc = GameObject.FindGameObjectWithTag("MainCamera").transform;
            sb = GetComponentInChildren<Scrollbar>();
            db = GetComponentInParent<Damageable>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(mc, Vector3.up);
            var e = transform.eulerAngles;e.x = 0; transform.eulerAngles = e;
            sb.size = db.hp / db.maxHp;
        }
    }
}