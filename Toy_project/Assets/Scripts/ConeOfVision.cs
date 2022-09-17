using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class ConeOfVision : MonoBehaviour
{

    public delegate void DetectionRagdoll();
    public static event DetectionRagdoll onDetectionRagdoll;

    public delegate void DetectionNormal();
    public static event DetectionNormal onDetectionNormal;

    public delegate void NotSeen();
    public static event NotSeen NotSaw;

    public bool isDetectedWhileRagdoll;
    public bool sightline = false;
    public bool incamera = false;
    public List<GameObject> playerGOS;
    public List<Collider> playerColliders;
    [SerializeField] RagdollController ragdollController;
    public int LayerMask = 36; //(was 36 or 100) - 00100100 ignore the sixth and third layer (character and its ragdoll)
    // Start is called before the first frame update
    int count = 0;

    void Start()
    {
        playerGOS = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject playerGO in playerGOS)
        {
            Collider col = (playerGO.GetComponent<Collider>());
            playerColliders.Add(col);
        }

    }
    
    // Update is called once per frame

    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
        foreach (Collider col in playerColliders){
            if (ragdollController.isRagdoll) {
                isDetectedWhileRagdoll = true;
                if (col.GetType() == typeof(CharacterController))
                {
                    continue;
                }
            }
            else if (col.GetType() == typeof(CapsuleCollider) || col.GetType() == typeof(BoxCollider))
            {
                isDetectedWhileRagdoll = false;
                continue;
            }
        
            //Debug.Log(GeometryUtility.TestPlanesAABB(planes, col.bounds));
            //check to see if the players box collider is inside of the frustum.
            incamera = GeometryUtility.TestPlanesAABB(planes, col.bounds);
            if(GeometryUtility.TestPlanesAABB(planes, col.bounds)){
                // if it is send a raycast from the camera to the player, ignoring player and its ragdoll
                sightline = Physics.Raycast(transform.position, col.gameObject.transform.position, LayerMask);
                if (Physics.Raycast(transform.position, col.gameObject.transform.position, LayerMask)){
                    //Debug.Log("stuff is happening");
                    //count++;
                    //Debug.Log(count);
                    if(isDetectedWhileRagdoll && onDetectionRagdoll != null)
                    {
                        //Listener & Delegate behavior.
                        onDetectionRagdoll();
                    }
                    else if(onDetectionNormal != null)
                    {
                        onDetectionNormal();
                    }
                }
                
                else{
                    NotSaw();
                }
                
            }
            else{
                NotSaw();
            }
        }
    }
}
