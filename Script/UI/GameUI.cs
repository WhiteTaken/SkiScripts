using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Text;
public class GameUI : MonoBehaviour {
    //public Sprite[] accelerateBarPIC;
    //private Image accelerateBar;
    private Button bStop;
    private Button bSound;
    private Button bQuit;
    private Button bReload;
    private Button bContinue;
    private Button bAccelerate;
    private Button bTool;
    private Slider joysticks;
    private Image panel;
    private Image accelerateCurrentImage;
    private Image accelerateButtonImage;
    private Image rank;
    private Image countDown;
    private UnityEngine.UI.Text tSpeed;
    private UnityEngine.UI.Text tTime;
    public Sprite SoundOFF;//静音图
    public Sprite SoundON;//开启音乐图
    public Sprite[] rankPIC;
    public Sprite[] accelerateButtonSprite;
    public Sprite[] countdwnNumber;
    private GameObject player;
    private GameObject Bot;
    private bool supendFlag = false;//暂停标志位
    private bool accelerateFlag = false;//加速标志位
    //private bool joystickflag = false;
    private float gameTime = 0;//游戏计时
    private float acceleratefloat = 0;//加速槽能量
    private float currentSpeed = 0;//当前速度
    private float gameStartCount=0;//游戏开始前计时器
    public AudioClip buttonClick;
    void Start()
    {
        gameStartCount = 0;
        //joystickflag = false;
        gameTime = 0;
        if (player == null)
        { 
            player=GameObject.FindGameObjectWithTag("Player");
            Bot = GameObject.FindGameObjectWithTag("BOT");
        }
        acceleratefloat = 0;//加速槽
        supendFlag = false;//暂停标志位
        accelerateFlag = false;//加速标志位
        //autoFitScreen();//自适应屏幕

        //accelerateBar =GameObject.Find("AccelerateBar").GetComponent<Image>();//进度条图片
        bStop = transform.Find("GameUIButton/BStop").GetComponent<Button>();//暂停按钮
        EventTriggerListener.Get(bStop.gameObject).onClick=OnStopButtonClick;
        bSound = transform.Find("GameUIButton/BSound").GetComponent<Button>();//声音按钮
        EventTriggerListener.Get(bSound.gameObject).onClick = OnSoundButtonClick;
        bQuit = transform.Find("SuspendUI/SuspendImage/BQuit").GetComponent<Button>();//暂停后退出按钮
        EventTriggerListener.Get(bQuit.gameObject).onClick = OnQuitButtonClick;
        bReload = transform.Find("SuspendUI/SuspendImage/BReload").GetComponent<Button>();//重新开始按钮
        EventTriggerListener.Get(bReload.gameObject).onClick = OnReloadButtonClick;
        bContinue = transform.Find("SuspendUI/SuspendImage/BContinue").GetComponent<Button>();//继续游戏按钮
        EventTriggerListener.Get(bContinue.gameObject).onClick = OnContinueButtonClick;
        bAccelerate = transform.Find("GameUIButton/BAccelerate").GetComponent<Button>();//加速按钮
        EventTriggerListener.Get(bAccelerate.gameObject).onClick = OnAccelerateButtonClick;
        bTool = transform.Find("GameUIButton/BTool").GetComponent<Button>();//使用道具按钮
        EventTriggerListener.Get(bTool.gameObject).onClick = OnToolButtonClick;
        rank = transform.Find("Rank").GetComponent<Image>();
        joysticks = transform.Find("joysticks").GetComponent<Slider>();
        if (GameData.UseAcceleration)
        {
            joysticks.gameObject.SetActive(false);
        }
        else {
            EventTriggerListener.Get(joysticks.gameObject).onDown = OnjoystickDown;
            EventTriggerListener.Get(joysticks.gameObject).onUp = OnjoystickUp;
        }
        
        accelerateCurrentImage = transform.Find("GameUIButton/BAccelerate/AccelerateCurrentImage").GetComponent<Image>();
        accelerateButtonImage = transform.Find("GameUIButton/BAccelerate/AccelerateButtonImage").GetComponent<Image>();
        tSpeed = transform.Find("SpeedText").GetComponent<UnityEngine.UI.Text>();//获取显示速度文本栏
        tTime = transform.Find("TimeText").GetComponent<UnityEngine.UI.Text>();//获取显示速度文本栏
        countDown = transform.Find("CountDown").GetComponent<Image>();
        AwakeSupendUI();//初始化暂停界面
        if (GameData.network)
        { 
            MainMenu.sc.SendMSG(Encoding.ASCII.GetBytes("<#GameStart#>"));
        }
        
    }
    void Update()
    {
        if (GameData.GameOver)//游戏结束
        {
            return;
        }
        if (GameData.GameStart)//游戏已经开始
        {
            
            gameTime += Time.deltaTime;
            tSpeed.text = UpdateSpeed();//速度板赋值
            tTime.text = UpdateTime();//时间板赋值
            Accelerate();
            if (!GameData.UseAcceleration)
            {
                GameData.joystickValue = (joysticks.value - 0.5f) * 2;
            }
            CalculateRank();//计算排名
            return;
        }
        //游戏尚未开始
        if (GameData.network&&NetworkData.GameStatus!=NetworkData.GAMESTART)//若是网络模式且客户端没有进入游戏
        {
            return;
        }
        gameStartCount+=Time.deltaTime;
        if (gameStartCount > 2.5f)//多0.5s后关闭
        {
            GameData.GameStart = true;
            countDown.gameObject.SetActive(false);//关闭计时板
            return;
        }
        countDown.sprite = countdwnNumber[(int)gameStartCount];
    }
    void OnjoystickDown(GameObject b)
    {
        //joystickflag = true;
    }
    void OnjoystickUp(GameObject b)
    {
        b.GetComponent<Slider>().value = 0.5f;
        //joystickflag = false;
    }
    void Accelerate()//加速
    {
        
        int accelerateBar = (int)(acceleratefloat / 0.33f);//当前加速槽数目 0-3
        float acceleateDump = 0.13f;//加速槽积累量

        MotionBlur mb = Camera.main.GetComponent<MotionBlur>();
        
        if (accelerateFlag)//加速
        {
            mb.enabled = true;
            acceleateDump = -0.2f;
            GameData.AcceleratePower = acceleratefloat + 3;//加速力
            if (acceleratefloat <= 0.01f)
            {
                accelerateFlag = false;
            }
        }else
        {
            mb.enabled = false;
            GameData.AcceleratePower = 1;
        }
        acceleratefloat = Mathf.Clamp(acceleratefloat + Time.deltaTime * acceleateDump, 0, 1);//平时缓慢增加加速槽
        accelerateCurrentImage.fillAmount = acceleratefloat;//设置加速槽进度
        int index=Mathf.Clamp(accelerateBar-1,0,2);
        //Debug.Log(accelerateBar);
        accelerateButtonImage.sprite=accelerateButtonSprite[index];//切换纹理图
    }
    string UpdateTime()//更新时间显示字符串
    {
        string time="--";
        float t= gameTime;
        int minute = (int)t / 60;
        int second = (int)t - minute * 60;
        time = (minute < 10 ? "0" : "") + minute + ":" + (second < 10 ? "0" : "") + second;
        GameData.timer = time;
        return time;
    }
    string UpdateSpeed()//更新速度显示字符串
    {
        currentSpeed = Mathf.Lerp(currentSpeed, player.GetComponent<Rigidbody>().velocity.magnitude * 4, Time.deltaTime);
        string speed = (int)currentSpeed+ " km/h";
        //Debug.Log((int)player.rigidbody.velocity.magnitude * 4);
        return speed;
    }
    void OnToolButtonClick(GameObject b)//道具按钮
    {
        Debug.Log("Btool");
    }
    void OnAccelerateButtonClick(GameObject b)//加速按钮
    {
        if (acceleratefloat > 0.33f)//加速力大于一格
        {
            accelerateFlag = true;
        }
        
        Debug.Log("AccelerateButton");
    }
    void OnQuitButtonClick(GameObject b)//退出游戏按钮
    {
        Debug.Log("quit");
        Time.timeScale = 1;
        Application.LoadLevel("StartScence");
    }
    void OnReloadButtonClick(GameObject b)//重新开始按钮
    {
        Debug.Log("Reload");
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevelName);
    }
    void OnContinueButtonClick(GameObject b)//继续游戏按钮
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick,GameData.GameEventVoiume);
        supendFlag = false;
        Time.timeScale = 1;
        AwakeSupendUI();
        Debug.Log("Continue");
    }
    void OnStopButtonClick(GameObject b)//暂停按钮监听
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick,GameData.GameEventVoiume);
        if (GameData.network)
        {
            return;
        }
        supendFlag = true;
        Debug.Log("stop");
        Time.timeScale = 0;
        AwakeSupendUI();
    }
    void OnSoundButtonClick(GameObject b)//静音按钮监听
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick, GameData.GameEventVoiume);
        GameData.SoundONOrOFF = !GameData.SoundONOrOFF;
        b.GetComponent<Image>().sprite = GameData.SoundONOrOFF ? SoundON : SoundOFF;
        GameObject.Find("Main Camera/BackGroundMusic").GetComponent<AudioSource>().volume //设置背景音乐的开关
            = GameData.SoundONOrOFF ? GameData.BackGroundSoundVoiume : 0;
    }
    void CalculateRank()//计算玩家排名
    {
        Vector3 PlayertoBot = player.transform.position - Bot.transform.position;
        float tempRank = Vector3.Dot(PlayertoBot,GameData.currentMapDirection);
        if (tempRank > 0)//玩家第一
        {
            rank.sprite = rankPIC[0];
            GameData.playerRank = 1;//玩家第一
        }
        else
        {
            rank.sprite = rankPIC[1];
            GameData.playerRank = 2;//玩家第二
        }
    }
    void autoFitScreen()//自适应屏幕
    {
        //编辑窗口的分辨率，在该分辨率下的窗口中将UI布局调整至最佳效果，在本方法中进行与其他分辨率屏幕的匹配
        Vector2 editScreen = new Vector2(811, 475);
        Transform canvas = GameObject.Find("GameUI").transform;         //在Canvas下的对象将进行位置和大小的调整
        Vector2 scaleExchange = new Vector2(Screen.width / editScreen.x, Screen.height / editScreen.y);
        for (int i = 0; i < canvas.childCount; i++)
        {
            RectTransform canvasChildRT = canvas.GetChild(i).GetComponent<RectTransform>();
            canvasChildRT.position = new Vector3(scaleExchange.x * canvasChildRT.position.x,    //调整其位置
                                                 scaleExchange.y * canvasChildRT.position.y,
                                                 0);
            canvasChildRT.localScale = new Vector3(scaleExchange.x * canvasChildRT.localScale.x,    //调整其缩放比
                                                   scaleExchange.y * canvasChildRT.localScale.y,
                                                   1);
        }
    }
    void AwakeSupendUI()//暂停游戏界面
    {
        panel = transform.Find("SuspendUI").GetComponent<Image>();
        panel.gameObject.SetActive(supendFlag);
    }
}
