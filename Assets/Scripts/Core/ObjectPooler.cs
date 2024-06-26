using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PoolingInfo
{
    public string keyName;
    public List<GameObject> inactiveObjects;
}

[System.Serializable]
public class PrespawnData
{
    public int count;
    public PoolTypes type = PoolTypes.None;
    public GameObject prefab;
}

public enum PoolTypes { ParticleSystem, GameObject, None }

public class ObjectPooler : MonoBehaviour
{

    [SerializeField] PrespawnData[] prespawnList;

    [Space(30)]
    [Header("=== Debug Info ===")]
    [SerializeField] bool debugMode;
    [SerializeField] List<PoolingInfo> DebugList;

    public static Dictionary<string, List<GameObject>> ObjectPools = new Dictionary<string, List<GameObject>>();

    private GameObject _objectPoolContainer;
    private static GameObject _particleSystemContainer;
    private static GameObject _gameObjectContainer;

    private void Awake()
    {
        _objectPoolContainer = new GameObject("=== Pooled Objects ===");

        _particleSystemContainer = new GameObject("--- Particle Systems ---");
        _particleSystemContainer.transform.SetParent(_objectPoolContainer.transform);

        _gameObjectContainer = new GameObject("--- Game Objects ---");
        _gameObjectContainer.transform.SetParent(_objectPoolContainer.transform);

        foreach (PrespawnData data in prespawnList)
        {
            ObjectPools.Add(data.prefab.name, new List<GameObject>());

            for (int i = 0; i < data.count; i++)
            {
                GameObject clone = Instantiate(data.prefab, new Vector3(10000, 10000, 10000), Quaternion.identity);
                clone.transform.SetParent(GetContainer(data.type));
                clone.SetActive(false);
                ObjectPools[data.prefab.name].Add(clone);

            }
        }
    }


    private void Update()
    {
        if (debugMode)
        {
            DebugList.Clear();
            foreach (var kvp in ObjectPools)
            {
                PoolingInfo info = new PoolingInfo();
                info.keyName = kvp.Key;
                info.inactiveObjects = kvp.Value;
                DebugList.Add(info);
            }
        }
    }

    static Transform GetContainer(PoolTypes type)
    {
        switch(type)
        {
            case PoolTypes.GameObject:
                return _gameObjectContainer.transform;
            case PoolTypes.ParticleSystem:
                return _particleSystemContainer.transform;
            default:
                return null;
        }
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 spawnPosition, Quaternion spawnRotation, PoolTypes type = PoolTypes.None)
    {
        Transform parentContainer = GetContainer(type);

        if (!ObjectPools.ContainsKey(prefab.name))
        {
            ObjectPools.Add(prefab.name, new List<GameObject>());
        }

        GameObject spawnableObj = null;

        List<GameObject> pool = ObjectPools[prefab.name];

        foreach (GameObject obj in pool)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(prefab, spawnPosition, spawnRotation);

            if (parentContainer != null)
            {
                spawnableObj.transform.SetParent(parentContainer);
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 spawnPosition, Quaternion spawnRotation, Transform parentContainer)
    {

        if (!ObjectPools.ContainsKey(prefab.name))
        {
            ObjectPools.Add(prefab.name, new List<GameObject>());
        }

        GameObject spawnableObj = null;

        List<GameObject> pool = ObjectPools[prefab.name];

        foreach (GameObject obj in pool)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(prefab, spawnPosition, spawnRotation);

            if (parentContainer != null)
            {
                spawnableObj.transform.SetParent(parentContainer);
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReleaseObject(GameObject obj)
    {
        string instanceName = obj.name.Substring(0, obj.name.Length - 7);

        if (!ObjectPools.ContainsKey(instanceName))
        {
            ObjectPools.Add(instanceName, new List<GameObject>());
        }

        List<GameObject> pool = ObjectPools[instanceName];

        obj.SetActive(false);
        pool.Add(obj);
    }
}
