using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float distance=10;
    public float height=5;
    public float heightDemp=2;
    public float roationDemp=3;

    public float temp = 0;
	void Start () {

	}
	
    void LateUpdate()
    {
        transform.position = target.position;
        transform.position += -1*target.forward * distance + new Vector3(0,height,0) ;

        //transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        
        transform.LookAt(target);
	}
}
