using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RagdollController : MonoBehaviour
{
    StarterAssets.StarterAssetsInputs playerInputs;
    StarterAssets.ThirdPersonController thirdPersonController;
    GameObject hips;
    Collider mainCollider;
    Rigidbody mainRigidbody;
    Animator anim;
    List<Rigidbody> rigidbodies;
    public bool isRagdoll = false;


    // Start is called before the first frame update
    void Start()
    {
        mainCollider = this.gameObject.GetComponent<Collider>();
        mainRigidbody = this.gameObject.GetComponent<Rigidbody>();
        anim = this.gameObject.GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        hips = GameObject.Find("Hips");
        playerInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        thirdPersonController = GetComponent<StarterAssets.ThirdPersonController>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnableRagdoll(bool enable)
    {
        // If the ragdoll isn't on the ground and we're in ragdoll, ignore the input. Do not Ragdoll!
        if (!isRagdollGrounded() && isRagdoll)
        {
            return;
        }
        // Sets the Characters position to the Rigidbody's hips.
        // Because we're disabling the CharacterController to avoid janky collision
        // We must teleport to the ragdoll because when we disable the player stops falling with the player.
        if (enable == false && isRagdoll == true) {
            transform.transform.position = hips.transform.position;
            playerInputs.jump = true;
        }
        // Set the ragdoll condition after we have exhausted the use for the previous position.
        isRagdoll = enable;

        // enable all of the rigidbodies
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            rigidbodies[i].isKinematic = !enable;
        }
        // disable the CharacterController
        // Note: will freak out without this.
        if (mainCollider != null)
        {
            mainCollider.enabled = !enable;
        }
        // see above
        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = enable;
        }
        // disable the animations for the character controller.
        if (anim != null)
        {
            anim.enabled = !enable;
        }
    }
    public bool isRagdollGrounded()
    {
        // Spawns a sphere around the player's hips, if its inside of the ground then the ragdoll is on the ground.
        Vector3 spherePosition = new Vector3(hips.transform.position.x, hips.transform.position.y,
                hips.transform.position.z);
        return Physics.CheckSphere(spherePosition, thirdPersonController.GroundedRadius * 5, thirdPersonController.GroundLayers,
            QueryTriggerInteraction.Ignore);
    }
}