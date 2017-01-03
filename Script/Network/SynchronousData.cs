using UnityEngine;
using System.Collections;

public class SynchronousData : MonoBehaviour
{
    private GameObject bot;
    private Vector3 botPOS = new Vector3();
    private Vector3 botROT = new Vector3();
    // Use this for initialization
    void Start () {
        if (bot == null)
        {
            bot = GameObject.FindWithTag("BOT");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (NetworkData.ConnectionStatus == NetworkData.ON_LINE)
        {
            lock (SocketConnect.datalock)
            {
                botPOS=new Vector3(NetworkData.enemyPosX, NetworkData.enemyPosY, NetworkData.enemyPosZ);
                botROT= new Vector3(NetworkData.enemyRotX, NetworkData.enemyRotY, NetworkData.enemyRotZ);
            }
            bot.transform.position = Vector3.Lerp(bot.transform.position,botPOS,Time.deltaTime*20f);
            bot.transform.eulerAngles = botROT;
        }
     }
}
