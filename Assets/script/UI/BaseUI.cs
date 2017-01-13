using UnityEngine;
using System.Collections;

public class BaseUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 关闭UI
    /// </summary>
    public void ClosePanel()
    {

    }
    public  void OpenUI()
    {
        Init();
    }


    /// <summary>
    /// 当打开时初始化UI 由子类实现
    /// </summary>
    protected virtual void  Init()
    {

    }
}
