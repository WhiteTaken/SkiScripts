using UnityEngine;
using System.Collections;

public class SetMapDirection : MonoBehaviour {
    public GameObject[] curve;

    void Start()
    {
        if(GameData.CurveNumber==2)
        {
            curve=new GameObject[2];
            curve[0] = GameObject.Find("Curve1/Sphere1");
            curve[1] = GameObject.Find("Curve2/Sphere2");
        }
    }
    void Update()
    {
        
        if (!GameData.calculateMapDirection||GameData.currentCurveNumber>=GameData.CurveNumber)//当前不需要计算赛道方向
        {
            return;
        }
        Vector3 dir = Vector3.Cross(curve[GameData.currentCurveNumber].transform.position - transform.position, Vector3.up).normalized;
        if (Vector3.Dot(dir, transform.forward) < 0)
        {
            dir *= -1;
        }
        GameData.currentMapDirection = dir;
        //Debug.Log(GameData.currentCurveNumber+"  "+Vector3.Dot(dir,transform.forward));
        Debug.DrawRay(transform.position, dir, Color.blue);
    }
}
