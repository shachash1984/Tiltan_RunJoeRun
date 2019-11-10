using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    static public SpawnManager S;
    public List<GameObject> segmentPool = new List<GameObject>();
    public List<GameObject> prefabPool = new List<GameObject>();
    [HideInInspector] public GameObject currentSegment;
    [HideInInspector] public GameObject prevSegment;
    private List<GameObject> activeSegments;
    private List<GameObject> inactiveSegments;
    [SerializeField] private List<GameObject> tutorialSegments = new List<GameObject>(); 

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        else
            S = this;

        
    }    

    void Start()
    {
        //spawn first 3 segments
        Init();
    }

    private void Init()
    {
        if (GameManager.S.playTutorial && !GameManager.S.isContinue)
        {
            PlayTutorial();
        }
        else
        {
            activeSegments = new List<GameObject>();
            inactiveSegments = new List<GameObject>(segmentPool.ToArray());
            for (int i = 0; i < 3; i++)
            {
                float f = i * 200;
                Spawn(f, true);
            }
        }
        
    }

    public void Spawn(float currentSegmentZPos, bool firstSpawn = false)
    {
        //called each time the player enters the segment trigger

        //pick a random segment from the inactive segment list
        //remove the segment from the inactive list        
        //place the segment in the correct position (according to currentSegmentZPos)
        //activate the segment
        //add the segment to the active list        
        try
        {
            int rand = Random.Range(0, inactiveSegments.Count);
            GameObject segmentToSpawn = inactiveSegments[rand];
            //Debug.Log("rand: " + rand + "inactiveSegments.Count: " + inactiveSegments.Count);
            if (inactiveSegments.Count == 10)
                currentSegment = segmentToSpawn;
            inactiveSegments.Remove(segmentToSpawn);
            if (firstSpawn)
            {
                segmentToSpawn.transform.position = new Vector3(0f, 0f, currentSegmentZPos);
            }
            else if (GameManager.S.playTutorial)
            {
                segmentToSpawn.transform.position = new Vector3(0f, 0f, currentSegmentZPos + 600f);
            }
            else
            {
                segmentToSpawn.transform.position = new Vector3(0f, 0f, currentSegmentZPos + 400f);
                //Debug.Log("new seg pos: " + segmentToSpawn.transform.position);
            }
            //Debug.Log("<color=green>Spawned: " + segmentToSpawn.GetComponent<Segment>().id + "time: " + Time.time + "</color>");
            if (prevSegment != null && prevSegment.transform.position.z < currentSegment.transform.position.z)
                StartCoroutine(Despawn(prevSegment));

            segmentToSpawn.SetActive(true);
            activeSegments.Add(segmentToSpawn);
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log("Created new Segment");        
            activeSegments.Add(CreateSegment(new Vector3(0f, 0f, currentSegmentZPos + 200f)));
            if (prevSegment != null && prevSegment.transform.position.z < currentSegment.transform.position.z)
                StartCoroutine(Despawn(prevSegment));
        } 

        
    }

    public IEnumerator Despawn(GameObject segmentToDespawn)
    {
        //called inside Spawn()
        //waits for 1 second

        //takes the previous segment the player was on and deactivates it (according to its id)
        // removes the previous segment from the active list
        // adds the previous segment to the inactive segment list
        //Debug.Log("<color=blue>currentSegment: " + SpawnManager.S.currentSegment.name +" distanceToplayer: " + (int)(currentSegment.GetComponent<Segment>().GetPlayerDistanceFromTrigger()) + "</color>");
        int distanceFromPlayerToTrigger = (int)currentSegment.GetComponent<Segment>().GetPlayerDistanceFromTrigger() ;
        //Debug.Log("distanceFromPlayerToTrigger : " + distanceFromPlayerToTrigger);
        //Debug.Log("CurrentSegment.z: " + currentSegment.transform.position.z + "distance from player to trigger: " + distanceFromPlayerToTrigger);
        while (distanceFromPlayerToTrigger > 5)
        {
            distanceFromPlayerToTrigger = (int)currentSegment.GetComponent<Segment>().GetPlayerDistanceFromTrigger();
            //Debug.Log("distanceFromPlayerToTrigger : " + distanceFromPlayerToTrigger);
            yield return new WaitForEndOfFrame();
        }        
        
        if (segmentToDespawn != null)
        {
            segmentToDespawn.SetActive(false);
            //Debug.Log("<color=blue>Despawned: " + segmentToDespawn.GetComponent<Segment>().id + "time: " + Time.time + "</color>");
            activeSegments.Remove(segmentToDespawn);
            inactiveSegments.Add(segmentToDespawn);
        }        
    }

    public GameObject SetCurrentSegment(int id)
    {
        return segmentPool.Find(go => go.GetComponent<Segment>().id == id);
    }
        
    private GameObject CreateSegment(Vector3 newSegPos)
    {
        int rand = Random.Range(0, prefabPool.Count);
        GameObject segmentToCreate = Instantiate(prefabPool[rand], newSegPos, Quaternion.identity);
        return segmentToCreate;
    }

    private void PlayTutorial()
    {
        Debug.Log("SpawnManager.PlayTutorial()");
        foreach (GameObject go in tutorialSegments)
        {
            go.SetActive(true);
        }

        activeSegments = new List<GameObject>();
        inactiveSegments = new List<GameObject>(segmentPool.ToArray());
        for (int i = 0; i < 3; i++)
        {
            float f = (tutorialSegments.Count-1) * 200 + (i * 200);
            Spawn(f, true);
        }
    }
}
