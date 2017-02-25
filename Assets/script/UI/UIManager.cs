using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager : Singleton<UIManager>
{
    public class UIInfoData
    {
        public EnumUIPlaneType _uiType
        {
            get;
            private set;
        }
        public object[] _uiparams
        {
            get;
            private set;
        }
        public string _path
        {
            get;
            private set;
        }
        public Type ScriptType
        {
            get;
            private set;
        }
        public UIInfoData(EnumUIPlaneType UIType,string Path,params object[] uiparams)
        {
            _uiparams = uiparams;
            _uiType = UIType;
            _path = Path;
            this.ScriptType = UIPathDefines.GetUIScriptType(UIType);
        }
    }
    //已经打开的UI
    private Dictionary<EnumUIPlaneType, GameObject> dicOpenUI;
    //将要打开的UI
    private Stack<UIInfoData> stackOpneUI;
    public void OpenUI(EnumUIPlaneType UItype,bool closeOther,params object[] UIparams)
    {
        if (closeOther==true)
        {
            CloseUIAll();
        }
        if (!dicOpenUI.ContainsKey(UItype))
        {
            string path = UIPathDefines.GetPrefabsPathByType(UItype);
            stackOpneUI.Push(new UIInfoData(UItype, path, UIparams));

        }
        if (stackOpneUI.Count>0)
        {
            for (int i = 0; i < stackOpneUI.Count; i++)
            {
                CoroutineController.Instance.StartCoroutine(asyncLoad());
            }
        }
        
    }
    public GameObject GetUIGameObject(EnumUIPlaneType type)
    {
        GameObject temp = null;
        if (!dicOpenUI.TryGetValue(type,out temp))
        {
            throw new Exception("_dicOpen trygetvalue fail");
        }
        return temp;
    }
    public void CloseUI(EnumUIPlaneType UItype)
    {
        GameObject temp = GetUIGameObject(UItype);
        if (temp!=null)
        {
            CloseUI(UItype, temp);

        }
        else
        {
            dicOpenUI.Remove(UItype);
        }
    }
    public void CloseUI(EnumUIPlaneType UItype,GameObject _uiObject)
    {
        //if (dicOpenUI.ContainsKey(UItype))
        //{
        //    GameObject temp;
        //    dicOpenUI.TryGetValue(UItype, out temp);
        //    GameObject.Destroy(temp);
        //    dicOpenUI.Remove(UItype);
        //}
        if (_uiObject==null)
        {
            GameObject.Destroy(_uiObject);
            dicOpenUI.Remove(UItype);
        }
        else
        {
            BaseUI _baseUI = _uiObject.GetComponent<BaseUI>();
            if (_baseUI==null)
            {
                GameObject.Destroy(_baseUI);
                dicOpenUI.Remove(UItype);
            }
            else
            {
                _baseUI.stateChange += CloseUIHandle;
                _baseUI.Release();
            }
        }
    }
    private void CloseUIHandle(object sender,EnumObjectState oldState,EnumObjectState newState)
    {
        if (newState==EnumObjectState.Closing)
        {
            BaseUI _baseUI = sender as BaseUI;
            dicOpenUI.Remove(_baseUI.GetUIType());
            _baseUI.stateChange -= CloseUIHandle;
        }
    }
    public void CloseUIAll()
    {
        List<EnumUIPlaneType> _listKey = new List<EnumUIPlaneType>(dicOpenUI.Keys);
        for (int i = 0; i < _listKey.Count; i++)
        {
            CloseUI(_listKey[i]);
        }
        dicOpenUI.Clear();
    }
    private IEnumerator<int> asyncLoad()
    {
        UIInfoData _uiInfoData = null;
        UnityEngine.Object _prefab = null;
        GameObject _uiObject = null;
        if (stackOpneUI != null && stackOpneUI.Count > 0)
        {
            do
            {
                _uiInfoData = stackOpneUI.Pop();
                _prefab = Resources.Load(_uiInfoData._path);
                if (_prefab != null)
                {
                    _uiObject = GameObject.Instantiate(_prefab,GameObject.FindObjectOfType<Canvas>().transform,false) as GameObject;
                    BaseUI _baseUI = _uiObject.GetComponent<BaseUI>();
                    if (_baseUI != null)
                    {

                        _baseUI.SetUIWhenOpening(_uiInfoData._uiparams);
                    }
                    else
                    {
                        Type temp = _uiInfoData.ScriptType;
                        _baseUI = _uiObject.AddComponent(_uiInfoData.ScriptType) as BaseUI;
                    }
                    dicOpenUI.Add(_uiInfoData._uiType, _uiObject);
                }
            } while (stackOpneUI.Count > 0);
        }
        yield return 0;
    }
    public override void Init()
    {
        base.Init();
        dicOpenUI = new Dictionary<EnumUIPlaneType, GameObject>();
        stackOpneUI = new Stack<UIInfoData>();
    }
}
