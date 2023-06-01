using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riffle : Gun
{
    private void Awake()
    {
        this.maxDistance = 100f;
        this.damage = 5f;
        this.Maxmagazine = 20;
        magazine = Maxmagazine;
        this.bulletSpeed = 1000;
    }

    public override void Fire()
    {
        if (magazine > 0)
        {
            base.Fire();
            magazine -= 1;
        }
    }
}
