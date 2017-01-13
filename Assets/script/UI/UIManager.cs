using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        public UIInfoData(EnumUIPlaneType UIType,string Path,params object[] uiparams)
        {
            _uiparams = uiparams;
            _uiType = UIType;
            _path = Path;
        }
    }


    //已经打开的UI
    private Dictionary<EnumUIPlaneType, GameObject> dicOpenUI;
    //将要打开的UI
    private Stack<UIInfoData> stackOpneUI;
    public void OpenUI(EnumUIPlaneType UItype,bool closeOther,params object[] UIparams)
    {
        if (!dicOpenUI.ContainsKey(UItype))
        {
            string path = "";
            stackOpneUI.Push(new UIInfoData(UItype,path,UIparams) );
        }
    }
    public void CloseUI(EnumUIPlaneType UItype)
    {
        if (dicOpenUI.ContainsKey(UItype))
        {
            GameObject temp;
            dicOpenUI.TryGetValue(UItype, out temp);
            GameObject.Destroy(temp);
            dicOpenUI.Remove(UItype);
        }
    }

}
