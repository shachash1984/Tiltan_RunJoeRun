using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public List<Level> levels;
    private int currentLevel;

    // Use this for initialization
    void Start () {
        Instantiate(levels[0]);
        Instantiate(levels[1]);
        Instantiate(levels[2]);


        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].end.transform.position.x > Player.S.transform.position.x)
            {
                currentLevel = i;
                break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {


        if (Player.S.transform.position.z > levels[currentLevel].end.transform.position.z)
        {
            Level first = levels[0];
            levels.RemoveAt(0);
            first.transform.position = levels[levels.Count - 1].end.transform.position;
            Instantiate(first);
            levels.Add(first);
        }


    }
}
