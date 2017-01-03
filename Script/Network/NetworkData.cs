using UnityEngine;
using System.Collections;

public static class NetworkData {

    public static int ConnectionStatus = 0;//连接状态
    public const int ON_LINE = 1;//在线 
    public const int FALL_LINE = 2;//掉线

    public static int GameStatus = 0;//游戏状态
    public const int DISCONNECT = 0;
    public const int CONNECTING = 1;
    public const int CHOOSEPLAYER = 2;
    public const int CHOOSELEVEL = 3;
    public const int WAITGAMESTART = 4;
    public const int LOADLEVEL = 5;
    public const int GAMESTART = 6;
    public const int GAMEOVER = 7;

    public static int clientnumber = 0;//客户端编号
    public static int playerModoIndex = 0;
    public static int enemyModoIndex=0;//对手角色模型编号

    public static float playerPosX;
    public static float playerPosY;
    public static float playerPosZ;
    public static float playerRotX;
    public static float playerRotY;
    public static float playerRotZ;

    public static float enemyPosX;
    public static float enemyPosY;
    public static float enemyPosZ;
    public static float enemyRotX;
    public static float enemyRotY;
    public static float enemyRotZ;

}
