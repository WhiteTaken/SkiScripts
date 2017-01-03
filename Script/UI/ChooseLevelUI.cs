using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ChooseLevelUI : MonoBehaviour {
    private GameObject rail;
    private GameObject Level;
    private Image level1Image;
    private Image level2Image;
    private Button bQuit;
    private Button blevel1;
    private Button blevel2;
    private RectTransform level1;
    private float touchStartPOS_X = 0;
    private float touchCurrentPOS_X = 0;
    private bool touchEnd = true;
    private bool touchMove=false;
    float targetPOS = 0;
	// Use this for initialization
	void Start () {
        //autoFitScreen();//自适应屏幕
        rail = transform.Find("BackgroundRail").gameObject;
        Level = transform.Find("Level").gameObject;
        level1Image = transform.Find("Level/Level1/LevelPic1").GetComponent<Image>();
        
        level2Image = transform.Find("Level/Level2/LevelPic2").GetComponent<Image>();
        blevel1 = transform.Find("Level/Level1/BLevel1").GetComponent<Button>();
        EventTriggerListener.Get(blevel1.gameObject).onClick = OnLevel1ButtonClick;
        blevel2 = transform.Find("Level/Level2/BLevel2").GetComponent<Button>();
        EventTriggerListener.Get(blevel2.gameObject).onClick = OnLevel2ButtonClick;
        bQuit = transform.Find("BQuit").GetComponent<Button>();
        EventTriggerListener.Get(bQuit.gameObject).onClick = OnQuitButtonClick;
        level1= transform.Find("Level/Level1").GetComponent<RectTransform>();
        targetPOS = Mathf.Abs(level1.anchoredPosition.x);
        GameObject.Find("Main Camera/BackGroundMusic").GetComponent<AudioSource>().volume =
            GameData.BackGroundSoundVoiume;//设置背景音乐音量
    }
	void Update () {
        //Debug.Log(Level.GetComponent<RectTransform>().localPosition.x);
        touchMove = false;
        if (Input.touchCount != 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                touchStartPOS_X = t.position.x;
                touchEnd = false;
            }
            touchCurrentPOS_X = t.position.x;
            float disMoveX = touchStartPOS_X - touchCurrentPOS_X;
            if (Level.GetComponent<RectTransform>().localPosition.x >= -1*Screen.width / 2 && 
                Level.GetComponent<RectTransform>().localPosition.x <= Screen.width / 2)//移动两个选关按钮
            {
                Level.GetComponent<RectTransform>().localPosition += new Vector3(-1*disMoveX, 0, 0);
                Level.GetComponent<RectTransform>().localPosition = new Vector3(
                    Mathf.Clamp(Level.GetComponent<RectTransform>().localPosition.x, -1 * Screen.width / 2, Screen.width / 2),
                    Level.GetComponent<RectTransform>().localPosition.y, Level.GetComponent<RectTransform>().localPosition.z);

            }
            if (t.phase == TouchPhase.Ended)
            {
                touchEnd = true;
            }
            if (t.phase == TouchPhase.Moved)
            {
                touchMove = true;
            }
            touchStartPOS_X = touchCurrentPOS_X;//重置起始位置
        }
        
        if (touchEnd)//手指不在屏幕上
        {
            float targetX = targetPOS;//300是中心到关卡图片的距离
            if (Level.GetComponent<RectTransform>().localPosition.x < 0)//自动回到第1关
            {
                targetX = -targetPOS;
            }
            if (Mathf.Abs(Level.GetComponent<RectTransform>().localPosition.x - targetX) > 10)//10为阈值
            {
                Level.GetComponent<RectTransform>().localPosition =
                        new Vector3(Mathf.Lerp(Level.GetComponent<RectTransform>().localPosition.x, targetX, Time.deltaTime * 4),
                           Level.GetComponent<RectTransform>().localPosition.y, Level.GetComponent<RectTransform>().localPosition.z);
            }
            }
        rail.GetComponent<RectTransform>().localPosition =
            new Vector3(Level.GetComponent<RectTransform>().localPosition.x,
                        rail.GetComponent<RectTransform>().localPosition.y,
                        rail.GetComponent<RectTransform>().localPosition.z);
        float Alpha1 = 1-(targetPOS - Level.GetComponent<RectTransform>().localPosition.x)/400;//
        level1Image.color=new Color(1,1,1,Alpha1);
        level2Image.color = new Color(1,1,1,1-Alpha1);
    }
    void OnLevel1ButtonClick(GameObject b)
    {
        Debug.Log(b.name);
        if (!GameData.network&&!touchMove)
        {
            Application.LoadLevel("level1");
        }
    }
    void OnLevel2ButtonClick(GameObject b)
    {
        Debug.Log(b.name);
        if (!GameData.network&&!touchMove)
        {
            Application.LoadLevel("level2");
        }
    }
    void OnQuitButtonClick(GameObject b)
    {
        Debug.Log(b.name);
        Application.LoadLevel("StartScence");
    }
}
