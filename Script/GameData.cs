using UnityEngine;
using System.Collections;

public static  class GameData{
    public static float BackGroundSoundVoiume=1;
    public static float GameEventVoiume = 1;
    public static Vector3 currentMapNormal = new Vector3(0, 1, 0);
    public static Vector3 nextMapNormal = new Vector3(0,1,0);
    public static Vector3 currentMapDirection = Vector3.forward;
    public static bool isRise=false;
    public static bool calculateMapDirection = false;//计算赛道方向标志位
    public static int CurveNumber = 2;//当前关卡弯道数量
    public static int currentCurveNumber = 0;//当前过弯数量
    public static bool isGround = false;//在地上
    public static bool isFall = false;//摔倒
    //public static float AccelerateProgress=0;//加速槽
    public static float AcceleratePower = 1;//加速力
    public static bool SoundONOrOFF=true;
    public static bool UseAcceleration = false;
    public static bool UseHelp =false;
    public static float joystickValue = 0;
    public static bool GameOver = false;
    public static bool GameStart = false;
    public static bool network=false;//网络游戏
    public static string timer;
    public static int playerRank = 1;
    //public static bool accelerateFlag=true;
}
