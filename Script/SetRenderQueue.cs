using UnityEngine;
using System.Collections;

public class SetRenderQueue : MonoBehaviour {
    private GameObject track;
    private TrailRenderer boardTrace;
	// Use this for initialization
	void Start () {
        track = GameObject.FindGameObjectWithTag("track");
        boardTrace=this.GetComponentInChildren<TrailRenderer>();
        boardTrace.GetComponent<Renderer>().material.renderQueue = track.GetComponent<Renderer>().material.renderQueue + 1;
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
