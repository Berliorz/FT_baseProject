using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogger : MonoBehaviour
{
    [SerializeField]
    GameObject m_TextObj;

    Text m_TextArea;


    // Start is called before the first frame update
    void Start()
    {
        m_TextArea = m_TextObj.GetComponent<Text>();
        m_TextArea.text = "Start";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLine(string str)
    {
        m_TextArea.text = str + "\n" + m_TextArea.text;
    }
}
