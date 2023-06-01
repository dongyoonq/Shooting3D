using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] ParticleSystem hitEffect;
    protected float bulletSpeed;
    protected float maxDistance;
    protected float damage;
    public int magazine;
    public int Maxmagazine;
    private ObjectPool EffectPool;
    private ObjectPool TrailPool;

    private void Awake()
    {
        EffectPool = GameObject.FindGameObjectWithTag("ParticlePool").GetComponent<ObjectPool>();
    }

    public virtual void Fire()
    {
        muzzleEffect.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            IHittable hittable = hit.transform.GetComponent<IHittable>();
            Poolable particle = EffectPool.Get();
            particle.transform.position = hit.point;
            particle.transform.rotation = Quaternion.LookRotation(hit.normal);
            particle.transform.parent = hit.transform;

            StartCoroutine(trailRoutine(muzzleEffect.transform.position, hit.point));

            hittable?.Hit(hit, damage);
        }
        else
        {
            StartCoroutine(trailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
        }
    }

    public void Reload()
    {
        magazine = Maxmagazine;
    }

    IEnumerator trailRoutine(Vector3 start, Vector3 end)
    {
        Poolable trail = TrailPool.Get();
        trail.transform.position = start;
        trail.transform.rotation = Quaternion.identity;
        
        float totalTime = Vector2.Distance(start, end) / bulletSpeed;

        float rate = 0f;

        while (rate < 1)
        { 
            trail.transform.position = Vector3.Lerp(start, end, rate);
            rate += Time.deltaTime / totalTime;

            yield return null;
        }

        Destroy(trail); 
    }
}
