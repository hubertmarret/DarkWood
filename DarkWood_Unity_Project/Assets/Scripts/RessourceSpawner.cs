using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceSpawner : MonoBehaviour {

    public GameObject orbPrefab;
    
    public float distanceFromPlayer = 20;
    public float distanceDelta = 10;
    public float angleOfSpawnMAX = 45;

    private GameObject player;
    private GameObject house;
    private LanternLightManager LanternMgr;
    

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        house = GameObject.FindGameObjectWithTag("House");
        LanternMgr = player.GetComponent<LanternLightManager>();
    }

    void SpawnInFront()
    {
        Vector3 playerPos = player.transform.position;

        Vector3 playerToHouse = house.transform.position - playerPos;
        playerToHouse.Normalize();

        // FirstOrb
        float randDist = Random.Range(distanceFromPlayer - distanceDelta, distanceFromPlayer + distanceDelta);
        float randAngle = Random.Range(0.0f, angleOfSpawnMAX);
        
        GameObject orb = Instantiate(orbPrefab);
        orb.transform.position = new Vector3(playerPos.x, orb.transform.position.y, playerPos.z);
        orb.transform.Translate(playerToHouse * randDist);
        orb.transform.RotateAround(playerPos, Vector3.up, randAngle);

        // raycast to find the y-position of the masked collider at the transforms x/z
        RaycastHit hit;
        // note that the ray starts at 100 units
        Ray ray = new Ray(orb.transform.position + Vector3.up * 100, Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                // this is where the gameobject is actually put on the ground
                orb.transform.position = new Vector3(orb.transform.position.x, hit.point.y + 1.0f, orb.transform.position.z);
            }
        }

        // SecondOrb
        randDist = Random.Range(distanceFromPlayer - distanceDelta, distanceFromPlayer + distanceDelta);
        randAngle = Random.Range(0.0f, angleOfSpawnMAX);

        GameObject orb2 = Instantiate(orbPrefab);
        orb2.transform.position = new Vector3(playerPos.x, orb2.transform.position.y, playerPos.z);
        orb2.transform.Translate(playerToHouse * randDist);
        orb2.transform.RotateAround(playerPos, Vector3.up, -randAngle);

        // raycast to find the y-position of the masked collider at the transforms x/z
        RaycastHit hit2;
        // note that the ray starts at 100 units
        Ray ray2 = new Ray(orb2.transform.position + Vector3.up * 100, Vector3.down);

        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity))
        {
            if (hit2.collider != null)
            {
                // this is where the gameobject is actually put on the ground
                orb2.transform.position = new Vector3(orb2.transform.position.x, hit2.point.y + 1.0f, orb2.transform.position.z);
            }
        }
    }

    void SpawnAround()
    {
        Vector3 playerPos = player.transform.position;
        
        float randDist = Random.Range(distanceFromPlayer - distanceDelta, distanceFromPlayer + distanceDelta);
        float randAngle = Random.Range(0.0f, 360.0f);

        float randX = randDist * Mathf.Cos(randAngle);
        float randZ = randDist * Mathf.Sin(randAngle);

        GameObject orb = Instantiate(orbPrefab);
        orb.transform.position = new Vector3(playerPos.x + randX, orb.transform.position.y, playerPos.z + randZ);
    }

    // Update is called once per frame
    void Update () {

        //if(Input.GetKeyUp("space"))
        if (LanternMgr.spawnOrbPossible && LanternMgr.GetLightPowerPercent() <= LanternMgr.lightPercentToSpawn)
        {
            SpawnInFront();
            LanternMgr.spawnOrbPossible = false;
        }
	}
}
