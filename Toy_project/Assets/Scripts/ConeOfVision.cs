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

    public delegate void CeaseDetection();
    public static event CeaseDetection onCeaseDetection;

    public bool isDetectedWhileRagdoll;

    bool prevRagdollDetection;
    bool prevNormalDetection;

    public List<GameObject> playerGOS;
    public List<Collider> playerColliders;
    [SerializeField] RagdollController ragdollController;
    [SerializeField] LayerMask layerMask;
    //00100100 ignore the sixth and third layer (character and its ragdoll)
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
        bool detectedRagdoll = false;
        bool detectedNormal = false;
        //Debug.Log(GetComponent<Camera>().gameObject);
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
            if (GeometryUtility.TestPlanesAABB(planes, col.bounds)) {
                // if it is send a raycast from the camera to the player, ignoring player and its ragdoll\
                //Debug.Log("in bounds");
                
                RaycastHit hit;
                Vector3 directionToPlayer = -(transform.position - col.gameObject.transform.position).normalized * Vector3.Distance(transform.position, col.gameObject.transform.position);
                //Debug.Log(Physics.Raycast(transform.position, directionToPlayer , layerMask));
                if (Physics.Raycast(transform.position, directionToPlayer , layerMask)) {
                    if (isDetectedWhileRagdoll && onDetectionRagdoll != null)
                    {
                        //Listener & Delegate behavior.

                        detectedRagdoll = true;
                    }
                    else if (onDetectionNormal != null)
                    {
                        //Debug.Log("player detected");
                        detectedNormal = true;
                    }
                }
            }
        }
        if (detectedNormal && onDetectionNormal != null){ 
            onDetectionNormal();
        }
        else if (detectedRagdoll && onDetectionRagdoll != null)
        {
            onDetectionRagdoll();
        }
        else if ((prevNormalDetection && !detectedNormal) || (prevRagdollDetection && !detectedRagdoll) && onCeaseDetection !=null){
            Debug.Log("callling");
            onCeaseDetection();
        }
        prevNormalDetection = detectedNormal;
        prevRagdollDetection = detectedRagdoll;
    }
}
