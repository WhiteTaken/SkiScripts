using UnityEngine;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class SocketConnect
{
    private Socket mySocket;
    private static SocketConnect st;
    public static System.Object datalock = new System.Object();//锁
    Thread thread;
    bool getMSGFlag = true;
    public static SocketConnect getSocketInstance(String ip)//获取SocketText对象
    {
        if (st == null)
        {
            st = new SocketConnect(ip);
        }
        return st;
    }
    SocketConnect(String ipStr)//构造器
    {
        mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse("192.168.31.173");
        Debug.Log("Ip" + ipStr);
        IPEndPoint ipe = new IPEndPoint(ip, 2015);
        IAsyncResult result = mySocket.BeginConnect(ipe, new AsyncCallback(ConnectCallBack), mySocket);
        //mySocket.Connect(ipe);
        bool connectsucces = result.AsyncWaitHandle.WaitOne(5000, true);//超时
        if (connectsucces)//连接成功
        {
            thread = new Thread(new ThreadStart(GetMSG));//线程方法
            thread.IsBackground = true;
            thread.Start();
        }
        else
        {
            Debug.Log("time out");
        }
    }
    void ConnectCallBack(IAsyncResult ast)
    {
        Debug.Log("connect success...");
    }
    int readInt()
    {
        byte[] bint = readPackage(4);
        return ByteUtil.byteArray2Int(bint, 0);
    }
    byte[] readPackage(int len)
    {
        byte[] bPackage = new byte[len];        //收到的数据包
        int status = mySocket.Receive(bPackage);//第一次接受的长度
        while (status != len)              //循环接收 直到收够指定长度
        {
            int index = status;         //记录已经收到的长度
            byte[] tempData = new byte[len - status];
            int count = mySocket.Receive(tempData);     //接受剩下的
            status += count;            //更新已接受到的长度
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    bPackage[index + i] = tempData[i]; //合并数组
                }
            }
        }
        return bPackage;
    }
    private void GetMSG()//接受数据包线程
    {
        while (getMSGFlag)
        {
            try
            {
                int dataLen = readInt();
                byte[] bdata = readPackage(dataLen);
                splitPackage(dataLen, bdata);
            }
            catch (Exception e)
            {
                //NetworkData.ConnectionStatus = NetworkData.FALL_LINE;//掉线
                Debug.Log("break connect (getMSG)");                                      
                Debug.Log(e.ToString());
            }
        }
    }
    public void SendMSG(byte[] bytes)
    {
        if (!mySocket.Connected)//断开连接
        {
            Debug.Log("bread connect (sendMSG)");                                       
        }
        try
        {
            mySocket.Send(ByteUtil.int2ByteArray(bytes.Length), SocketFlags.None);
            mySocket.Send(bytes, SocketFlags.None);
        }
        catch (Exception e)
        {
            Debug.Log("bread connect (sendMSG_catch)");
            Debug.Log(e.ToString());                               
        }
    }

    void splitPackage(int dataLen, byte[] bdata)
    {
        //Debug.Log(dataLen + "    gameStatus=" + NetworkData.GameStatus);
        if (NetworkData.GameStatus==NetworkData.CONNECTING&&dataLen == 4)//记录客户端编号
        {
            NetworkData.clientnumber = ByteUtil.byteArray2Int(bdata, 0);
            Debug.Log("count num= " + NetworkData.clientnumber);
        }
        else if (NetworkData.clientnumber!=0&&NetworkData.GameStatus == NetworkData.CONNECTING&&dataLen == 9)//确认全部客户端进入选人界面
        {
            Debug.Log(Encoding.ASCII.GetString(bdata));
            if (Encoding.ASCII.GetString(bdata) == "<#Ready#>")
            {
                Debug.Log("get <#Ready#>");
                NetworkData.ConnectionStatus = NetworkData.ON_LINE;//更新连接状态
                NetworkData.GameStatus = NetworkData.CHOOSEPLAYER;//更新游戏状态
            }
        }
        else if (NetworkData.GameStatus == NetworkData.CHOOSEPLAYER && dataLen == 8)//接收对手选择人物编号
        {
            if (NetworkData.clientnumber == 1)//分客户端赋值
            {
                NetworkData.enemyModoIndex = ByteUtil.byteArray2Int(bdata,4);
            }
            else if (NetworkData.clientnumber == 2)
            {
                NetworkData.enemyModoIndex = ByteUtil.byteArray2Int(bdata, 0);
            }
            Debug.Log(NetworkData.clientnumber+" ChangeEnemyIndex=  " + NetworkData.enemyModoIndex);
        }else if (NetworkData.GameStatus == NetworkData.WAITGAMESTART && dataLen == 13)//收到<#LoadLevel#>
        {
            if (Encoding.ASCII.GetString(bdata) == "<#LoadLevel#>")
            {
                Debug.Log("收到<#LoadLevel#>");
                NetworkData.GameStatus = NetworkData.LOADLEVEL;//全部客户端选人结束 加载游戏场景
            }
        }else
        if (NetworkData.GameStatus == NetworkData.LOADLEVEL && dataLen == 13)//收到<#GameStart#>
        {
            if (Encoding.ASCII.GetString(bdata) == "<#GameStart#>")
            {
                NetworkData.GameStatus = NetworkData.GAMESTART;//全部客户端进入游戏关卡 游戏开始
            }
        } 
        if(NetworkData.GameStatus==NetworkData.GAMESTART&& dataLen==48 && NetworkData.ConnectionStatus==NetworkData.ON_LINE)
        {
            lock (datalock)//加锁
            {
                if (NetworkData.clientnumber == 1)
                {
                    NetworkData.enemyPosX = ByteUtil.byteArray2Float(bdata, 24);
                    NetworkData.enemyPosY = ByteUtil.byteArray2Float(bdata, 28);
                    NetworkData.enemyPosZ = ByteUtil.byteArray2Float(bdata, 32);
                    NetworkData.enemyRotX = ByteUtil.byteArray2Float(bdata, 36);
                    NetworkData.enemyRotY = ByteUtil.byteArray2Float(bdata, 40);
                    NetworkData.enemyRotZ = ByteUtil.byteArray2Float(bdata, 44);
                }
                else if (NetworkData.clientnumber == 2)
                {
                    NetworkData.enemyPosX = ByteUtil.byteArray2Float(bdata, 0);
                    NetworkData.enemyPosY = ByteUtil.byteArray2Float(bdata, 4);
                    NetworkData.enemyPosZ = ByteUtil.byteArray2Float(bdata, 8);
                    NetworkData.enemyRotX = ByteUtil.byteArray2Float(bdata, 12);
                    NetworkData.enemyRotY = ByteUtil.byteArray2Float(bdata, 16);
                    NetworkData.enemyRotZ = ByteUtil.byteArray2Float(bdata, 20);
                }
            }
        }
    }
   

    private bool isSocketConnect(Socket client)//判断客户端是否在线
    {
        bool blockingState = client.Blocking;
        try
        {
            byte[] tmp = new byte[1];
            client.Blocking = false;
            client.Send(tmp, 0, 0);
            return true;
        }
        catch (SocketException e)
        {
            if (e.NativeErrorCode.Equals(10035))
                return true;
            else
                return false;
        }
        finally {
            client.Blocking = blockingState;
        }
    }
    public void close()
    {
        getMSGFlag = false;
        mySocket.Close();
        thread.Abort();
    }
}
