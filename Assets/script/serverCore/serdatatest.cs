using UnityEngine;
using System.Collections;

public class serdatatest : MonoBehaviour
{
    private static serdatatest _instence;
    public static serdatatest Instence
    {
        get
        {
            if (_instence==null)
            {
                _instence = GameObject.FindObjectOfType<serdatatest>();
               
            }
            return _instence;
        }
    }



    public int i = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log(i);
        }
       
	}
    public void add()
    {
        i++;
        Debug.Log(i);
    }
}
