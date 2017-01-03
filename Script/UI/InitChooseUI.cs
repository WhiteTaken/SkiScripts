using UnityEngine;
using System.Collections;

public class InitChooseUI : MonoBehaviour {
    public GameObject chooseUI;
    public GameObject NetworkchooseUI;

	void Start () {

        if (!GameData.network)
        {
            NetworkchooseUI.SetActive(false);
            chooseUI.SetActive(true);
        }
        else
        {
            NetworkchooseUI.SetActive(true);
            chooseUI.SetActive(false);
        }
        GameObject.Find("Main Camera/BackGroundMusic").GetComponent<AudioSource>().volume = GameData.BackGroundSoundVoiume;//设置背景音乐音量

	}
    void OnApplicationQuit()
    {
        if (GameData.network)
            MainMenu.sc.close();
    }

    void Update () {

	}
}
