using UnityEngine;
using System.Collections;

public class ResetPlayerPOS : MonoBehaviour {
    private PathNode lastPoint;//上一个经过的路径点
    public PathNode nextPoint;
    public float minDis = 10.0f;//最小距离
    private bool resetFlag = false;
    private float falltime=0;
	// Use this for initialization
	void Start () {
        nextPoint=GameObject.Find("ResetPoint/point0").GetComponent<PathNode>();
        lastPoint = nextPoint;

        falltime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPOS = new Vector3(this.transform.position.x, 0, this.transform.position.z);//玩家位置
        float distence = Vector3.Distance(nextPoint.transform.position, playerPOS);//距离
        if(distence<minDis)
        {
            if (nextPoint.m_child != null)
            {
                lastPoint = nextPoint;
                nextPoint = nextPoint.m_child;
            }
            else//到了最后一个路径点
            {
                lastPoint = nextPoint;
            }
        }
        if (resetFlag)
        {
            falltime += Time.deltaTime;
            //Debug.Log(falltime);
        }
        if (GameData.isFall)//摔倒
        {
            resetFlag = true;
            if(falltime>2)
            {
                this.transform.position=lastPoint.transform.position;
                this.transform.forward = lastPoint.gameObject.transform.forward;
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GameData.calculateMapDirection = false;
                GameData.isFall = false;
                GameData.currentMapDirection = lastPoint.transform.forward;
                resetFlag = false;
                falltime = 0;
            }
        }
        //Debug.Log(lastPoint.m_child.name + lastPoint.name);
	}
}
