using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField] float releaseTime;

    private ObjectPool pool;
    public ObjectPool Pool { get { return pool; } set { pool = value; } }

    private void OnEnable()
    {
        StartCoroutine(ReleaseTimer());
    }

    IEnumerator ReleaseTimer()
    {
        yield return new WaitForSeconds(releaseTime);
        pool.Release(this);
    }

    public Poolable InstantiatePoolable(Poolable poolable, Vector3 position, Quaternion rotation)
    {

        poolable.transform.position = position;
        poolable.transform.rotation = rotation;

        return poolable;
    }

    public Poolable InstantiatePoolable(Poolable poolable, Vector3 position, Quaternion rotation, Transform parent)
    {

        poolable.transform.position = position;
        poolable.transform.rotation = rotation;
        poolable.transform.parent = parent;

        return poolable;
    }
}
