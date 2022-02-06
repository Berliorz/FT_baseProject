using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class Constants
{
    public const int SEPARATE_COUNT = 32;
}

enum Phase
{
    Initializing,
}

public class Art_Ecquisite2 : ArtBase
{
    [SerializeField]
    Vector3 CubeCenter = new Vector3(0, 1.8f, 0);

    [SerializeField]
    GameObject BaseCube;

    private float m_ScaleEnableDist = 3.0f;
    private float m_ScaleMinDist = 1.0f;
    private float m_CenterCubeScale = 0.25f;
    private Camera m_Camera;

    private GameObject[] m_Cubes_div1 = new GameObject[1];
    private GameObject[] m_Cubes_div4 = new GameObject[4*4*4 - 2*2*2];
    private GameObject[] m_Cubes_div10 = new GameObject[10*10*10 - 8*8*8];

    private int[] m_DivCount = new int[] { 1, 4, 10 };

    private List<GameObject[]> m_Cubes = new List<GameObject[]>();

    private DebugLogger logger;

    // Start is called before the first frame update
    void Start()
    {
        m_Cubes.Add(m_Cubes_div1);
        m_Cubes.Add(m_Cubes_div4);
        m_Cubes.Add(m_Cubes_div10);

        logger = GetComponent<DebugLogger>();

        m_Camera = GameObject.Find("AR Camera").GetComponent<Camera>();

        // Center Cube
        GameObject n_Cube = Instantiate(BaseCube);
        n_Cube.transform.position = transform.position + CubeCenter;
        n_Cube.transform.localScale = new Vector3(m_CenterCubeScale, m_CenterCubeScale, m_CenterCubeScale);
        m_Cubes[0][0] = n_Cube;

        //--------------------------------------------------------------------
        float base_scale = n_Cube.transform.localScale.x;
        Vector3 center = n_Cube.transform.position;
        int low = -m_DivCount[1] / 2;
        int max = m_DivCount[1] / 2;
        int count = 0;
        for (int i = low; i < max; i++)
        {
            for (int j = low; j < max; j++)
            {
                for (int k = low; k < max; k++)
                {
                    if (i == low || j == low || k == low ||
                        i == max-1 || j == max-1 || k == max - 1)
                    {
                        GameObject g = Instantiate(BaseCube);
                        g.transform.position = new Vector3((i+0.5f) * base_scale / 2 + center.x, (j + 0.5f) * base_scale / 2 + center.y, (k + 0.5f) * base_scale / 2 + center.z);
                        g.transform.localScale = new Vector3(base_scale / 2, base_scale / 2, base_scale / 2);
                        m_Cubes[1][count] = g;
                        count++;
                    }
                }
            }
        }

        //         ----------------------------------------------------------
        low = -m_DivCount[2] / 2;
        max = m_DivCount[2] / 2;
        count = 0;
        for (int i = low; i < max; i++)
        {
            for (int j = low; j < max; j++)
            {
                for (int k = low; k < max; k++)
                {
                    if (i == low || j == low || k == low ||
                        i == max - 1 || j == max - 1 || k == max - 1)
                    {
                        GameObject g = Instantiate(BaseCube);
                        g.transform.position = new Vector3((i + 0.5f) * base_scale / 4 + center.x, (j + 0.5f) * base_scale / 4 + center.y, (k + 0.5f) * base_scale / 4 + center.z);
                        g.transform.localScale = new Vector3(base_scale, base_scale, base_scale) / 4;
                        m_Cubes[2][count] = g;
                        count++;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Camera == null)
        {
            return;
        }

        Vector3 cam_pos = m_Camera.transform.position;

        for (int i = 0; i < m_Cubes.Count; i++)
        {
            GameObject[] objects = m_Cubes[i];
            for (int j = 0; j <objects.Length; j++)
            {
                GameObject cube = objects[j];
                Vector3 cube_pos = cube.transform.position;
                float dist = Vector3.Magnitude(cube_pos - cam_pos);
                float scale = dist > m_ScaleEnableDist ? 1.0f : (dist - m_ScaleMinDist) / (m_ScaleEnableDist - m_ScaleMinDist);
                scale = dist < m_ScaleMinDist ? 0 : scale;
                cube.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}

