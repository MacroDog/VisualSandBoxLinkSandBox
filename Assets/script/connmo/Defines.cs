using UnityEngine;
using System.Collections;





public delegate void StateChangeEvent(object ui, EnumObjectState oldState, EnumObjectState newState);


public enum EnumObjectState
{
    None,
    Initial,
    Loading,
    Ready,
    Disable,
    Closing
}


public enum EnumUIPlaneType:int
{
    MainUI=0,
    MoviePlay=1,
}
public class UIPathDefines
{
    public const string UI_PREFAB = "UIPrefabs/";


    public const string UI_CONTROLS_PAEFAB = "UI_Controls_Prefabs/";
    public static  string  GetPrefabsPathByType(EnumUIPlaneType type)
    {
        string path = string.Empty;
        switch (type)
        {
            case EnumUIPlaneType.MainUI:
                path = UI_PREFAB + "MainUI";
                break;
            case EnumUIPlaneType.MoviePlay:
                path = UI_PREFAB + "MoviePlay";
                break;
            default:
                break;
        }
        return path;
    }
    public static System.Type GetUIScriptType(EnumUIPlaneType _uiType)
    {
        System.Type temp = null;
        switch (_uiType)
        {
            case EnumUIPlaneType.MainUI:
                temp = typeof(MainMaun);
                break;
            case EnumUIPlaneType.MoviePlay:
                temp = typeof(MoviePlayUI);
                break;
            default:
                break;
        }
        return temp;
    }
}
public class Defines
{
    public Defines()
    {

    }

}
