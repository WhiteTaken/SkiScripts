using UnityEngine;
using System.Collections;

public class EndCalculateDirectionTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameData.calculateMapDirection = false;//停止计算法线
            GameData.currentMapDirection = this.transform.forward;
            GameData.currentCurveNumber++;
            Debug.Log("end");
        }
    }
}
