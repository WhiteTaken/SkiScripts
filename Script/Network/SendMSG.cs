using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
public class SendMSG : MonoBehaviour {
    private GameObject player;
    private float timer = 0;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }
    //private MainMenu mm;
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
    void FixedUpdate()
    {
        if (NetworkData.GameStatus != NetworkData.GAMESTART)//只有在游戏开始状态才发送数据
        {
            return;
        }
        timer += Time.fixedDeltaTime;   //限制发送次数

        if (timer > 0.05f)//每0.05秒发一次
        {
            sendData();
            timer = 0;
        }
        
    }
    void sendData()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 playerRotation = player.transform.localRotation.eulerAngles;
        lock (SocketConnect.datalock)//加锁
        {
            NetworkData.playerPosX = playerPosition.x;//给玩家位置赋值
            NetworkData.playerPosY = playerPosition.y;
            NetworkData.playerPosZ = playerPosition.z;
            NetworkData.playerRotX = playerRotation.x;
            NetworkData.playerRotY = playerRotation.y;
            NetworkData.playerRotZ = playerRotation.z;
        }

        if (NetworkData.ConnectionStatus == NetworkData.ON_LINE)//客户端在线
        {
            MainMenu.sc.SendMSG(packageData());
        } 
    }

    byte[] packageData()//打包位置和姿态数据
    { 
        byte[] data=new byte[24];
        byte[][] tempdata=new byte[6][];
        lock (SocketConnect.datalock)//加锁
        {
            tempdata.SetValue(ByteUtil.float2ByteArray(NetworkData.playerPosX), 0);
            tempdata.SetValue(ByteUtil.float2ByteArray(NetworkData.playerPosY), 1);
            tempdata.SetValue(ByteUtil.float2ByteArray(NetworkData.playerPosZ), 2);
            tempdata.SetValue(ByteUtil.float2ByteArray(NetworkData.playerRotX), 3);
            tempdata.SetValue(ByteUtil.float2ByteArray(NetworkData.playerRotY), 4);
            tempdata.SetValue(ByteUtil.float2ByteArray(NetworkData.playerRotZ), 5);
        }
        int index=0;
        for (int i = 0; i < tempdata.GetLength(0); i++)
        {
            for (int j = 0; j < tempdata[i].GetLength(0); j++)
            {
                data[index] = tempdata[i][j];
                index++;
            }
        }
        return data;
    }
    void OnApplicationQuit()
    {
        if(GameData.network)
            MainMenu.sc.close();
    }
}
