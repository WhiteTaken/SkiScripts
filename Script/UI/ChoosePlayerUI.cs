using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ChoosePlayerUI : MonoBehaviour {
    private Button bBack;
    private Button bEnter;
    private Button[] player=new Button[9];
	public  RenderTexture rt;
    public GameObject[] playerModo;
    private int lastChooseIndex = PlayerData.playerIndex;
    private Slider maxSpeed;
    private Slider rotateDump;
    private Image playerFrame;
    private int choosePlayerIndex;
    public AudioClip buttonClick;
	void Awake()
	{
		transform.Find("Preview/PlayerImage").GetComponent<RawImage>().texture=rt;//设置rawtexture的显示图像
	}
	void Start () {
        //autoFitScreen();
        for (int i = 0; i < player.Length; i++)//查找按钮并添加监听
        {
            player[i] = transform.Find("choose/player"+(i+1)).GetComponent<Button>();
            EventTriggerListener.Get(player[i].gameObject).onClick = playerButton;
        }
        maxSpeed = transform.Find("Preview/MaxSpeed").GetComponent<Slider>();
        rotateDump = transform.Find("Preview/RotateDump").GetComponent<Slider>();
        playerFrame = transform.Find("choose/PlayerFrame").GetComponent<Image>();
        bBack = transform.Find("BackButton").GetComponent<Button>();
        EventTriggerListener.Get(bBack.gameObject).onClick = OnBackButtonClick;
        bEnter = transform.Find("EnterButton").GetComponent<Button>();
        EventTriggerListener.Get(bEnter.gameObject).onClick = OnEnterButtonClick;
        playerButton(player[PlayerData.playerIndex].gameObject);
        
        
	}
    void OnBackButtonClick(GameObject b)
    {
        Debug.Log("bback");
        Application.LoadLevel("StartScence");
    }
    void OnEnterButtonClick(GameObject b)
    {
        Debug.Log(choosePlayerIndex);
        PlayerData.playerIndex = choosePlayerIndex;
        Application.LoadLevel("ChooseLevel");
    }
    void playerButton(GameObject b)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick, GameData.GameEventVoiume);
        Button btemp=b.GetComponent<Button>();
        int index = 0;
        while (index<10)
        {
            if (player[index] == btemp)
            {
                break;
            }
            index++;
        }
        choosePlayerIndex = index;
        playerModo[choosePlayerIndex].SetActive(true);
        playerModo[choosePlayerIndex].transform.position = Vector3.zero;//将非选中的模型移出视口
        if (index != lastChooseIndex)//和上次点的不是一个人
        {
            playerModo[lastChooseIndex].transform.position = new Vector3(-5, 0, 0);//将非选中的模型移出视口
            playerModo[lastChooseIndex].SetActive(false);
        }
        int animIndex = ((int)Random.Range(1, 10));
        playerModo[choosePlayerIndex].GetComponent<Animator>().SetInteger("AnimIndex", animIndex);
        PlayerData.playerIndex = index;
        maxSpeed.value=(PlayerData.playerMaxSpeed[index])/60;
        rotateDump.value = (PlayerData.playerControl[index]) / 2;
        playerFrame.GetComponent<RectTransform>().position=b.GetComponent<RectTransform>().position;

        lastChooseIndex = index;
    }
	void Update () {
	}
    void autoFitScreen()//自适应屏幕
    {
        //编辑窗口的分辨率，在该分辨率下的窗口中将UI布局调整至最佳效果，在本方法中进行与其他分辨率屏幕的匹配
        Vector2 editScreen = new Vector2(811, 475);
        Transform canvas = GameObject.Find("ChooseUI").transform;         //在Canvas下的对象将进行位置和大小的调整
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
