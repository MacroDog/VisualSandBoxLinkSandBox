using UnityEngine;
using System.Collections;

public abstract class BaseUI : MonoBehaviour {

    protected EnumObjectState _state = EnumObjectState.None;
    public event StateChangeEvent stateChange;
    public EnumObjectState State
    {
        protected get
        {
            return _state;
        }
        set
        {
            EnumObjectState oldState = this._state;
            _state = value;
            if (stateChange != null)
            {
                stateChange(this, oldState, _state);
            }
        }
    }
    public abstract EnumUIPlaneType GetUIType();
    void Awake()
    {
        this.State = EnumObjectState.Initial;
        OnAwake();
    }
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
        UIManager.Instence.CloseUI(GetUIType());
    }
    protected virtual void OnAwake()
    {
        this.State = EnumObjectState.None;
        this.OnPlayOpenUIAduio();
    }
    public void OpenUI()
    {
        Init();
        
    }
    void Distory()
    {
        this._state = EnumObjectState.None;
    }
    protected virtual void OnState()
    {

    }
    protected virtual void OnPlayOpenUIAduio()
    {

    }
    protected virtual void OnCloseUIAudio()
    {

    }
    protected virtual void SetUI(params object[] UIParams)
    {
        this.State = EnumObjectState.Loading;
    }
    /// <summary>
    /// 当打开时初始化UI 由子类实现
    /// </summary>
    protected virtual void  Init()
    {

    }
    
    public void SetUIWhenOpening(params object[] uiparams)
    {

        SetUI(uiparams);
        StartCoroutine(loadDataAsyn());

    }
    protected virtual void OnLoadData()
    {
        Init();
    }
    public  void Release()
    {
        this.State = EnumObjectState.Closing;
        OnRelease();
        Destroy(this.gameObject);
        
    }
    protected virtual void OnRelease()
    {

    }
    private IEnumerator loadDataAsyn()
    {
        yield return new WaitForSeconds(0);
        if (this.State ==EnumObjectState.Loading)
        {
            this.OnLoadData();
            this.State = EnumObjectState.Ready;
        }
        
    }
   
    
}
