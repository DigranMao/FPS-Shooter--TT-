using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnemy : MonoBehaviour
{
    private RaycastHit hit;
    private ParticleSystem muzzleFlash;

    [SerializeField] private float damage = 25f;

    void Awake()
    {
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 500f);
    }

    public void Shooting()
    {
        IDamageable player = hit.transform.GetComponent<IDamageable>();
        if(player != null)
            player.ApplayDamage(damage);

        muzzleFlash.Play();
    }
}
