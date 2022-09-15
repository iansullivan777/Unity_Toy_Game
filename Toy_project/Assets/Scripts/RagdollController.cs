using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RagdollController : MonoBehaviour
{
    GameObject hips;
    Collider mainCollider;
    Rigidbody mainRigidbody;
    Animator anim;
    List<Rigidbody> rigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        mainCollider = this.gameObject.GetComponent<Collider>();
        mainRigidbody = this.gameObject.GetComponent<Rigidbody>();
        anim = this.gameObject.GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        hips = GameObject.Find("Hips");
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableRagdoll(bool enable)
    {
        if (enable == false) {
            transform.transform.position = hips.transform.position;

        }

        for (int i = 0; i < rigidbodies.Count; i++)
        {
            rigidbodies[i].isKinematic = !enable;
        }
        if (mainCollider != null)
        {
            mainCollider.enabled = !enable;
        }
        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = enable;
        }
        if (anim != null)
        {
            anim.enabled = !enable;
        }
    }
}