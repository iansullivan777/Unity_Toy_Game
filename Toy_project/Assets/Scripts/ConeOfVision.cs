using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConeOfVision : MonoBehaviour
{
    public List<GameObject> playerGOS;
    public List<Collider> playerColliders;
    [SerializeField] RagdollController ragdollController;
    public int LayerMask = 36; //00100100 ignore the sixth and third layer (character and its ragdoll)
    // Start is called before the first frame update
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
        Debug.Log(GetComponent<Camera>().gameObject);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
        foreach (Collider col in playerColliders) {
            

            //check to see if the players box collider is inside of the frustum.
            if(GeometryUtility.TestPlanesAABB(planes, col.bounds)){
                // if it is send a raycast from the camera to the player, ignoring player and its ragdoll
                if (Physics.Raycast(transform.position, col.gameObject.transform.position, LayerMask)){
                     
                }
            }
        }
    }
}
