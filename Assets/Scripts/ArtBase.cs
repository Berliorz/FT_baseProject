using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBase :MonoBehaviour
{
    protected SceneManager m_SceneManager;
    protected DebugLogger m_Dbg;

    void Awake()
    {
        m_SceneManager = GameObject.Find("AR Session Origin").GetComponent<SceneManager>();
        m_Dbg = GameObject.Find("AR Session Origin").GetComponent<DebugLogger>();
    }
}
