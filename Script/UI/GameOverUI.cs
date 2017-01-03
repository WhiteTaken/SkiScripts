using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class GameOverUI : MonoBehaviour {
    private Button bReload;
    private Button bQuit;
    private Image rank;
    public Sprite[] rankImage;
    private UnityEngine.UI.Text timerText;
	// Use this for initialization
	void Start () {
        bReload = transform.Find("BReload").GetComponent<Button>();
        EventTriggerListener.Get(bReload.gameObject).onClick = OnReloadButtonClick;
        bQuit = transform.Find("BQuit").GetComponent<Button>();
        EventTriggerListener.Get(bQuit.gameObject).onClick = OnQuitButtonClick;
        timerText = transform.Find("TimerText").GetComponent<UnityEngine.UI.Text>();
        timerText.text = GameData.timer;
        rank = transform.Find("RankBoard").GetComponent<Image>();
        rank.sprite=rankImage[GameData.playerRank-1];
        //autoFitScreen();
	}
    void OnReloadButtonClick(GameObject b)
    {
        if (!GameData.network)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
    void OnQuitButtonClick(GameObject b)
    {
        Debug.Log(b.name);
        if (GameData.network)
        {
            MainMenu.sc.close();
        }
        Application.LoadLevel("StartScence");
    }
	// Update is called once per frame
	void Update () {
        
	}
}
