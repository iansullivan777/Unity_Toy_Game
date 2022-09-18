using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomAnimationController : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("IsWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BeginWalking()
    {
        anim.SetBool("IsWalking", true);
    }

    public void BeginGrabbing()
    {
        anim.SetBool("IsGrabbing", true);
    }
    
    public void BeginIdle()
    {
        anim.SetBool("IsIdle", true);
    }
}
