using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] TrailRenderer bulletTrail;
    public float bulletSpeed;
    public float maxDistance;
    public float damage;
    private int magazine;
    public int Maxmagazine;

    private void Awake()
    {
        Maxmagazine = 20;
        damage = 5;
        bulletSpeed = 2000;
        maxDistance = 200;
        magazine = Maxmagazine;
        muzzleEffect = GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    public virtual void Fire()
    {
        if (magazine > 0)
        {
            muzzleEffect.Play();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
            {
                IHittable hittable = hit.transform.GetComponent<IHittable>();

                ParticleSystem effect = GameManager.Resouce.Instantiate("prefabs/HitEffect", hitEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform, true);
                GameManager.Resouce.Destroy(effect, 3f);

                StartCoroutine(trailRoutine(muzzleEffect.transform.position, hit.point));

                hittable?.Hit(hit, damage);
            }
            else
            {
                StartCoroutine(trailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
            }

            magazine -= 1;
        }
    }

    public void Reload()
    {
        magazine = Maxmagazine;
    }

    IEnumerator trailRoutine(Vector3 start, Vector3 end)
    {
        TrailRenderer trail = GameManager.Resouce.Instantiate("prefabs/BulletTrail", bulletTrail, start, Quaternion.identity, true);
        trail.Clear();
        
        float totalTime = Vector2.Distance(start, end) / bulletSpeed;

        float rate = 0f;

        while (rate < 1)
        { 
            trail.transform.position = Vector3.Lerp(start, end, rate);
            rate += Time.deltaTime / totalTime;

            yield return null;
        }

        GameManager.Resouce.Destroy(trail.gameObject, 10f);
    }
}
