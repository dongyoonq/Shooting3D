using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTester : MonoBehaviour
{
    private ObjectPool pool;

    private void Awake()
    {
        pool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Poolable poolable = pool.Get();
            poolable.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        }
    }
}
