using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetPool<T> where T : UnityEngine.Object
{
    private Dictionary<string, T> _pathToAsset = new Dictionary<string, T>();
    private Dictionary<T, string> _assetToPath = new Dictionary<T, string>(); // 反向映射，用于直接释放资源

    /// <summary>
    /// 加载资源（如果已缓存则直接返回）
    /// </summary>
    public T Get(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("AssetPool: Path cannot be null or empty!");
            return null;
        }

        // 如果已缓存，直接返回
        if (_pathToAsset.TryGetValue(path, out T cachedAsset))
        {
            return cachedAsset;
        }

        // 否则加载资源
#if UNITY_EDITOR
        T asset = AssetDatabase.LoadAssetAtPath<T>(path);
#else
        T asset = Resources.Load<T>(path);
#endif
        if (asset == null)
        {
            Debug.LogError($"AssetPool: Failed to load asset at path '{path}'");
            return null;
        }

        // 存入缓存
        _pathToAsset[path] = asset;
        _assetToPath[asset] = path; // 建立反向映射
        return asset;
    }

    /// <summary>
    /// 通过路径释放资源
    /// </summary>
    public void Release(string path)
    {
        if (_pathToAsset.TryGetValue(path, out T asset))
        {
            ReleaseInternal(asset, path);
        }
    }

    /// <summary>
    /// 直接释放资源对象（无需知道路径）
    /// </summary>
    public void Release(T asset)
    {
        if (asset == null) return;

        if (_assetToPath.TryGetValue(asset, out string path))
        {
            ReleaseInternal(asset, path);
        }
        else
        {
            Debug.LogWarning("AssetPool: Trying to release an uncached asset.");
        }
    }

    /// <summary>
    /// 内部释放逻辑
    /// </summary>
    private void ReleaseInternal(T asset, string path)
    {
        if (asset != null && !string.IsNullOrEmpty(path))
        {
#if UNITY_EDITOR
            Resources.UnloadAsset(asset);
#endif
            _pathToAsset.Remove(path);
            _assetToPath.Remove(asset);
        }
    }

    /// <summary>
    /// 清空所有缓存
    /// </summary>
    public void Clear()
    {
#if UNITY_EDITOR
        foreach (var asset in _pathToAsset.Values)
        {
            Resources.UnloadAsset(asset);
        }
#endif
        _pathToAsset.Clear();
        _assetToPath.Clear();
    }

    /// <summary>
    /// 检查某个路径的资源是否已缓存
    /// </summary>
    public bool Contains(string path)
    {
        return _pathToAsset.ContainsKey(path);
    }

    /// <summary>
    /// 检查某个资源对象是否已缓存
    /// </summary>
    public bool Contains(T asset)
    {
        return _assetToPath.ContainsKey(asset);
    }
}