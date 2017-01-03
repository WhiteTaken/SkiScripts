using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerControl : MonoBehaviour {
	private float levelaxle=0;//水平轴分量
    private float maxVelacity = 0;
    private GameObject player;
    private Animator player_animator;
    private float dirdump = 0.25f;//方向阻尼
    //private Vector3 mapNormal = new Vector3(0, 1, 0);
    void Start()
    {
        maxVelacity = PlayerData.playerMaxSpeed[PlayerData.playerIndex]; 
        GameData.isRise = false;
    }
    void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            player_animator = player.GetComponent<Animator>();
            //player_controller = player.GetComponent<CharacterController>();
        }
        animatorControl();//角色动画控制
        if (!GameData.GameStart)//游戏尚未开始
        {
            return;
        }
        player.GetComponent<Rigidbody>().AddForce(playerSpeed());
        
    }
    void animatorControl()//动画播放控制
    {
        if (GameData.UseAcceleration)
        {
            levelaxle = Mathf.Clamp(Input.acceleration.x * 2, -1, 1);//获取重力感应x轴分量
        }
        else
        {
            levelaxle = GameData.joystickValue;
        }
        //levelaxle = Input.GetAxis("Horizontal");//使用键盘水平轴
        
        player_animator.SetFloat("Direction", levelaxle, dirdump, Time.deltaTime);//动画

        player_animator.SetBool("fall", GameData.isFall);

        player_animator.SetBool("zhuaban", GameData.isRise);

        player_animator.SetBool("Accelerate",(GameData.AcceleratePower>1));//设置加速动画
    }

    Vector3 playerSpeed()//计算玩家速度
    {
        float f = (maxVelacity - player.GetComponent<Rigidbody>().velocity.magnitude) / maxVelacity * 20*GameData.AcceleratePower;
        Vector3 playerRotation = player.transform.forward;
        //playerRotation.y = 0;
        Vector3 velacity = (playerRotation) * f;
        //Debug.Log(f);
        if ((GameData.isRise || !GameData.isGround) )
        {
            if (GameData.AcceleratePower > 1)//加速状态
            {
                velacity = Vector3.zero;
            }
            else {
                velacity = Vector3.zero;
            }
            player.GetComponent<Rigidbody>().drag = 0f;
        }
        else
        { 
            player.GetComponent<Rigidbody>().drag=2;
        }
        if (GameData.isFall)
        {
            velacity = Vector3.zero;
        }
        //Debug.Log(player.rigidbody.velocity.magnitude + "  " + velacity.magnitude);
        return velacity;
    }
}