using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPool 
{
    Transform parent_Transform { get; set; }
    public Queue<GameObject> pool { get; set; }

    GameObject Get(Action<GameObject> action = null);

    void Return(GameObject obj, Action<GameObject> action = null);
}

public class Object_Pool : IPool
{
    public Transform parent_Transform { get; set; }
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        action?.Invoke(obj);

        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        obj.transform.parent = parent_Transform;
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        action?.Invoke(obj);    
    }
}

public class Pool_Manager
{
    public Dictionary<string, IPool> pool_Dictionary = new Dictionary<string, IPool>();
    Transform base_Parents;
    public void Init(Transform T)
    {
        base_Parents = T;
    }
    public IPool Pooling_OBJ(string path)
    {
        if (pool_Dictionary.ContainsKey(path) == false) 
        {
            Add_Pool(path);
        }
        if (pool_Dictionary[path].pool.Count <= 0)
        {
            Add_Queue(path);
        }
        return pool_Dictionary[path];
    }

    public GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject("##" + path); 
        obj.transform.parent = base_Parents;

        Object_Pool pool = new Object_Pool();

        pool_Dictionary[path] = pool;

        pool.parent_Transform = obj.transform;

        return obj;
    }

    public void Add_Queue(string path)
    {
        var go = Base_Manager.instance.Get_Prefab_OBJ("Pool_Obj/" + path);
        go.transform.parent = pool_Dictionary[path].parent_Transform;

        pool_Dictionary[path].Return(go);
    }
}
