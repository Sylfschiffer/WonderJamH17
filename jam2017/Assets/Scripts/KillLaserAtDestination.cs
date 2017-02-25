﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLaserAtDestination : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(gameObject.name == other.name)
        {
            Debug.Log("Entered");
            Destroy(other.gameObject);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled=true;
            Debug.Log(gameObject.transform.GetChild(0).name);
            gameObject.GetComponent<FadeOut>().enabled = true;
        }
    }
}