using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    private Animator animator;
    private bool isReloading;

    public float reloadTime;

    [SerializeField] Rig aimRig;
    [SerializeField] UnityEvent OnReloaded;
    [SerializeField] UnityEvent OnFired;

    [SerializeField] Transform rightHandHold;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnReload(InputValue value)
    {
        StartCoroutine(ReloadRoutine());
    }

    private void OnFire(InputValue value)
    {
        if (isReloading)
            return;

        StartCoroutine(FireRiggingRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        OnReloaded?.Invoke();
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    IEnumerator FireRiggingRoutine()
    {
        OnFired?.Invoke();
        rightHandHold.Translate(-0.07f, 0, 0);
        yield return new WaitForSeconds(0.1f);
        rightHandHold.Translate(0.07f, 0, 0);
        yield return new WaitForSeconds(0.1f);
    }
}
