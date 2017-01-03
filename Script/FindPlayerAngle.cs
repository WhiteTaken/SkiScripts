using UnityEngine;
using System.Collections;

public class FindPlayerAngle : MonoBehaviour {
    private GameObject[] BOT;
    //float tempAngle = 0;
	void Start () {
        BOT = GameObject.FindGameObjectsWithTag("BOT");//查找标签为“player”的物体
        Debug.Log(this.gameObject.name);
	}
	void LateUpdate () {

        if (BOT.Length != 0)
        {
            foreach (GameObject player in BOT)
            {
                if (!GameData.isRise)
                { 
                    FindAngle(player);
                    //player.transform.Rotate(player.transform.forward,Time.deltaTime*20);
                }
            }
        }
	}
    void FindAngle(GameObject player)//修正角度
    {
        Ray ray = new Ray(player.transform.position, player.transform.up * -3);//声明一条射线沿着角色的Y轴向下
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))//获取碰撞点
        {
            Vector3 currentUP = Vector3.Lerp(player.transform.up, hit.normal, Time.deltaTime * 3);
            Vector3 rotateAxis = Vector3.Cross(player.transform.up, currentUP).normalized;
            float rotateAngle = Vector3.Angle(player.transform.up, currentUP);
            player.transform.Rotate(rotateAxis, rotateAngle, Space.World);
        }
    }
}
