using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace FrameworkDesign.Example
{
   
    public class PlaceManager : MonoBehaviour
    {
        [Header("AR Foundation")]  
        /// <summary>
        /// The active ARRaycastManager used in the example.
        /// </summary>
        public ARRaycastManager m_RaycastManager;


        [Header("UI")]
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;//Ҫ���õ�Ԥ�Ƽ�

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        [HideInInspector]
        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();//��ż�⵽����ײ��
        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }

        void Awake()
        {
            // m_RaycastManager = GetComponent<ARRaycastManager>();//Ҳ����ͨ��GetComponent��ȡ��ARRaycastManager
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        void Update()
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            var touch = Input.GetTouch(0);
            const TrackableType trackableTypes =
            TrackableType.FeaturePoint |
            TrackableType.PlaneWithinPolygon;

            if (Input.touchCount == 1 && touch.phase == TouchPhase.Moved)//�ƶ��ѷ��õĶ���
            {

                if (m_RaycastManager.Raycast(touchPosition, s_Hits, trackableTypes))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;
                    if (spawnedObject != null)
                    {
                        spawnedObject.transform.position = hitPose.position;
                    }                    
                }
            }
            if (Input.touchCount == 1 && touch.phase == TouchPhase.Began)//���touch begin����touch begin����������ײ���
            {
                //---�ж��Ƿ�touch��UI���----
                //#if IPHONE || ANDROID
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                //#else
                // if (EventSystem.current.IsPointerOverGameObject())
                //#endif
                //Debug.Log("��ǰ������UI��");
                {
                    Logger.Log($"��ǰ������UI��"+ touch.phase);
                    return;
                }
                else
                {
                    //Debug.Log("��ǰû�д�����UI��");
                    Logger.Log($"��ǰû�д�����UI��"+ touch.phase);
                }

                if (m_RaycastManager.Raycast(touchPosition, s_Hits, trackableTypes))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;
                    if (spawnedObject == null)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);//ʵ����Ԥ�Ƽ�����
                    }
                    else
                    {
                        spawnedObject.transform.position = hitPose.position;//���¶���״̬
                    }
                }
            }               
        }       
    }
}
