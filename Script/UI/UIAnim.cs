using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class UIAnim : MonoBehaviour {
    //private Tweener m_pos;
    private GameObject topUI;
    private GameObject quitbutton;

    private Image topFrame;
    private Image buttomFrame;
    private Image bNetwork;
    private Image bChoose;
    private Image bPlay;
	// Use this for initialization
	void Start () {
        Vector2 scaleDivisor= getScaleDivisor();
        topUI = transform.Find("MainMenu/TopUI").gameObject;
        quitbutton = transform.Find("MainMenu/QuitButton").gameObject;
        topFrame = transform.Find("MainMenu/ButtomUI/TopFrame").GetComponent<Image>();
        buttomFrame = transform.Find("MainMenu/ButtomUI/ButtomFrame").GetComponent<Image>();
        bNetwork = transform.Find("MainMenu/ButtomUI/BNetwork").GetComponent<Image>();
        bChoose = transform.Find("MainMenu/ButtomUI/BChoose").GetComponent<Image>();
        bPlay = transform.Find("MainMenu/ButtomUI/BPlay").GetComponent<Image>();
        topUI.transform.DOMoveX(690 * scaleDivisor.x, 1);
        topFrame.rectTransform.DOMoveX(338 * scaleDivisor.x, 1);
        buttomFrame.rectTransform.DOMoveX(474 * scaleDivisor.x,1);
        bNetwork.rectTransform.DOMoveX(251 * scaleDivisor.x, 1);
        bChoose.rectTransform.DOMoveX(561 * scaleDivisor.x, 1);
        bPlay.rectTransform.DOMoveY(101*scaleDivisor.y,1);
        quitbutton.transform.DOMoveX(406*scaleDivisor.x,1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    Vector2 getScaleDivisor()//计算缩放因子
    {
        Vector2 defaultScreen = new Vector2(1280,720);
        Vector2 scaleDivisor = new Vector2(Screen.width / defaultScreen.x, Screen.height / defaultScreen.y);
        return scaleDivisor;
    }
}
