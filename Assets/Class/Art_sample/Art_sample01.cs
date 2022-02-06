using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Art_sample01 : ArtBase
{
    [SerializeField]
    float Height;
    [SerializeField]
    float NoiseAmount;
    [SerializeField]
    float Delay;

    [SerializeField]
    List<GameObject> Cubes;

    Rigidbody m_FirstRigidBody;

    public Vector3 CenterPoint;
    // Start is called before the first frame update

    float m_DelayCount;
    List<Vector3> m_ToPosition;
    List<Vector3> m_FromPosition;

    void Start()
    {
        m_FirstRigidBody = Cubes[0].GetComponent<Rigidbody>();
        //m_DelayCount = new List<float>() { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        m_DelayCount = Delay;
        m_ToPosition = new List<Vector3>();
        m_FromPosition = new List<Vector3>();

        for (int i = 0; i < Cubes.Count; i ++) {
            m_ToPosition.Add(Vector3.zero);
            m_FromPosition.Add(Vector3.zero);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = CenterPoint - Cubes[0].transform.position;
        float x_noise = Random.Range(-NoiseAmount, NoiseAmount);
        float y_noise = Random.Range(-NoiseAmount, NoiseAmount)*10;
        float z_noise = Random.Range(-NoiseAmount, NoiseAmount)*10;
        Vector3 noized = new Vector3(diff.x + x_noise, diff.y + y_noise, diff.z + z_noise);
        m_FirstRigidBody.AddForce(noized.normalized);

        Cubes[0].transform.LookAt(CenterPoint);

        if (m_DelayCount > 0)
        {
            for (int i = 1; i < Cubes.Count; i++)
            {
                Vector3 to_pos = m_ToPosition[i];
                Vector3 from_pos = m_FromPosition[i];
                GameObject obj = Cubes[i];

                obj.transform.position = Vector3.Lerp(to_pos, from_pos,  m_DelayCount / Delay);
            }
            m_DelayCount -= Time.deltaTime;
        }
        else
        {
            for (int i = 1; i < Cubes.Count; i++)
            {
                GameObject target_obj = Cubes[i - 1];
                GameObject obj = Cubes[i];
                m_ToPosition[i] = target_obj.transform.position;
                m_FromPosition[i] = obj.transform.position;
            }
            m_DelayCount = Delay;
        }
    }

    public void RiftUpCenterPoint()
    {
        CenterPoint.y += Height;
    }
}
