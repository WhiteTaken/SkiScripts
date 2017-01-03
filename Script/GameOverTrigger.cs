using UnityEngine;
using System.Collections;

public class GameOverTrigger : MonoBehaviour {
    public Camera gameOverCamera;
    public GameObject gameUI;
    public GameObject gameOverUI;
    private bool gameOver=false;
    private GameObject player;
    public Transform endPoint;

	// Use this for initialization
	void Start () {
        gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameOver ||player==null)
        {
            return;   
        }
        if (Vector3.Distance(player.transform.position, endPoint.position) > 0.7f)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(endPoint.position.x,player.transform.position.y, endPoint.position.z), Time.deltaTime);
        }
        
        //Debug.Log(Vector3.Distance(player.transform.position, endPoint.position));

	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }
        if (GameData.network)
        {
            NetworkData.GameStatus = NetworkData.GAMEOVER;
        }
        GameData.GameOver = true;
        gameOver = true;
        Camera.main.enabled = false;//切换摄像机
        gameOverCamera.GetComponent<Camera>().enabled = true;
        gameUI.SetActive(false);//关闭GameUI
        gameOverUI.SetActive(true);//打开GameOverUI
        player = other.gameObject;
        Animator playerAnin = other.gameObject.GetComponent<Animator>();
        playerAnin.SetBool("GameOver",true);
        PlayerControl pc = other.gameObject.GetComponent<PlayerControl>();//关脚本
        pc.enabled = false;
        PlayerRotate pr = other.gameObject.GetComponent<PlayerRotate>();
        pr.enabled = false;
    }
}
