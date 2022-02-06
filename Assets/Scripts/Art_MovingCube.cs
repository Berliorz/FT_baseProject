using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Art_MovingCube : ArtBase
{
    [SerializeField]
    float PhaseDuration;

    [SerializeField]
    float ObjectDistance;

    [SerializeField]
    GameObject SpawnObject;

    private Phase m_Phase;
    private float m_Duration;
    private List<GameObject> m_Objects;

    private Vector3 m_RotationStart;
    private Vector3 m_RotationEnd;

    enum Phase
    {
        Spawn,
        Move,
        StartRotation,
        Rotating,
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Phase = Phase.Spawn;
        m_Objects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Phase == Phase.Spawn)
        {
            GameObject next_object = Instantiate(SpawnObject);
            next_object.transform.parent = this.transform;
            next_object.transform.localPosition = Vector3.zero;

            m_Objects.Add(next_object);

            foreach (GameObject g in m_Objects)
            {
                g.GetComponent<MovingCube>().Move(ObjectDistance, PhaseDuration);
            }

            m_Duration = PhaseDuration;
            m_Phase = Phase.Move;

            Debug.Log("end spawn phase");
        }
        else if ( m_Phase == Phase.Move)
        {
            if (m_Duration > 0)
            {
                m_Duration -= Time.deltaTime;
            }
            else
            {
                m_Duration = PhaseDuration;
                m_Phase = NextPhase();

                Debug.Log("end move phase");
            }
        }
        else if (m_Phase == Phase.StartRotation)
        {
            m_RotationStart = this.transform.rotation.eulerAngles;
            m_RotationEnd = new Vector3(m_RotationStart.x, m_RotationStart.y + 90, m_RotationStart.z);

            m_Phase = Phase.Rotating;

            Debug.Log("end startRotation phase.");
        }
        else if (m_Phase == Phase.Rotating)
        {
            if (m_Duration > 0)
            {
                m_Duration -= Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(Vector3.Lerp(m_RotationStart, m_RotationEnd, (PhaseDuration - m_Duration) / PhaseDuration));
            }
            else
            {
                m_Phase = NextPhase();

                Debug.Log("end rotating phase.");
            }
        }
    }

    Phase NextPhase()
    {
        float value = Random.value;
        Phase next_phase = Phase.Spawn;

        if (value <= 0.1)
        {
            next_phase = Phase.StartRotation;
        }

        return next_phase;
    }
}
