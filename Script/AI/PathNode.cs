using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {
    public PathNode m_parent;
    public PathNode m_child;
	
    public void SetNext(PathNode node){
        if (m_child != null) {
            m_child.m_parent = null;
        }
        m_child = node;
        node.m_parent = this;
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(this.transform.position, "Node.tif");
    }
}
