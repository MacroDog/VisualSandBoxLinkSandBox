using UnityEngine;
using System.Collections;

public class DDOLSingelton<T> : MonoBehaviour where T:DDOLSingelton<T>
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance==null)
                {
                    GameObject temp = new GameObject();
                    _instance = temp.AddComponent<T>();
                    DontDestroyOnLoad(temp);
                }
            }
            return _instance;
        }
        
    }
    private void OnApplicationQuit()
    {
        _instance = null;
    }
}
