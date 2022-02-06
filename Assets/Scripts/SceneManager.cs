using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_CurrentArtPrefab;
    
    [SerializeField]
    int m_CurrentArtItr = 0;

    ARPlaneManager m_PlaneManager;
    ARRaycastManager m_RaycastManager;
    ARPointCloudManager m_PointCloudManager;

    
    public List<Vector3> PointPositions = new List<Vector3>();
    public List<ARPlane> Planes = new List<ARPlane>();

    float maxRayDistance = 30.0f;

    List<ARRaycastHit> m_HitList = new List<ARRaycastHit>();

    DebugLogger m_DbgL;

    // Start is called before the first frame update
    void Start()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_PointCloudManager = GetComponent<ARPointCloudManager>();

        m_PlaneManager.planesChanged += this.UpdatedPlaneInfo;

        m_DbgL = GetComponent<DebugLogger>();
    }

    private void OnDestroy()
    {
        m_PlaneManager.planesChanged -= this.UpdatedPlaneInfo;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        TouchInUnityEditor();
#endif

#if UNITY_ANDROID
        TouchInAndroid();
#endif
    }

    private void TouchInUnityEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Rayが衝突したかどうか
            if (Physics.Raycast(mouseRay, out hit, 1000.0f, Physics.AllLayers))
            {
                GameObject hitGameObject =  hit.collider.gameObject;
                ARPlaneArtComponent art_comp = hitGameObject.GetComponent<ARPlaneArtComponent>();

                if (art_comp.currentArt == null)
                {
                    Vector3 hitPose = hit.point;  //RayとARPlaneが衝突しところのPose
                    GameObject instanted_art = Instantiate(m_CurrentArtPrefab[m_CurrentArtItr], hitPose, Quaternion.identity);  //オブジェクトの生成
                    art_comp.currentArt = instanted_art;
                }
            }
        }
    }

    private void TouchInAndroid()
{
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)  //画面に指が触れた時に処理する
            {
                if (m_RaycastManager.Raycast(touch.position, m_HitList, TrackableType.PlaneWithinPolygon))
                {
                    // HitしたARPlaneを取得する。ARTrackableはGameObjectを継承しているので、GetComponentが叩ける。
                    ARTrackable trackable = m_HitList[0].trackable;
                    // PrefabのAR Default Planeにつけた、ARPlaneArtComponentを取得する。
                    ARPlaneArtComponent art_comp = trackable.GetComponent<ARPlaneArtComponent>();

                    if (art_comp.currentArt == null)
                    {
                        Pose hitPose = m_HitList[0].pose;  //RayとARPlaneが衝突しところのPose
                        GameObject instanted_art = Instantiate(m_CurrentArtPrefab[m_CurrentArtItr], hitPose.position, hitPose.rotation);  //オブジェクトの生成
                        art_comp.currentArt = instanted_art;
                    }
                }
            }
        }
    }

    public void UpdatedPlaneInfo(ARPlanesChangedEventArgs updatePlanes)
    {
        foreach(ARPlane rp in  updatePlanes.removed)
        {
            this.Planes.Remove(rp);
        }

        foreach(ARPlane ap in updatePlanes.added)
        {
            this.Planes.Add(ap);
        }


        foreach (ARPlane ep in updatePlanes.updated)
        {
            ARPlaneArtComponent art_comp = ep.gameObject.GetComponent<ARPlaneArtComponent>();

            if (art_comp.currentArt != null)
            {
                Art_sample01 art_comp_main = art_comp.currentArt.GetComponent<Art_sample01>();
                art_comp_main.CenterPoint = ep.center;
                art_comp_main.RiftUpCenterPoint();
            }
        }
    }

    public void ChangeCurrentArt()
    {
        m_CurrentArtItr++;

        if (m_CurrentArtItr >= m_CurrentArtPrefab.Count) 
        {
            m_CurrentArtItr = 0;
        }
    }
}
