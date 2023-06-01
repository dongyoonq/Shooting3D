using UnityEngine;

interface IHittable
{
    abstract void Hit(RaycastHit hit, float damage);
}