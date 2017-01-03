using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class MainMenu : MonoBehaviour {
    private Button bOption;
    private Button bSound;
    private Button bNetwork;
    private Button bPlay;
    private Button bChoose;
    private Button bQuit;
    private Image topImage1;
    private Image topImage2;
    private Image bSoundImage;
    private Image topFrame;
    private Image buttomFrame;
    public static SocketConnect sc;
    public AudioClip buttonClick;//按键声音
    private Toggle useHelp;
    private Toggle useGravitySensor;//使用重力感应
    private GameObject optionUI;//设置界面
    private GameObject mainMenu;//游戏主菜单界面
    private Slider setBGSoundVolime;//设置背景音乐滑块
    private Slider setGameEventSoundVolime;//设置游戏事件音效滑块
    private InputField setIP;
    private Button optionQuit;
    public Sprite[] soundONorOFFPIC;
	// Use this for initialization
	void Start () {
        //autoFitScreen();
        bOption = transform.Find("MainMenu/TopUI/BOption").GetComponent<Button>();
        EventTriggerListener.Get(bOption.gameObject).onClick = OnOptionButtonClick;
        bSound = transform.Find("MainMenu/TopUI/BSoundONorOFF").GetComponent<Button>();
        EventTriggerListener.Get(bSound.gameObject).onClick = OnSoundButtonClick;
        bNetwork = transform.Find("MainMenu/ButtomUI/BNetwork").GetComponent<Button>();
        EventTriggerListener.Get(bNetwork.gameObject).onClick = OnNetworkButtonClick;
        bPlay = transform.Find("MainMenu/ButtomUI/BPlay").GetComponent<Button>();
        EventTriggerListener.Get(bPlay.gameObject).onClick = OnPlayButtonClick;
        bChoose = transform.Find("MainMenu/ButtomUI/BChoose").GetComponent<Button>();
        EventTriggerListener.Get(bChoose.gameObject).onClick = OnChooseButtonClick;
        bQuit = transform.Find("MainMenu/QuitButton/BQuit").GetComponent<Button>();
        EventTriggerListener.Get(bQuit.gameObject).onClick = OnQuitButtonClick;
        useHelp = transform.Find("OptionUI/UseHelp/Toggle").GetComponent<Toggle>();//开启帮助
        useHelp.isOn = GameData.UseHelp;
        useHelp.onValueChanged.AddListener(OnUseHelpChange);
        useGravitySensor = transform.Find("OptionUI/UseGravitySensor/Toggle").GetComponent<Toggle>();//设置界面的退出按钮
        useGravitySensor.isOn = GameData.UseAcceleration;//
        useGravitySensor.onValueChanged.AddListener(OnUseGravitySensorChange);
        optionQuit = transform.Find("OptionUI/QuitButton/BQuit").GetComponent<Button>();
        EventTriggerListener.Get(optionQuit.gameObject).onClick = OnOptionQuitButtonClick;
        setBGSoundVolime = transform.Find("OptionUI/SetBGSoundVolime/Slider").GetComponent<Slider>();//游戏背景音滑块
        setBGSoundVolime.onValueChanged.AddListener(OnBGSoundVolimeSliderChange);
        setGameEventSoundVolime = transform.Find("OptionUI/SetGameEventSoundVolime/Slider").GetComponent<Slider>();//游戏事件音效滑块
        setGameEventSoundVolime.onValueChanged.AddListener(OnGameEventSoundVolimeSliderChange);
        setIP = transform.Find("OptionUI/SetIP/InputField").GetComponent<InputField>();//获取IP地址组件
        setIP.text = PlayerPrefs.GetString("ip");//记录下玩家的设置 下次开启时使用它
        setIP.onValueChange.AddListener(OnIPInputFieldChange);
        optionUI = transform.Find("OptionUI").gameObject;
        mainMenu = transform.Find("MainMenu").gameObject;
        setBGSoundVolime.value = GameData.BackGroundSoundVoiume;//设置两个滑块的初始值
        setGameEventSoundVolime.value = GameData.GameEventVoiume;
        //Debug.Log(useHelp.name);
	}

    private void OnIPInputFieldChange(string arg0)
    {
        PlayerPrefs.SetString("ip", arg0);
        Debug.Log(arg0);
    }

    void OnIPInputFieldChange()
    {
            
    }
	// Update is called once per frame
	void FixedUpdate () {
        
    }
    void OnBGSoundVolimeSliderChange(float value)//游戏背景音乐音量滑块
    {
        //Debug.Log(GameObject.Find("Main Camera/BackGroundMusic").name);
        GameData.BackGroundSoundVoiume = value;
        GameObject.Find("Main Camera/BackGroundMusic").GetComponent<AudioSource>().volume = value;
    }
    void OnGameEventSoundVolimeSliderChange(float value)//游戏事件音效音量滑块
    {
        //Debug.Log(value);
        GameData.GameEventVoiume=value;
    }
    void OnUseHelpChange(bool check)//启用帮助开关
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick, GameData.GameEventVoiume);
        GameData.UseHelp = check;
    }
    void OnUseGravitySensorChange(bool check)//启用重力感应
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick, GameData.GameEventVoiume);
        GameData.UseAcceleration = check;
    }
    void OnOptionQuitButtonClick(GameObject b)//设置按钮监听
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick,GameData.GameEventVoiume);
        optionUI.SetActive(false);
        mainMenu.SetActive(true);
    }
    void OnQuitButtonClick(GameObject b)
    {
        Debug.Log(bQuit.name);
        Application.Quit();
    }
    void OnOptionButtonClick(GameObject b)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick, GameData.GameEventVoiume);
        optionUI.SetActive(true);
        mainMenu.SetActive(false);
        //Debug.Log("bOption");
    }
    void OnSoundButtonClick(GameObject b)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(buttonClick, GameData.GameEventVoiume);//播放按钮声音
        GameData.SoundONOrOFF = !GameData.SoundONOrOFF;//设置静音标志位
        bSound.transform.Find("BSoundImage").GetComponent<Image>().sprite//按钮换图
            = soundONorOFFPIC[GameData.SoundONOrOFF?0:1];
        GameObject.Find("Main Camera/BackGroundMusic").GetComponent<AudioSource>().volume //设置背景音乐的开关
            = GameData.SoundONOrOFF?GameData.BackGroundSoundVoiume:0;
        setBGSoundVolime.interactable = GameData.SoundONOrOFF;
    }
    void OnNetworkButtonClick(GameObject b)
    {
        Debug.Log("Network");
        if (sc == null)//创建网络连接
        {
            string ip = setIP.text;
            sc = SocketConnect.getSocketInstance(ip);
            byte[] bconnect = Encoding.ASCII.GetBytes("<#CONNECT#>");
            sc.SendMSG(bconnect);
            Debug.Log("send <#CONNECT#>");
            GameData.network = true;
            NetworkData.GameStatus = NetworkData.CONNECTING;//设置为正在连接状态
            Application.LoadLevel("ChoosePlayer");
        }
    }
    void OnPlayButtonClick(GameObject b)//单人游戏
    {
        Debug.Log("Play");
        GameData.network = false;
        Application.LoadLevel("ChooseLevel");
    }
    void OnChooseButtonClick(GameObject b)
    {

        Debug.Log("Choose");
        GameData.network = false;
        Application.LoadLevel("ChoosePlayer");
    }
}
