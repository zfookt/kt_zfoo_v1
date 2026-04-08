using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceLoader : MonoSingleton<ResourceLoader>
{
    /// <summary>
    /// 同步加载GameObject
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <returns></returns>
    public GameObject LoadAndInstantiate(string resourcePath)
    {
        GameObject loadedObject = Load<GameObject>(resourcePath);
        if (loadedObject != null)
        {
            GameObject instantiatedObject = Instantiate(loadedObject);
            instantiatedObject.name = loadedObject.name;
            return instantiatedObject;
        }

        Debug.LogError($"{resourcePath} not find!!!");

        return null;
    }

    /// <summary>
    /// 异步加载GameObject，并回调
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <param name="onInstantiated"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadAndInstantiateAsync<T>(string resourcePath, Action<GameObject> onInstantiated)
    {
        LoadAsync<GameObject>(resourcePath, loadedObject =>
        {
            if (loadedObject != null)
            {
                GameObject instantiatedObject = Instantiate(loadedObject);
                instantiatedObject.name = loadedObject.name;
                onInstantiated?.Invoke(instantiatedObject);
            }
            else
            {
                Debug.LogError($"{resourcePath} not find!!!");
            }
        });
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <param name="onLoaded"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadAsync<T>(string resourcePath, Action<T> onLoaded) where T : UnityEngine.Object
    {
        StartCoroutine(LoadCoroutine(resourcePath, onLoaded));
    }

    /// <summary>
    /// 同步加载资源(使用较少)
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Load<T>(string resourcePath) where T : UnityEngine.Object
    {
        return Resources.Load<T>(resourcePath);
    }

    private IEnumerator LoadCoroutine<T>(string resourcePath, Action<T> onLoaded) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(resourcePath);

        // 等待加载完成
        yield return request;

        if (request.asset != null)
        {
            onLoaded?.Invoke(request.asset as T);
        }
        else
        {
            onLoaded?.Invoke(null);
        }
    }
}