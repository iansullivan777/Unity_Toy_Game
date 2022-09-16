using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConeOfVision : MonoBehaviour
{
    List<GameObject> player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>()); 
    }
}
