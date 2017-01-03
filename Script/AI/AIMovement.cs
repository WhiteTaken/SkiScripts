using UnityEngine;  
using System.Collections;  
  
public class AIMovement : MonoBehaviour  
{
    private PathNode currentEndPos;
    public float distMin = 4.0f;
    private NavMeshAgent agent;
    public float speed = 10;
    void Start()  
    {  
        //获取组件  
        agent = GetComponent<NavMeshAgent>();
        currentEndPos = GameObject.Find("RoadNode/node0").GetComponent<PathNode>();
    }  
  
  
    void Update()  
    {
        if (!GameData.GameStart)
        {
            return;
        }
        MoveTo(); 
    }
  
    void MoveTo(){
        Vector3 pos1 = this.transform.position;
        Vector3 pos2 = currentEndPos.transform.position;

        //transform.LookAt(new Vector3(pos2.x, transform.position.y, pos2.z)); 
        Vector3 targetPos = new Vector3(pos2.x, transform.position.y, pos2.z);
        Vector3 targetDir = targetPos - transform.position;
        Vector3 currentRight = Vector3.Lerp(transform.forward, targetDir, Time.deltaTime * 0.1f);
        Vector3 rotateAxis = Vector3.Cross(transform.forward, currentRight).normalized;
        float rotateAngle = Vector3.Angle(transform.forward, currentRight);
        transform.Rotate(rotateAxis, rotateAngle, Space.World);
        float dist = Vector3.Distance(pos1, pos2);
        if (dist < distMin) {
            if (currentEndPos.m_child == null) { 
                
            }
            else {
                currentEndPos = currentEndPos.m_child;
            }
        }

        agent.SetDestination(pos2);
        //agent.speed = BotSpeed();
    }
}  