using UnityEngine;
using System.Collections;

public class Singleton<T> where T : class, new()
{
    protected static T _instence = null;
    public  static T Instence
    {
        get
        {
            if (_instence==null)
            {
                _instence = new T();

            }
            return _instence;
        }
    }
    protected Singleton()
    {
        Init();
    }
    public virtual void Init()
    {

    }
}
