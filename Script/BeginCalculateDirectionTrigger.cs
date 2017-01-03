using UnityEngine;
using System.Collections;

public class BeginCalculateDirectionTrigger : MonoBehaviour
{
    //碰到该触发器开始计算赛道方向
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameData.calculateMapDirection = true;
            Debug.Log("begin");
        }
    }
}
