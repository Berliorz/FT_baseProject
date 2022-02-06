using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using System.IO;

namespace UnityEngine.XR.ARFoundation
{
    public class PointCloudDataPicker : MonoBehaviour
    {
        ARPointCloud m_PointCloud;
        SceneManager m_Scenemanager;

        List<Vector3> m_Positions;
        List<Vector3> m_Confidences;
        bool m_HasChanged = false;
        bool m_WhileUpdating = false;

        void Awake()
        {
            m_PointCloud = GetComponent<ARPointCloud>();

        }

        void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs)
        {
            if (!m_PointCloud.positions.HasValue)
                return;

            m_WhileUpdating = true;

            var positions = m_PointCloud.positions.Value;
            var confidences = m_PointCloud.confidenceValues.Value;

            if (positions.Length == confidences.Length)
            {
                m_Positions.Clear();
                for (int i = 0; i < positions.Length; ++i)
                {
                    m_Positions.Add(positions[i]);
                }
            }
            else
            {
                Debug.LogError("positions and confidences are not same length.");
            }


            m_HasChanged = true;
            m_WhileUpdating = false;
        }

        private void Update()
        {
            if (!m_WhileUpdating && m_HasChanged)
            {
                m_Scenemanager.PointPositions = new List<Vector3>(m_Positions);
                m_HasChanged = false;
            }
        }

        void OnEnable()
        {
            m_PointCloud.updated += OnPointCloudChanged;
        }

        void OnDisable()
        {
            m_PointCloud.updated -= OnPointCloudChanged;
        }
    }
}