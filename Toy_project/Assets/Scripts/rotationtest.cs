using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationtest : MonoBehaviour
{


	public float Speed = 1f;


	private Coroutine LookCoroutine;

    public float goalAngle = 90f;

	public void StartRotating(){
        
		if(LookCoroutine != null){
			StopCoroutine(LookCoroutine);
		}

		LookCoroutine = StartCoroutine(LookAt());
        
	}

    
    
	private IEnumerator LookAt(){
        

        Debug.Log("i'm looking :)");
		Quaternion lookRotation = Quaternion.Euler(0f, goalAngle, 0f);

		float time = 0;

		while(time < 1){

			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

			time += Time.deltaTime*Speed;

			yield return null;
		}
	}
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, goalAngle, 0f);
        
        goalAngle++;
        if(goalAngle > 360){
            goalAngle = 0;
        }
        //StartRotating();
    }
}
