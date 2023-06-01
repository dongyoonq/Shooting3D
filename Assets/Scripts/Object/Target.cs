using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IHittable
{
    [SerializeField] float maxHp;
    private Rigidbody target;

    private void Awake()
    {
        target = GetComponent<Rigidbody>();
    }

    public void Hit(RaycastHit hit, float damage)
    {
        maxHp -= damage;
        target?.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);
    }

    public void Update()
    {
        if (maxHp <= 0)
            Destroy(gameObject);
    }
}
