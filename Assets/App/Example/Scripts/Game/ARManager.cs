using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace FrameworkDesign.Example
{

    public class ARManager : MonoBehaviour
    {
        [Header("AR Foundation")]
        /// <summary>
        /// The active ARRaycastManager used in the example.
        /// </summary>
        public ARPlaneManager m_ARPlaneManager;

        [HideInInspector]

        /// <summary>
        /// ��ǰʶ�����ƽ��
        /// </summary>
        List<ARPlane> detectPlanes = new List<ARPlane>();

        /// <summary>
        /// ��ǰ�Ƿ�Ҫ��ʾƽ��
        /// </summary>
        bool isShowPlane = true;

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            //m_ARPlaneManager = FindObjectOfType<ARPlaneManager>();
        }

        void Start()
        {
            CheckDevice();

            m_ARPlaneManager.planesChanged += OnPlaneChanged;
        }

        private void Update()
        {
            SaveElePolicy();
        }

        void OnDisable()
        {
            m_ARPlaneManager.planesChanged -= OnPlaneChanged;
        }

        #endregion

        // ���������ƽ����
        // ����Ĭ�����ã�����ʱһֱ��ͣ�ؼ��ƽ�档�ر�ʱ�򲻻��ټ����ƽ���ˡ�
        public void DetectionPlane(bool value)
        {
            m_ARPlaneManager.enabled = value;
            if (m_ARPlaneManager.enabled)
            {
                print("������ƽ����");
            }
            else
            {
                print("�ѽ���ƽ����");
            }
        }

        // ��ʾ�����ؼ�⵽��ƽ��  
        public void SwitchPlane()
        {
            isShowPlane = !isShowPlane;
            Logger.Log($"SwitchPlane");
            for (int i = detectPlanes.Count - 1; i >= 0; i--)
            {
                if (detectPlanes[i] == null || detectPlanes[i].gameObject == null)
                    detectPlanes.Remove(detectPlanes[i]);
                else
                    detectPlanes[i].gameObject.SetActive(isShowPlane);
            }
        }

        /// <summary>
        /// �õ���ǰAR�Ự�Ƿ��������У���������(�������豸�ܹ�ȷ�����������ϵ�λ�úͷ���)��
        /// </summary>
        public bool Skode_IsTracking()
        {
            bool isTracking = false;

            if (ARSession.state == ARSessionState.SessionTracking)
            {
                isTracking = true;
            }

            return isTracking;
        }


        //��ARFoundation�·���ƽ��ʱ����ƽ����ӽ��б���������ǿ�����Щƽ��
        void OnPlaneChanged(ARPlanesChangedEventArgs arg)
        {
            for (int i = 0; i < arg.added.Count; i++)
            {
                detectPlanes.Add(arg.added[i]);
                arg.added[i].gameObject.SetActive(isShowPlane);
            }
        }

        //����豸���л���
        void CheckDevice()
        {
            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                ShowAndroidToastMessage("AR is supported, but requires an additional install. .");
                Invoke("Quit", 1);
            }
            else if (ARSession.state == ARSessionState.Ready)
            {
                Debug.Log("AR is supported and ready.");
            }
            else if (ARSession.state == ARSessionState.Unsupported)
            {
                ShowAndroidToastMessage("AR is not supported on the current device.");
                Invoke("Quit", 1);
            }
        }

        void ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        /// һ��ʡ�����ã����豸û�ҵ�ʶ��Ŀ�꣬������Ļ����󼤻�һ��ʱ���䰵
        /// </summary>
        void SaveElePolicy()
        {
            if (ARSession.state != ARSessionState.SessionTracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
        }
    }
}