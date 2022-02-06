using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    private Vector3 m_ToPos;
    private Vector3 m_FromPos;
    private float m_Duration;
    private float m_DefaultDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Duration < 0)
        {
        }
        else
        {
            this.transform.localPosition = Vector3.Lerp(m_FromPos, m_ToPos, (m_DefaultDuration - m_Duration) / m_DefaultDuration);

            m_Duration -= Time.deltaTime;

        }
    }
    public void Move(float distance, float duration)
    {
        m_FromPos = this.transform.localPosition;
        m_ToPos = m_FromPos + NextPosition()*distance;

        m_Duration = duration;
        m_DefaultDuration = duration;
    }

    private Vector3 NextPosition()
    {
        // ‚Î‚ç‚¯‹ï‡‚ð‚±‚±‚ÅŒˆ‚ß‚éB
        Vector3 n_pos;
        float v = Random.value;
        float step = 0.08f;

        if (v < step)
        {
            n_pos = new Vector3(1, 0, 0);
        }
        else if (v < step*2)
        {
            n_pos = new Vector3(-1, 0, 0);
        }
        else if (v < step*3)
        {
            n_pos = new Vector3(0, 0, 1);
        }
        else if (v < step*4)
        {
            n_pos = new Vector3(0, 0, -1);
        }
        else
        {
            n_pos = new Vector3(0, 1, 0);
        }

        return n_pos;
    }
}