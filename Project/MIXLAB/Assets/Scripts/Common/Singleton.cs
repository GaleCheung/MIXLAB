﻿public class Singleton<T> where T : class, new()
{
    private static T _instance;
    private static readonly object syslock = new object();

    public static T Instance()
    {
        if (_instance == null)
        {
            lock (syslock)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
            }
        }

        return _instance;
    }
}