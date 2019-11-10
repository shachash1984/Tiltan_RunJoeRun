using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour {

    public int id;
    public BoxCollider _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

	void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggerred segment: " + id);
        if (other.GetComponent<Player>())
        {
            //Debug.Log("<color=red>name: " + name + " trigger.z: " + _collider.center.z + "</color>");
            
            SpawnManager.S.prevSegment = SpawnManager.S.currentSegment;
            SpawnManager.S.currentSegment = SpawnManager.S.SetCurrentSegment(this.id);
            //Debug.Log("<color=red>Player.z " + Player.S.transform.position.z + "| currentSegment.center.z: " + SpawnManager.S.currentSegment.GetComponent<Segment>()._collider.center.z + "</color>");
            SpawnManager.S.Spawn(transform.position.z);
            Player.S.IncreaseSpeed();
        }
            
    }

    public float GetPlayerDistanceFromTrigger()
    {        
        return (Vector3.Distance(Player.S.transform.position, transform.position));
    }

    /*void Update()
    {
        if(Time.frameCount%1200 == 0 && gameObject != SpawnManager.S.prevSegment && gameObject != SpawnManager.S.currentSegment)
        {
            StartCoroutine(SpawnManager.S.Despawn(gameObject));
        }
    }*/
}
