using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScripSingleton <T>:MonoBehaviour where T:ScripSingleton<T>
{
    protected static T _instance;
    public static T Ins
    {
        get
        {
            if(_instance == null)
            {
                //从场景中找T脚本的对象
                _instance = FindObjectOfType<T>();
                if(FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogError("场景中的单例脚本数量 > 1 :" + _instance.GetType().ToString());
                    return _instance;
                }
                //场景中找不到
                if(_instance == null)
                {
                    string insName = typeof(T).Name;
                    Debug.LogErrorFormat("typeof T -{0} name - {1}/", typeof(T),insName);
                    GameObject insGo = GameObject.Find(insName);
                    if(insGo == null)
                    {
                        insGo = new GameObject(insName);
                        DontDestroyOnLoad(insGo);
                        _instance = insGo.AddComponent<T>();
                        DontDestroyOnLoad(_instance);
                    }
                    else
                    {
                        Debug.LogError("场景中已存在单例脚本所挂载的游戏物体：" + insGo.name);
                    }
                }
            }
            return _instance;
        }

    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
