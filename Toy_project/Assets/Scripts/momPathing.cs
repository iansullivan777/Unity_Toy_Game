using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class momPathing : MonoBehaviour
{
    // Start is called before the first frame update
	
	private float waitTime = 3.0f;
    private float timer = 0.0f;
	bool readyToStop = false;
	bool wait = false;

	
	bool direction = true;
	int coinflip;
	public Transform player;
	public float playerDistance;
	
	public Transform[] navPoint;
	public UnityEngine.AI.NavMeshAgent agent;
	public int destPoint = 0;
	int lastDest;
	
	void Start () {
		
		UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = navPoint[0].position; 
		agent.speed = 100;
		
		//readyToStop = false;
	}
	
	void Update () {

		
		if(wait){
			
			timer += Time.deltaTime;
			// Check if we have reached beyond 2 seconds.
			// Subtracting two is more accurate over time than resetting to zero.
			if(timer > waitTime)
			{
				wait = false;
				readyToStop = false;
				agent.speed = 100;
				GotoNextPoint();
				timer =  0;
			}
			
		}
		
		if(!wait){
			
			if(agent.remainingDistance < 0.5f){
				if(readyToStop){
					wait = true;
					agent.speed = 0;
				}
				if(!readyToStop){
					GotoNextPoint();
				}
				
			}
			
		}
		
		
	}
	
	void GotoNextPoint()
	{
		
		
		if (navPoint.Length == 0)
			return;     
		

		//random destination
		
		/*
		destPoint = Random.Range(0, navPoint.Length - 1);
		if(destPoint == lastDest){
			while(destPoint == lastDest){
				destPoint = Random.Range(0, navPoint.Length - 1);
			}
		}
		lastDest = destPoint;
		*/

		//actual pathing
		
		//If 'mom' is at a special point or not (feel free to rewrite if you have a better solution)
		//This big block of garbage is when mom was at special point, it picks out the correct spot on the main path to return to
		if(destPoint > 15){
			
			//oven and counter A are located at 8
			if(navPoint[destPoint].name == "oven" || navPoint[destPoint].name == "counterA"){
				if(direction){
					destPoint = 9;
				}
				if(!direction){
					destPoint = 7;
				}
			}
			//table is located at 2
			if(navPoint[destPoint].name == "table"){
				if(direction){
					destPoint = 3;
				}
				if(!direction){
					destPoint = 1;
				}
			}
			//sink is located at 5
			if(navPoint[destPoint].name == "sink"){
				if(direction){
					destPoint = 6;
				}
				if(!direction){
					destPoint = 4;
				}
			}
			
			if(navPoint[destPoint].name == "counterB"){
				if(direction){
					destPoint = 10;
				}
				if(!direction){
					destPoint = 8;
				}
			}
			//fridge is located at 11
			if(navPoint[destPoint].name == "fridge"){
				if(direction){
					destPoint = 12;
				}
				if(!direction){
					destPoint = 10;
				}
			}
			//shortcut is at 3 OR 12
			if(navPoint[destPoint].name == "shortcut"){
				if(direction){
					destPoint = 13;
				}
				if(!direction){
					destPoint = 3;
				}
			}
			
		}
		else{
			
			//Walks down mainpath
			//true is forward (start -> end), false is backward (end -> start)
			if(direction){
				destPoint++;
			}
			if(!direction){
				destPoint--;
			}

			//choose whether or not to break path
			
			if(destPoint == 2){
				coinflip = Random.Range(0, 2);
				if(coinflip == 1){
					//table
					destPoint = 16;
				}
			}
			
			if(destPoint == 4 || destPoint == 13){
				coinflip = Random.Range(0, 2);
				if(coinflip == 1){
					//shortcut entrance A
					destPoint = 19;
				}
			}

			if(destPoint == 5){
				coinflip = Random.Range(0, 2);
				if(coinflip == 1){
					//sink
					destPoint = 20;
				}
			}

			if(destPoint == 8){
				coinflip = Random.Range(0, 3);
				if(coinflip == 1){
					//oven
					destPoint = 21;
				}
				if(coinflip == 2){
					//counter spot A
					destPoint = 17;
				}
			}

			if(destPoint == 9){
				coinflip = Random.Range(0, 2);
				if(coinflip == 1){
					//countertopB
					destPoint = 18;
				}
			} 

			if(destPoint == 11){
				coinflip = Random.Range(0, 2);
				if(coinflip == 1){
					//fridge
					destPoint = 22;
				}
			} 
			
			if(navPoint[destPoint].name == "start"){
				direction = !direction;
			}
			if(navPoint[destPoint].name == "end"){
				direction = !direction;
			}

		}



		agent.destination = navPoint[destPoint].position;
		
		//Locations to stop and wait
		if(navPoint[destPoint].name == "oven" || navPoint[destPoint].name == "sink" || navPoint[destPoint].name == "fridge"){
			readyToStop = true;
		}
		if(navPoint[destPoint].name == "counterA" || navPoint[destPoint].name == "counterB"||navPoint[destPoint].name == "table"){
			readyToStop = true;
		}
		
	}

}
