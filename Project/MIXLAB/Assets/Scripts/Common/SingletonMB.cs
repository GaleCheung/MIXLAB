using UnityEngine;

public class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
{
    //私有的静态实例
    private static T _instance = null;

    //共有的唯一的，全局访问点
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null)
                {
                    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        //防止存在多个单例
        if (_instance == null)
            _instance = this as T;
        else
            Destroy(gameObject);
    }
}