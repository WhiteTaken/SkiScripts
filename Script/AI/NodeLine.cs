using UnityEngine;
using System.Collections;

public class NodeLine : MonoBehaviour {
    public bool m_debug = false;
    public ArrayList m_PathNodes;

    void Start() {
        BuildPath();
    }

    void BuildPath() {
        m_PathNodes = new ArrayList();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");

        for (int i = 0; i < objs.Length; i++ )
        {
            PathNode node = objs[i].GetComponent<PathNode>();
            m_PathNodes.Add(node);
        }
    }

    void OnDrawGizmos() {
        if (!m_debug || m_PathNodes == null) {
            return;
        }
        Gizmos.color = Color.red;

        foreach (PathNode node in m_PathNodes) {
            if (node.m_child != null) {
                Gizmos.DrawLine(node.transform.position, node.m_child.transform.position);
            }
        }
    }
}
