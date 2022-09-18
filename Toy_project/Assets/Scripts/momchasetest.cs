using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class momchasetest : MonoBehaviour
{

    public NavMeshAgent enemy;
    public GameObject Player;

    public bool chasing = true;
	float chasertimer = 0;
	int MoveSpeed = 10;
    int MaxDist = 10;
    int MinDist = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 target = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
        transform.LookAt(target);
        if(chasing){
			if (Vector3.Distance(transform.position, Player.transform.position) >= MinDist)
			{
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
			}
		}

        /*
		if(chasing){
			if (Vector3.Distance(transform.position, playerpos) >= MinDist)
			{
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
			}
			chasertimer += Time.deltaTime;
				// Check if we have reached beyond 2 seconds.
				// Subtracting two is more accurate over time than resetting to zero.
			if(chasertimer > attentionTime)
			{
				chasing = false;
				wait = false;
				readyToStop = false;
				agent.speed = 50f;
				GotoNextPoint();
				chasertimer =  0;
			}
		}
        */
    }


}
