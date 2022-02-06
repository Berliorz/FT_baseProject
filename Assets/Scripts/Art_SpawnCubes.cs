using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Art_SpawnCubes : MonoBehaviour
{
    [SerializeField]
    float SpawnAreaRadius = 3;

    [SerializeField]
    float SpawnDuration = .5f;

    [SerializeField]
    int MaxObjectCount = 40;

    [SerializeField]
    GameObject SpawnObject;

    private List<GameObject> m_SpawnObjects = new List<GameObject>();
    private float m_Duration = -1;
    private Vector3 m_GrabityCenter;

    void Start()
    {
        Vector3 p = this.transform.position;
        m_Duration = SpawnDuration;
        m_GrabityCenter = new Vector3(p.x, p.y + .5f, p.z);
    }

    void Update()
    {
        if (m_Duration > 0)
        {
            m_Duration -= Time.deltaTime;
        }
        else
        {
            // ÉLÉÖÅ[Éuê∂ê¨         
            Vector3 spawn_pos = new Vector3(Random.Range(-SpawnAreaRadius, SpawnAreaRadius), 0, Random.Range(-SpawnAreaRadius, SpawnAreaRadius));
            GameObject spObject = Instantiate(SpawnObject, this.transform.position + spawn_pos, Quaternion.identity);
            spObject.GetComponent<UnGravityCube>().SetGrabityCenter(m_GrabityCenter);

            m_SpawnObjects.Add(spObject);

            if (m_SpawnObjects.Count > MaxObjectCount)
            {
                Destroy(m_SpawnObjects[0]);
                m_SpawnObjects.RemoveAt(0);
            }

            m_Duration = SpawnDuration;
        }
    }
}