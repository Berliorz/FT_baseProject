using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnGravityCube : MonoBehaviour
{
    [SerializeField]
    Vector3 localGravity;

    [SerializeField]
    float RandomForce = 5.0f;

    [SerializeField]
    float RandomForceProb = 30.0f;

    private Rigidbody m_RidgidBody;
    private Vector3 m_GrabityCenter;
    private float localsCale = 0.05f;

    private bool m_isTriggered = false;

    public void SetGrabityCenter(Vector3 center)
    {
        m_GrabityCenter = center;
    }

    public void Triggered()
    {
        m_isTriggered = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        m_RidgidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (m_isTriggered)
        {
            return;
        }

        Vector3 force = localGravity + (m_GrabityCenter - this.transform.position);
        float f = RandomForce;
        if (Random.Range(0, 100) < RandomForceProb)
        {
            force += new Vector3(Random.Range(-f, f), Random.Range(-f, f), Random.Range(-f, f));
        }

        m_RidgidBody.AddForce(force, ForceMode.Acceleration);
        Vector3 velo = m_RidgidBody.velocity;
        
        //if (velo.magnitude > MaxVelocity)
        //{
        //    m_RidgidBody.velocity = velo.normalized * MaxVelocity;
        //}
    }
    void OnTriggerEnter(Collider other)
    {   
        if (m_isTriggered)
        {
            return;
        }

        if (this.gameObject.GetComponent<UnGravityCube>() == null)
        {
            return;
        }

        UnGravityCube ugc = other.gameObject.GetComponent<UnGravityCube>();
        if (ugc != null)
        {
            ugc.Triggered();
            float scale = localsCale;
            other.gameObject.transform.parent = this.transform;
            Vector3 localPosition = other.gameObject.transform.localPosition;
            int fx = (int)Mathf.Floor(localPosition.x);
            int fy = (int)Mathf.Floor(localPosition.y);
            int fz = (int)Mathf.Floor(localPosition.z);

            if (Mathf.Abs(fx) + Mathf.Abs(fy) + Mathf.Abs(fz) > 2)
            {
                fx = 0;
            }

            localPosition.x = fx != 0 ? fx / Mathf.Abs(fx) : 0;
            localPosition.y = fy != 0 ? fy / Mathf.Abs(fy) : 0;
            localPosition.z = fz != 0 ? fz / Mathf.Abs(fz) : 0;
            other.gameObject.transform.localPosition = localPosition;

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null) {
                Destroy(rb);
            }
        }
        
    }

}
