using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
namespace VRStartupKit2018
{
    //TODO 实验检验
    public class VRPlayerBodyEstimator : MonoBehaviour
    {
        public bool isStance { get { return m_stance; } }
        public bool isCrouching { get { return m_crouching; } }
        public bool isJumping { get { return m_jumpStage>1; } }
        public float verticalSpeedTS { get { return evY; } }
        public float verticalAcceleratorTS { get { return eG; } }
        public float hmdHeightTS { get { return HMD.localPosition.y; } }
        public Transform HMD;
        public Transform Basis;
        public Transform player;
        public float hmdInitHeightTS=1.8f;
        public float stanceCriteria = 0.95f;
        public float crouchCriteria = 0.8f;
        public float scale { get { return HMD.lossyScale.y; } }
        bool m_stance, m_crouching;
        bool m_isStance;
        int m_jumpStage;
        float lastY, evY,lastvY,eG;
        const float lerpFactor = .5f;
        public Vector3 GetShoulderPositionWS()
        {
            return player.position + player.up * scale * hmdInitHeightTS * 0.85f;
        }
        public Vector3 GetCOMPositionWS()
        {
            return player.position + player.up * scale * hmdInitHeightTS * 0.5f;
        }
        float enterStanceCount, enterCrouchCount;
        void Start()
        {
            lastY = HMD.localPosition.y;
            evY = lastvY=0;
            eG = 0;
            enterStanceCount = enterCrouchCount = 0;
            m_stance = m_crouching = false;
            m_jumpStage = 0;
        }
        public void MeasurePlayerHeight()
        {
            hmdInitHeightTS = HMD.localPosition.y;
        }
        void Update()
        {
            {

                float Y = HMD.localPosition.y;
                float vy = (Y - lastY) / Time.deltaTime; lastY =Y;
                evY = Mathf.Lerp(evY, vy, lerpFactor);
                float g = (evY - lastvY) / Time.deltaTime;lastvY = evY;
                eG = Mathf.Lerp(eG, g, lerpFactor);
            }
            {
                switch (m_jumpStage)
                {
                    case 0:
                        if (hmdHeightTS > hmdInitHeightTS * .9f && eG > 0.5f && evY>0.5f)
                            m_jumpStage = 1;
                        break;
                    case 1:
                        if (hmdHeightTS > hmdInitHeightTS * 1.1f && eG < 0)
                            m_jumpStage = 2;
                        break;
                    case 2:
                        if (evY < 0 && eG < 0)
                            m_jumpStage = 3;
                        break;
                    case 3:
                        if (eG > 0 && hmdHeightTS< hmdInitHeightTS*1.1f)
                            m_jumpStage = 0;
                        break;
                }
            }
            if (HMD.localPosition.y < stanceCriteria * hmdInitHeightTS)
            {
                enterStanceCount += Time.deltaTime;
                if(enterStanceCount>.1f && !m_stance)
                {
                    m_stance = true;
                }
            }
            else
            {
                enterStanceCount = 0;
                m_stance = false;
            }

            if (HMD.localPosition.y < crouchCriteria * hmdInitHeightTS)
            {
                enterCrouchCount += Time.deltaTime;
                if (enterCrouchCount > .1f && !m_crouching)
                {
                    m_crouching = true;
                }
            }
            else
            {
                enterCrouchCount = 0;
                m_crouching = false;
            }
            VRDebug.Log(string.Format("height: {0:0.00}\tvy: {1:0.0}\tg:{2:0.0}",hmdHeightTS,verticalSpeedTS,verticalAcceleratorTS));
            VRDebug.Log("crouch: " + isCrouching + "\tstance: " + isStance + "\tjump: " + m_jumpStage+">1");
            
        }
    }
}
