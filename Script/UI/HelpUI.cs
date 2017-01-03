using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HelpUI : MonoBehaviour {
    private Image helpBackGround;
    private Image font;
    public Sprite[] fontPIC;
    private float timer=0;
    public Slider joysticks;
    public Image joysticksImage;
    public Image joysticksButton;
    public Image AccelerateBar;
    public Button AccelerateButton;
    private float color_red;
    public Image phoneImage;
	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(GameData.UseHelp);//是否开启帮助显示
        helpBackGround = transform.Find("HelpBackGround").GetComponent<Image>();
        font = transform.Find("Font").GetComponent<Image>();
        //Debug.Log(GameData.UseHelp);
    }
	
	// Update is called once per frame
	void Update () {
        

        if(Time.timeScale==0)
        {
            return;
        }
        if (timer >= 5 && timer < 7)//使用摇杆控制方向（重力感应）
        {
            Time.timeScale = 0.5f;
            SetImageActive(true);
            if (GameData.UseAcceleration)//显示重力感应的帮助
            {
                font.sprite = fontPIC[1]; // 
                phoneImage.gameObject.SetActive(true);
                //Debug.Log(phoneImage.GetComponent<RectTransform>().rotation);
                Quaternion ea = phoneImage.GetComponent<RectTransform>().rotation;
                ea.eulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * 60, 50) - 25);
                phoneImage.GetComponent<RectTransform>().rotation = ea;
            }
            else
            {
                font.sprite = fontPIC[0]; // 
                ChangeImageColor(joysticksImage);
                joysticksButton.color = joysticksImage.color;
            }
            
        }else if(timer>=10&&timer<13)//加速槽会随时间增加
        {
            Time.timeScale = 0.5f;
            SetImageActive(true);
            font.sprite = fontPIC[2]; // 
            ChangeImageColor(AccelerateBar);
        }
        else if (timer >= 13 && timer < 15)//可以使用加速按钮
        {
            Time.timeScale = 0.5f;
            SetImageActive(true);
            font.sprite = fontPIC[3]; // 
            ChangeImageColor(AccelerateButton.GetComponent<Image>());
        }
        else if (timer >= 18 && timer < 20)//腾空做动作吧
        {
            Time.timeScale = 0.5f;
            SetImageActive(true);
            font.sprite = fontPIC[4]; // 
        }else if(timer >= 20 && timer < 23)
        {
            Time.timeScale = 0.5f;
            SetImageActive(true);
            font.sprite = fontPIC[5]; // 
        }
        else {
            Time.timeScale = 1;
            joysticksImage.color = new Color(1, 1, 1);//重置按钮颜色
            joysticksButton.color = joysticksImage.color;
            AccelerateButton.GetComponent<Image>().color = new Color(1,1,1,1) ;
            AccelerateBar.color = new Color(0.5f,0.5f,1);
            phoneImage.gameObject.SetActive(false);
            SetImageActive(false);
        }
        if (timer > 23)
        {
            this.gameObject.SetActive(false);
        }
	}
    void ChangeImageColor(Image i)
    {
        color_red = Mathf.PingPong(Time.time * 5, 1);
        i.color = new Color(1, color_red, color_red);
    }
    void SetImageActive(bool b)
    {
        helpBackGround.gameObject.SetActive(b);//开启
        font.gameObject.SetActive(b);
    }
    void FixedUpdate()
    {
            timer += Time.fixedDeltaTime;
            //Debug.Log(timer);
    }
}
