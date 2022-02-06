using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Art_SprintMan : MonoBehaviour
{
    [SerializeField]
    GameObject SprintMan;

    private Animator m_SMAnimator;
    private int m_CurrentStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_SMAnimator = SprintMan.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_CurrentStep++;

            if (m_CurrentStep > 2)
            {
                m_SMAnimator.SetTrigger("run");
            }
            else if (m_CurrentStep > 0)
            {
                m_SMAnimator.SetTrigger("start");
            }
        }
    }
}
