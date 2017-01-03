using UnityEngine;
using System.Collections;

public class PlayerRotate : MonoBehaviour {
    //Quaternion originalRotation;
    float levelaxle;
    float rotateAngle = 0;
    public float min = -50;
    public float max = 50;
    //private float currentAngle=0;
    private float timer=0;
    private bool btimer = false;
    private TrailRenderer trailRender;
    private float rotateDump=1;
    //public GameObject curve1;
	void Start () {
        rotateAngle = 0;
        //currentAngle = 0;
        min = -50;
        max = 50;
        trailRender = GetComponentInChildren<TrailRenderer>();
        rotateDump = PlayerData.playerControl[PlayerData.playerIndex];
        //originalRotation = transform.rotation;
	}
    void FixedUpdate()
    {
        if(!GameData.GameStart)
        {
            return;
        }
        Quaternion playerUPTOMapNormal = FindAngle();
        setAngleLimit();//设置旋转角度限制
        //levelaxle = (GameData.isRise) ? 0 : Input.GetAxis("Horizontal");//获取键盘水平轴 若是当前浮空触控无效

        if (GameData.UseAcceleration)
        {
            levelaxle = (GameData.isRise) ? 0 : Mathf.Clamp(Input.acceleration.x * 2, -1, 1);
        }
        else
        {
            levelaxle = (GameData.isRise) ? 0 : GameData.joystickValue;
        }//levelaxle = (GameData.isRise) ? 0 : Mathf.Clamp(Input.acceleration.x * 2, -1, 1);
        
        
        float correctionAngle = autoCorrectionAngle(levelaxle);
        rotateAngle += correctionAngle*rotateDump;
        rotateAngle = clampAngle(rotateAngle, min, max);//旋转角度限制
        Quaternion rotate = Quaternion.AngleAxis(rotateAngle, Vector3.up);
        transform.rotation = playerUPTOMapNormal * rotate;
        if (btimer)//出界时间 防止误判定
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }
    float autoCorrectionAngle(float axis)
    {
        if(axis!=0||GameData.currentMapNormal==Vector3.up)//发生操控或者在平地上
        {
            return axis;
        }
        float correctionAngle = 0;
        Vector3 tempAxis = Vector3.Cross(GameData.currentMapNormal,Vector3.up);
        float tempLeftORRight = Vector3.Dot(transform.forward, tempAxis);
        if (Mathf.Abs(tempLeftORRight) > 0.2f)//自动修正
        {
            correctionAngle = tempLeftORRight;
        }
        //Debug.Log(correctionAngle);
        return correctionAngle;
    }

    void setAngleLimit()//旋转角度限制
    {
        if (!GameData.calculateMapDirection)
        {
            return;
        }
        Vector3 dir = GameData.currentMapDirection;
        float rotateAngle = Vector3.Angle(Vector3.forward, dir);//角色前方和赛道方向夹角
        max = 50 + rotateAngle;
        min = -50 + rotateAngle;
        //Debug.Log(min+"  "+max);
    }

    float clampAngle(float angle, float min, float max)
    {
        if (angle > 360)
        {
            angle -= 360;
        }
        if (angle < -360)
        {
            angle += 360;
        }
        return Mathf.Clamp(angle,min,max);
    }

    Quaternion FindAngle()//根据地图修正角度
    {
        Quaternion playerUPTOMapNormal=Quaternion.identity;
        Ray ray = new Ray(transform.position,transform.up*-1);//声明一条射线沿着角色的Y轴向下
        RaycastHit hit;
        bool raycastFlag = false;
        if (Physics.Raycast(ray, out hit))//获取碰撞点
        {
            ChangePlayerState(hit);//设置游戏状态
            raycastFlag = true;
        }
        if (raycastFlag&&hit.collider.tag != "Map")
        {
            Vector3 lerpUPTONormal = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 4);
            playerUPTOMapNormal = Quaternion.FromToRotation(Vector3.up, lerpUPTONormal);
        }
        else
        {
            //Debug.Log();
            playerUPTOMapNormal = Quaternion.FromToRotation(Vector3.up, transform.up);
        }
        
        return playerUPTOMapNormal;
    }
    void ChangePlayerState(RaycastHit hit)//设置游戏状态
    {
        if (!GameData.isRise)
        {
            trailRender.startWidth = 0.65f + Mathf.Abs(levelaxle * 0.2f);
        }
        bool isground = ((hit.point - transform.position).magnitude <= 0.5f);//距离碰撞点距离（《0.5为模型在碰撞体表面）
        GameData.isGround = isground;
        Ray rayDown = new Ray(transform.position,Vector3.up*-1);
        RaycastHit rayDownHit;
        if (Physics.Raycast(rayDown, out rayDownHit))
        {
            //Debug.Log(hit.collider.name+" "+rayDownHit.collider.name + "  " + (rayDownHit.point - transform.position).magnitude);
            if ((rayDownHit.point - transform.position).magnitude > 1)
            {
                GameData.isRise = true;
                trailRender.startWidth = 0;
                trailRender.endWidth = 0;
            }
            else
            {
                GameData.isRise = false;
                //trailRender.renderer.enabled = true;
            }
        }

        if (rayDownHit.collider.tag == "Map")
        {
            btimer = true;
            if (timer > 0.2f)
            {
                GameData.isFall = true;       //判定为角色出界     
            }
        }
        else
        {
            btimer = false;
        }

        if (hit.collider.tag != "Map" && rayDownHit.collider.tag!="Map")//未出界 碰撞点距离模型中心<0.5
        {
            GameData.currentMapNormal = hit.normal;
        }
        //GameData.isRise = (hit.collider.tag == "RiseCollider");
        //Debug.Log(GameData.isRise);
    }
}
