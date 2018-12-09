using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMove : MonoBehaviour {

    public float wanderingSpeed = 4f;
    public float runningSpeed = 7f;
    private Vector3 targetPosition;

    public bool seePlayer = false;
    public GameObject player;

    public float blindMoveTime = 10.0f;
    private float curBlindMoveTime;

    public Animator animator;
    //public WolfCam wolfCam;
    private Rigidbody rb;
    public AudioSource wolfAudio;
    private GameObject house;


    public float rangeOfDetection = 50.0f;
    public float rangeForHowlTrigger = 130.0f;
    public float rangeEndOfAmbientSound = 90.0f;
    public float spawnDelta = 30.0f;
    public float angleOfSpawnMAX = 65.0f;
    public float timeBetweenMoveRandom = 30.0f;
    private float timeBeforeMoveRandom;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        wolfAudio = GetComponent<WolfAudio>().wolfFarAudio;
        house = GameObject.FindGameObjectWithTag("House");
        timeBeforeMoveRandom = timeBetweenMoveRandom;
        MoveRandom();
    }
	
	// Update is called once per frame
	void Update () {
        float _playerDistance = Vector3.Distance(transform.position, player.transform.position);
        if (_playerDistance < rangeOfDetection)
        {
            seePlayer = true;
            curBlindMoveTime = blindMoveTime;
            targetPosition = player.transform.position;
        } else
        {
            seePlayer = false;
            curBlindMoveTime -= Time.deltaTime;
        }

        if (curBlindMoveTime > 0)
        {
            MoveStraightLine();
            animator.SetBool("Run", true);
        } else
        {
            animator.SetBool("Run", false);
            
            GameObject[] orbs;
            orbs = GameObject.FindGameObjectsWithTag("LanternRessource");
            MoveAwayFromClosest(orbs);
            rb.angularVelocity = Vector3.zero;
        }
        timeBeforeMoveRandom -= Time.deltaTime;
        if (_playerDistance > rangeForHowlTrigger && timeBeforeMoveRandom <= 0.0f)
        {
            timeBeforeMoveRandom = timeBetweenMoveRandom;
            MoveRandom();
        }
        //Debug.Log(timeBeforeMoveRandom);
        player.GetComponent<PlayerAudio>().fadeAmbientSoundWithDistance(_playerDistance, rangeEndOfAmbientSound, rangeForHowlTrigger);

	}

    public void MoveRandom()
    {
        Vector3 playerPos = player.transform.position;

        Vector3 playerToHouse = house.transform.position - playerPos;
        playerToHouse.Normalize();

        // FirstOrb
        float randDist = Random.Range(rangeForHowlTrigger + 10.0f, rangeForHowlTrigger + spawnDelta);
        float randAngle = Random.Range(0.0f, angleOfSpawnMAX);
        
        transform.position = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        transform.rotation = Quaternion.identity;
        transform.Translate(playerToHouse * randDist);
        transform.RotateAround(playerPos, Vector3.up, randAngle);

        // raycast to find the y-position of the masked collider at the transforms x/z
        RaycastHit hit;
        // note that the ray starts at 100 units
        Ray ray = new Ray(transform.position + Vector3.up * 100, Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                // this is where the gameobject is actually put on the ground
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }
    }

    public void MoveStraightLine()
    {
        transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            transform.position = targetPosition;
            curBlindMoveTime = 0;
        } else
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * runningSpeed);
        }
        
    }

    public void MoveAwayFromClosest(GameObject[] _gameObjects)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject go in _gameObjects)
        {
            float dist = Vector3.Distance(go.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = go.transform;
                minDist = dist;
            }
        }
        if (minDist < rangeOfDetection + 5.0f)
        {
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.LookRotation(transform.position - tMin.position);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * runningSpeed);
        } else
        {
            animator.SetBool("Run", false);
        }
    }
}
