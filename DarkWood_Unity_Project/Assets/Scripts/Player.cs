using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // mouvement
    public float mouvementSpeed = 5;
    public float rotationSpeed = 10;
    public GameObject playerCamera;
    private Rigidbody rb;

    // animation
    public bool walking;
    private Animator animator;
    private int frameCount;

    // ressource pickup
    public float ressourceValue = 10f;
    private LanternLightManager lanternLightManager;

    // START
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        frameCount = 0;

        walking = false;
        animator = GetComponentInChildren<Animator>();

        lanternLightManager = GetComponentInChildren<LanternLightManager>();
    }

    //OTHER METHODS
    void Move()
    {
        if (Input.GetKey("z") || Input.GetKey("q") || Input.GetKey("s") || Input.GetKey("d"))
        {
            Vector3 worldCamPosition = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, playerCamera.transform.position.z);
            Quaternion worldCamRotation = new Quaternion(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, playerCamera.transform.rotation.z, playerCamera.transform.rotation.w);

            Vector3 vecCam = Vector3.forward;

            if (Input.GetKey("z"))
            {
                vecCam = playerCamera.transform.forward;
                vecCam.y = 0;
                vecCam = vecCam.normalized;
            }
            else if (Input.GetKey("s"))
            {
                vecCam = playerCamera.transform.forward;
                vecCam.y = 0;
                vecCam = vecCam.normalized * (-1);
            }
            else if (Input.GetKey("d"))
            {
                vecCam = playerCamera.transform.right;
                vecCam.y = 0;
                vecCam = vecCam.normalized;
            }
            else if (Input.GetKey("q"))
            {
                vecCam = playerCamera.transform.right;
                vecCam.y = 0;
                vecCam = vecCam.normalized * (-1);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vecCam), rotationSpeed * Time.deltaTime);

            playerCamera.transform.position = worldCamPosition;
            playerCamera.transform.rotation = worldCamRotation;

            //transform.position += vecCam * mouvementSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + vecCam * mouvementSpeed * Time.deltaTime);

            walking = true;
            animator.SetBool("Walk", true);
        }

        if (Input.GetKeyUp("z") || Input.GetKeyUp("q") || Input.GetKeyUp("s") || Input.GetKeyUp("d"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            walking = false;
            animator.SetBool("Walk", false);
        }
    }

    public void WalkTo(GameObject target)
    {
        Vector3 playerToTarget = target.transform.position - transform.position;
        transform.Translate(playerToTarget);
        walking = true;
        animator.SetBool("Walk", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LanternRessource"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            walking = false;
            animator.SetBool("Walk", false);
            animator.SetBool("PickRessource", true);
            PickupRessource(other.gameObject);
        }
    }

    public void PickupRessource(GameObject _ressource)
    {
        Destroy(_ressource.gameObject.GetComponent<Collider>());
        Destroy(_ressource, 2);
        lanternLightManager.RefillLight(ressourceValue);
    }


    // UPDATE
    void Update () {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickFire"))
        {
            walking = false;
            animator.SetBool("Walk", false);
            animator.SetBool("PickRessource", false);
        }
        else
        {
            Move();
        }
        
        if (!walking)
        {
            frameCount += 1;

            if (frameCount % 200 == 0)
            {
                int randAnim = Random.Range(0, 4);
                animator.SetInteger("IdleSelector", randAnim);
            }
        }
    }
}
