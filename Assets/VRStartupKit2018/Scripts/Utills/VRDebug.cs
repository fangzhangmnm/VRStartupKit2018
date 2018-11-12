using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStartupKit2018
{
	public class VRDebug : MonoBehaviour {
		public static VRDebug instance;
		public TextMesh text;
		public GameObject SpherePrefab;
        public string str1;
        public string str2;
		void Start(){
            instance =this;
            str1 = str2 = "";
        }
		public static void Log(string s){
			if(!instance)return;
			instance.str1=s+'\n'+ instance.str1;
			if(instance.str1.Length > 2500)
				instance.str1 = instance.str1.Substring(0,2500);
        }
        public static void LogPersistence(string s)
        {
            if (!instance) return;
            instance.str2 = s + '\n' + instance.str2;
            if (instance.str2.Length > 2500)
                instance.str2 = instance.str2.Substring(0, 2500);
        }
        public static void ClearPersistence()
        {
            instance.str2 = "";
        }
        public static void ClearAll(){
			if(!instance)return;
            instance.str1 = "";
            instance.str2 = "";
			for(int i=0;i<instance.transform.childCount;++i)
				Destroy(instance.transform.GetChild(i).gameObject);
		}
		public void Update(){
			if(!instance)return;
            if (str1 != "")
                text.text = str1 + "\n" + str2;
            else
                text.text = str2;
            str1 = "";
		}
		public static void Sphere(string name,Vector3 pos,Quaternion rot,float radius){
			if(!instance)return;
			var h=Instantiate(instance.SpherePrefab,pos,rot,instance.transform);
			h.name=name;
			h.transform.localScale=Vector3.one*radius;
		}
		
	}
}