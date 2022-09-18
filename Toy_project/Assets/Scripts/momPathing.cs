using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class momPathing : MonoBehaviour
{
    // Start is called before the first frame update
	
	//timer + stopping variables
	private float waitTime = 3.0f;
    private float timer = 0.0f;
	bool readyToStop = false;
	bool wait = false;

	//turning to face whatever stop location is reached
	float goalAngle = 0;
	float currentangle = 0;
	float turnsp = .4f;
	bool setturndirec;

	public GameObject player;
	Vector3 playerpos;
	float Speed = 1f;

	//for the navmesh + pathing
	bool direction = true;
	int coinflip;
	public Transform[] navPoint;
	public UnityEngine.AI.NavMeshAgent agent;
	public int destPoint = 0;
	int lastDest;

	//animation bools
	public bool isgrabbed, iswalking, isidle, seen;


	//chasing player
	public bool chasing = false;
	float chasertimer = 0;
	int MoveSpeed = 50;
	float attentionTime = 5f;
	float playerDistance;
	float grabRange = 25f;
	float AIMoveSpeed = 10f;
	int count = 0;

	void Start () {
		
		UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = navPoint[0].position; 
		agent.speed = 50f;
		
	}
	private void OnEnable(){

		ConeOfVision.onDetectionNormal += ISeeYou;
		//ConeOfVision.onDetectionRagdoll += ISeeYou;
		ConeOfVision.onCeaseDetection += whereyougo;
	}

	private void OnDisable(){

		ConeOfVision.onDetectionNormal -= ISeeYou;
		ConeOfVision.onCeaseDetection -= whereyougo;
		//ConeOfVision.onDetectionRagdoll -= ISeeYou;
	}

	void ISeeYou(){
		seen = true;
		agent.isStopped = true;
	}

	void whereyougo(){
		if(seen){
			wait = false;
			readyToStop = false;
			agent.speed = 50f;
			//GotoNextPoint();
		}
		//if seen was recently set to true, don't set it to false until a couple second have passed
		seen = false;
	}

	void Update(){
		
		if(!seen){
			if(wait){
				if(setturndirec){
					isidle = true;
					iswalking = false;
					currentangle = transform.eulerAngles.y;
					if(currentangle > goalAngle){
						if(currentangle - goalAngle < 180f){
							turnsp = -turnsp;
						}
					}
					else{
						if(goalAngle  - currentangle > 180f){
							turnsp = -turnsp;
						}
					}
					setturndirec = false;
				}

				if(currentangle > goalAngle + .5f || currentangle < goalAngle - .5f){
					currentangle += turnsp;
					if(currentangle > 360 || currentangle < 0){
						currentangle = 0;
					}
				}

				transform.rotation = Quaternion.Euler(0f, currentangle, 0f);
				timer += Time.deltaTime;
				// Check if we have reached beyond 2 seconds.
				// Subtracting two is more accurate over time than resetting to zero.
				if(timer > waitTime)
				{
					wait = false;
					readyToStop = false;
					agent.speed = 50f;
					GotoNextPoint();
					timer =  0;
				}
			
			}
			
			if(!wait && !chasing){
				iswalking = true;
				isidle = false;
				if(agent.remainingDistance < 0.5f){
					if(readyToStop){
						turnsp = .4f;
						wait = true;
						agent.speed = 0;
					}
					if(!readyToStop){
						GotoNextPoint();
					}
			
				}
				
			}
		}
		
		if(seen){
			setturndirec = true;
			chasing = true;
			Vector3 target = new Vector3(player.transform.position.x, transform.position.y,player.transform.position.z);
			playerpos = player.transform.position;
			transform.LookAt(target);
			if (Vector3.Distance(transform.position, player.transform.position) >= grabRange - 5f)
			{
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
			}
			if(playerDistance < grabRange){
				//Debug.Log("I grabbed you ;)");
				isgrabbed = true;
			}
		}
		
		if(chasing && !seen){
			if (Vector3.Distance(transform.position, playerpos) >= 25f)
			{
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
			}
			else{
				if(setturndirec){
					goalAngle = Random.Range(0f, 360f);
					isidle = true;
					iswalking = false;
					currentangle = transform.eulerAngles.y;
					if(currentangle > goalAngle){
						if(currentangle - goalAngle < 180f){
							turnsp = -turnsp;
						}
					}
					else{
						if(goalAngle  - currentangle > 180f){
							turnsp = -turnsp;
						}
					}
					setturndirec = false;
				}
				if(currentangle > goalAngle + .5f || currentangle < goalAngle - .5f){
					currentangle += turnsp * .3f;
					if(currentangle > 360 || currentangle < 0){
							currentangle = 0;
					}
				}
				else{
					setturndirec = true;
				}
				transform.rotation = Quaternion.Euler(0f, currentangle, 0f);
			}
			chasertimer += Time.deltaTime;
				// Check if we have reached beyond 2 seconds.
				// Subtracting two is more accurate over time than resetting to zero.
			if(chasertimer > attentionTime)
			{
				agent.isStopped = false;
				chasing = false;
				wait = false;
				readyToStop = false;
				agent.speed = 50f;
				GotoNextPoint();
				chasertimer =  0;
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
					goalAngle = 300f;
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
					goalAngle = 180f;
				}
			}

			if(destPoint == 8){
				coinflip = Random.Range(0, 3);
				if(coinflip == 1){
					//oven
					destPoint = 21;
					goalAngle = 180f;
				}
				if(coinflip == 2){
					//counter spot A
					destPoint = 17;
					goalAngle = 0f;
				}
			}

			if(destPoint == 9){
				coinflip = Random.Range(0, 2);
				if(coinflip == 1){
					//countertopB
					destPoint = 18;
					goalAngle = 0f;
				}
			} 

			if(destPoint == 11){
				coinflip = Random.Range(0, 2);
				if(coinflip == 1){
					//fridge
					destPoint = 22;
					goalAngle = 270f;
				}
			} 

			currentangle = transform.eulerAngles.y;
			setturndirec = true;
			if(currentangle < 0){
				currentangle += 360f;
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
