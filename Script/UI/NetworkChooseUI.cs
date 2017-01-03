using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Text;
public class NetworkChooseUI : MonoBehaviour {
    private Button[] player=new Button[9];
    private Button bBack;
    private Button bEnter;
    public RenderTexture rt1;
    public RenderTexture rt2;
    public GameObject[] playerModo;
    public GameObject[] enemyModo;
    private Image playerFrame;
    private Image backGround;
    private int choosePlayerIndex=-1;
    private int lastChooseIndex ;//上一次选择人物索引
    private int lastenemyIndex;//上一次另一个客户端选择人物索引
    private GameObject netUI;
    private GameObject waitUI;
    private GameObject outCircular;
    private GameObject inCircular;
    private UnityEngine.UI.Text connectInformation;
	// Use this for initialization
    void Awake()
    {
        transform.Find("NetUI/playerImage").GetComponent<RawImage>().texture = rt1;
        transform.Find("NetUI/enemyImage").GetComponent<RawImage>().texture = rt2;
    }
    void Start () {
        //autoFitScreen();
        

        for (int i = 0; i < player.Length; i++)//查找按钮并添加监听
        {
            player[i] = transform.Find("NetUI/choose/player" + (i + 1)).GetComponent<Button>();
            EventTriggerListener.Get(player[i].gameObject).onClick = OnPlayerButtonClick;
        }
        backGround = transform.Find("BackGround").GetComponent<Image>();
        playerFrame = transform.Find("NetUI/choose/Playerframe").GetComponent<Image>();
        bBack = transform.Find("NetUI/BackButton").GetComponent<Button>();
        EventTriggerListener.Get(bBack.gameObject).onClick = OnBackButtonClick;
        bEnter = transform.Find("NetUI/EnterButton").GetComponent<Button>();
        EventTriggerListener.Get(bEnter.gameObject).onClick = OnEnterButtonClick;
        netUI = transform.Find("NetUI").gameObject;
        waitUI = transform.Find("WaitUI").gameObject;
        
        outCircular = transform.Find("WaitUI/OutCircular").gameObject;
        inCircular = transform.Find("WaitUI/InCircular").gameObject;
        connectInformation=transform.Find("WaitUI/Text").GetComponent<UnityEngine.UI.Text>();
        OnPlayerButtonClick(player[NetworkData.playerModoIndex].gameObject);//默认点击一次
        
        
        lastChooseIndex = NetworkData.playerModoIndex;
        lastenemyIndex = -1;
	}
	
	void Update () {
        if (NetworkData.enemyModoIndex != lastenemyIndex)//更换敌人模型
        {
            ChangeEnemyModo(NetworkData.enemyModoIndex);
        }
        if (NetworkData.GameStatus == NetworkData.CONNECTING)//还在连接中
        {
            waitUI.SetActive(true);
            outCircular.transform.Rotate(Vector3.forward, -15 * Time.deltaTime);//旋转
            inCircular.transform.Rotate(Vector3.forward, 15 * Time.deltaTime);
            connectInformation.text = "等待其他玩家进入游戏房间...";
            backGround.color = new Color(0.3f,0.3f,0.3f,1);
        }
        if (NetworkData.GameStatus == NetworkData.CHOOSEPLAYER)
        {
            waitUI.SetActive(false);
            netUI.SetActive(true);
            backGround.color = new Color(1,1,1,1);
        }else
        if (NetworkData.GameStatus == NetworkData.WAITGAMESTART)//等待确认界面
        {
            waitUI.SetActive(true);
            netUI.SetActive(false);
            backGround.color = new Color(0.3f, 0.3f, 0.3f, 1);
            connectInformation.text = "等待其他玩家确认选择...";
            outCircular.transform.Rotate(Vector3.forward, -15 * Time.deltaTime);//旋转
            inCircular.transform.Rotate(Vector3.forward, 15 * Time.deltaTime);
        }else 
        if(NetworkData.GameStatus == NetworkData.LOADLEVEL)//服务器允许加载游戏场景
        {
            Debug.Log("all client already, begin load level");
            backGround.color = new Color(1,0,0,1);
            Application.LoadLevel("level2");
        }
	}
    void OnBackButtonClick(GameObject b)
    {
        Debug.Log(b.name);
        GameData.network = false;

        NetworkData.GameStatus = NetworkData.DISCONNECT;
        Application.LoadLevel("StartScence");//回到主界面
    }
    void OnEnterButtonClick(GameObject b)//确定按钮
    { 
        Debug.Log(b.name);
        NetworkData.GameStatus = NetworkData.WAITGAMESTART;//切换游戏状态
        byte[] bsend = Encoding.ASCII.GetBytes("<#LoadLevel#>");//发送信息
        MainMenu.sc.SendMSG(bsend);
        //Application.LoadLevel("level1");
    }
    public void ChangeEnemyModo(int index)//切换对手模型
    {
        enemyModo[index].SetActive(true);
        enemyModo[index].transform.position = new Vector3(-5, 0, 0);
        int animIndex = ((int)Random.Range(1, 10));
        enemyModo[index].GetComponent<Animator>().SetInteger("AnimIndex", animIndex);
        if (index != lastenemyIndex && lastenemyIndex!=-1)
        {
            enemyModo[lastenemyIndex].transform.position = new Vector3(-10,0,0);
            enemyModo[lastenemyIndex].SetActive(false);
        }
        lastenemyIndex = index;
        NetworkData.enemyModoIndex = index;
    }
    void OnPlayerButtonClick(GameObject b)//9个角色按钮
    {
        Button btemp = b.GetComponent<Button>();
        int index = 0;
        while (index < 10)//查找点到的是具体哪个按钮
        {
            if (player[index] == btemp)
            {
                break;
            }
            index++;
        }
        choosePlayerIndex = index;
        playerModo[choosePlayerIndex].SetActive(true);
        playerModo[choosePlayerIndex].transform.position = Vector3.zero;
        int animIndex = ((int)Random.Range(1, 10));
        playerModo[choosePlayerIndex].GetComponent<Animator>().SetInteger("AnimIndex", animIndex);
        if (choosePlayerIndex != lastChooseIndex)//和上次触摸的不是一个人物
        {
            //Debug.Log(lastChooseIndex);
            playerModo[lastChooseIndex].transform.position = new Vector3(-10, 0, 0);
            playerModo[lastChooseIndex].SetActive(false);
        }
        playerFrame.transform.position = b.transform.position;
        lastChooseIndex = choosePlayerIndex;
        NetworkData.playerModoIndex = choosePlayerIndex;
        SendChooseResult();
    }
    void SendChooseResult()
    {
        byte[] bplayerIndex = ByteUtil.int2ByteArray(choosePlayerIndex);
        MainMenu.sc.SendMSG(bplayerIndex);
    }
    void autoFitScreen()//自适应屏幕
    {
        //编辑窗口的分辨率，在该分辨率下的窗口中将UI布局调整至最佳效果，在本方法中进行与其他分辨率屏幕的匹配
        Vector2 editScreen = new Vector2(811, 475);
        Transform canvas = GameObject.Find("NetWorkChooseUI").transform;         //在Canvas下的对象将进行位置和大小的调整
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
}
