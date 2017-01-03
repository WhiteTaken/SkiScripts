using UnityEngine;
using System.Collections;

public class InitGameScence : MonoBehaviour {
    public GameObject[] Players;
    public GameObject[] Bots;
    public GameObject[] Enemys;
    //private GameObject player;
    //private GameObject bot;
    private Vector3 pos1 = new Vector3(-1.61f, 0.63f, 64.34f);
    private Vector3 pos2 = new Vector3(3.03f,  0.63f, 64.34f);
    private Vector3 level1end = new Vector3(400.4f, -14.2f, 772);
    private Vector3 level2end = new Vector3(599.6f, -13.464f, 1470.4f);
    public static bool initplayerandbotflag = false;
	void Awake () {
        initPlayerAndBot();
        initGameData();
        GameObject.Find("Main Camera/BackGroundMusic").GetComponent<AudioSource>().volume = 
            GameData.BackGroundSoundVoiume;//设置背景音乐音量

	}
    void initPlayerAndBot()//初始化角色和机器人
    {
        if (GameData.network)//网络模式
        {
            lock (SocketConnect.datalock)//加锁
            {
                NetworkData.playerPosX = NetworkData.clientnumber == 1 ? pos1.x : pos2.x;
                NetworkData.playerPosY = NetworkData.clientnumber == 1 ? pos1.y : pos2.y;
                NetworkData.playerPosZ = NetworkData.clientnumber == 1 ? pos1.z : pos2.z;
                NetworkData.playerRotX = 0;
                NetworkData.playerRotY = 0;
                NetworkData.playerRotZ = 0;
            }
            Instantiate(Players[NetworkData.playerModoIndex], NetworkData.clientnumber == 1 ? pos1 : pos2, Quaternion.identity);
            Instantiate(Enemys[NetworkData.enemyModoIndex], NetworkData.clientnumber == 1 ? pos2 : pos1, Quaternion.identity);


            Camera.main.GetComponent<SendMSG>().enabled = true;//开启发送数据脚本
            Camera.main.GetComponent<SynchronousData>().enabled = true;//开启同步数据脚本
        }
        else
        {
            Instantiate(Players[PlayerData.playerIndex], pos1, Quaternion.identity);
            Instantiate(Bots[0], pos2, Quaternion.identity);

        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
    }

    void initGameData()//初始化游戏数据
    {
        GameData.currentMapNormal = new Vector3(0, 1, 0);
        GameData.nextMapNormal = new Vector3(0, 1, 0);
        GameData.currentMapDirection = Vector3.forward;
        GameData.isRise = false;
        GameData.calculateMapDirection = false;
        GameData.currentCurveNumber = 0;
        GameData.isGround = false;
        GameData.isFall = false;
        GameData.GameOver = false;
        GameData.GameStart = false;
        GameData.timer = "--";
        GameData.playerRank = 1;
        //GameData.AccelerateProgress = 0;
    }
}
