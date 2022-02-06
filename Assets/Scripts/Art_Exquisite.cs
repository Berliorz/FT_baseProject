using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Art_Exquisite : ArtBase
{
    [SerializeField]
    float StartScale;

    [SerializeField]
    float StartHeight;

    [SerializeField]
    GameObject RootObject;

    [SerializeField]
    Art_Exquisite.Type PrimitiveType = Art_Exquisite.Type.Cube;

    [SerializeField]
    float StepDuration;

    private List<GameObject> m_Cubes;
    private Art_Exquisite.Phase m_Phase = Phase.Split;
    private int m_Step = 1;
    private float m_CurrentDuration;

    enum Phase
    {
        Split,
        Moving,
    }

    enum Type
    {
        Cube
    }


    // Start is called before the first frame update
    void Start()
    {
        m_Cubes = new List<GameObject>();

        Transform tf = this.transform;
        //this.gameObject.transform.localScale = new Vector3(this.StartScale, this.StartScale, this.StartScale);
        this.gameObject.transform.position = new Vector3(tf.position.x, tf.position.y + this.StartHeight, tf.position.z);

        // first generate
        GameObject[] first_object = CreateNextGenObjects(null, m_Step, StartScale);
        m_Cubes.Add(first_object[0]);
        m_Phase = Phase.Moving;
        m_CurrentDuration = StepDuration;
        m_Step++;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Phase == Phase.Split)
        {
            List<GameObject> next_gen_objects = new List<GameObject>();

            foreach (GameObject gObject in this.m_Cubes)
            {
                GameObject[] next_gen_objects_part = CreateNextGenObjects(gObject, m_Step, StartScale);

                next_gen_objects.AddRange(next_gen_objects_part);
            }

            m_Cubes.Clear();
            m_Cubes = next_gen_objects;

            m_Phase = Phase.Moving;
            m_Step++;
            m_CurrentDuration = StepDuration;

        }
        else if (m_Phase == Phase.Moving)
        {
            if (m_CurrentDuration < 0)
            {
                m_Phase = Phase.Split;                
            }
            else
            {
                m_CurrentDuration -= Time.deltaTime;
            }
        }
    }

    GameObject[] CreateNextGenObjects(GameObject currentObject, int step, float startScale)
    {
        GameObject[] next_gen_objects;

        if (step <= 1)
        {
            next_gen_objects = new GameObject[1];
            GameObject cube = Instantiate(RootObject);
            float s = startScale;

            cube.transform.parent = this.transform;
            cube.transform.localPosition = Vector3.zero;
            cube.transform.localScale = new Vector3(s, s, s);

            next_gen_objects[0] = cube;
        }
        else if (PrimitiveType == Type.Cube)
        {
            int count = 8;
            float move_distance = startScale / (step-1) / 2;
            float size = startScale / (step - 1) / 2;
            next_gen_objects = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                GameObject cube = Instantiate(RootObject);
                cube.transform.parent = this.transform;
                float x = currentObject.transform.localPosition.x + (i < 4 ? 1 : -1) * move_distance; ;//++++----
                float y = currentObject.transform.localPosition.y + (i/2 % 2 < 1 ? 1 : -1) * move_distance;//++--++--
                float z = currentObject.transform.localPosition.z + (i%2 < 1 ? 1 : -1) * move_distance;//+-+-+-+-

                cube.transform.localPosition = new Vector3(x, y, z);
                cube.transform.localScale = new Vector3(size, size, size);

                next_gen_objects[i] = cube;
            }

        }
        else
        {
            next_gen_objects = new GameObject[0];
        }

        return next_gen_objects;
    }
}

struct ArtDetail
{

}