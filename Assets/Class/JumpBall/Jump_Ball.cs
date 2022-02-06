using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Ball : MonoBehaviour
{

    private float m_CurrentDuration = 0;
    private float m_Duration = 1.0f;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;

        m_Animator.SetFloat("vertical_acceleration", m_Rigidbody.velocity.y);
        Debug.Log(m_Rigidbody.velocity.y);

        if (m_CurrentDuration > 0)
        {
            m_CurrentDuration -= Time.deltaTime;
        } 
        else
        {
            m_Animator.SetTrigger("jump");
        }
    }

    public void StartJump()
    {
        m_Rigidbody.AddForce(new Vector3(Random.RandomRange(-50, 50), 500, Random.RandomRange(-50, 50)));
    }

    public void EndMotion()
    {
        m_Animator.SetTrigger("end_landing");
        m_CurrentDuration = m_Duration;
    }
}
