using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExcuiteCube : MonoBehaviour
{
    public enum TaskType
    {
        None,
        Rotate,
        Rotating,
        Shake,
        Shaking,
    }

    private List<TaskType> m_Tasks = new List<TaskType>();

    private float m_CurrentDuration;
    private float m_Duration;
    private TaskType m_CurrentTask = TaskType.None;

    private Quaternion m_FromQ;
    private Quaternion m_ToQ;

    private Vector3 m_FromPos;
    private Vector3 m_ToPos;

    private Vector3 m_Rand;

    private float RotateDuration = .8f;

    private float ShakeDuration = .8f;

    // Start is called before the first frame update
    void Start()
    {
        m_Rand = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        //this.PushTask(TaskType.Rotate);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_CurrentTask)
        {
            case TaskType.Rotate:
                m_FromQ = transform.rotation;
                m_ToQ = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 350, 0));
                m_CurrentTask = TaskType.Rotating;
                m_CurrentDuration = RotateDuration;
                m_Duration = RotateDuration;
                break;

            case TaskType.Rotating:
                m_CurrentDuration -= Time.deltaTime;
                if (m_CurrentDuration > 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0.5f, 0));
                }
                else
                {
                    m_CurrentTask = PopTask();
                }
                break;

            case TaskType.Shake:
                m_FromPos = transform.position;
                m_CurrentTask = TaskType.Shaking;
                m_CurrentDuration = ShakeDuration;
                m_Duration = ShakeDuration;
                break;

            case TaskType.Shaking:
                m_CurrentDuration -= Time.deltaTime;
                float duration_0_1 = m_CurrentDuration / m_Duration;
                float phase_1 = 0.8f;
                float phase_2 = 0.2f;
                float the_time = Time.time * 20.0f;
                float move_bias = 0.07f;

                if (duration_0_1 > phase_1)
                {
                    float n = 1.0f - phase_1;
                    float bias = (n - (duration_0_1 - phase_1)) / n;

                    Vector3 add = new Vector3(Mathf.Sin(the_time) * m_Rand.x, Mathf.Sin(the_time) * m_Rand.y, Mathf.Sin(the_time) * m_Rand.z);

                    transform.position = m_FromPos + add * bias * move_bias;
                }
                else if (duration_0_1 > phase_2)
                {
                    Vector3 add = new Vector3(Mathf.Sin(the_time)*m_Rand.x, Mathf.Sin(the_time)*m_Rand.y, Mathf.Sin(the_time)*m_Rand.z);
                    transform.position = m_FromPos + add * move_bias;
                }  
                else if (m_CurrentDuration > 0)
                {                    
                    float bias = duration_0_1 / phase_2;
                    Vector3 add = new Vector3(Mathf.Sin(the_time) * m_Rand.x, Mathf.Sin(the_time) * m_Rand.y, Mathf.Sin(the_time) * m_Rand.z);

                    transform.position = m_FromPos + add * bias * move_bias;
                }
                else
                {
                    transform.position = m_FromPos;
                    m_CurrentTask = PopTask();
                }
                
                break;

            case TaskType.None:
                m_CurrentTask = this.PopTask();
                break;
        }        
    }

    public void PushTask(TaskType task)
    {
        m_Tasks.Add(task);
    }

    private TaskType PopTask()
    {
        TaskType task = TaskType.None;

        if (m_Tasks.Count > 0)
        {
            task = m_Tasks[0];
            m_Tasks.RemoveAt(0);
        }

        return task;
    }
}
