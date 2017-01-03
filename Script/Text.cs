using UnityEngine;

public class Text : MonoBehaviour
{
    private bool t=true;
    void Start()
    {
    }
    void Update()
    {
        if (t)
        {
            Debug.Log(t);
            t = false;
        }
    }

}
