using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    private Animator anim;
    private LineRenderer lineRenderer;
    private Camera mainCamera;
    private RaycastHit hit;
    private ParticleSystem muzzleFlash;

    [SerializeField] private float LineRadius = 0.3f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        mainCamera = Camera.main;

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = LineRadius;
        lineRenderer.endWidth = LineRadius;
    }

    void Update()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraDirection = mainCamera.transform.forward;

        if (Physics.Raycast(cameraPosition, cameraDirection, out hit, 500f))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, cameraPosition + cameraDirection * 100);
        }
    }

    public void Shoting(float damage)
    {
        anim.SetTrigger("Fire");
        if(hit.collider.tag == "Enemy")
        {
            IDamageable enemy = hit.collider.GetComponent<IDamageable>();
            if(enemy != null)
            enemy.ApplayDamage(damage);
        }
        muzzleFlash.Play();
    }
}
