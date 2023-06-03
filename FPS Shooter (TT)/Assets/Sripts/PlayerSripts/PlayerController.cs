using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{    
    [SerializeField] private GameObject losePanel;
    [SerializeField] private ParticleSystem explosionSmoke;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Inventory inventory;
    [SerializeField] private float maxSpeed = 10f, minSpeed = 5f, sensitivity = 5f;
    [SerializeField] private float jumpForce = 5f, gravity = -25f;

    private CharacterController characterController;
    private Animator anim;
    private Weapons weapons;
    private Vector3 moveDirection, velocity;

    private float cameraRotationX = 0f, moveSpeed;
    private float currentHealth = 100;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        weapons = GetComponentInChildren<Weapons>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Movement();
        Jump();
        Shooting();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            inventory.SelectQuickSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            inventory.SelectQuickSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            inventory.SelectQuickSlot(2);
    }

    void Shooting()
    {
        if(Input.GetMouseButtonDown(0))
        {
            WeaponItem selectedWeapon = inventory.GetWeaponInQuickSlot(inventory.selectedQuickSlotIndex);
            if (selectedWeapon != null && selectedWeapon.currentAmmoCount > 0)
                Fire(selectedWeapon);
            else Reload(selectedWeapon);
        }
    }

    void Fire(WeaponItem weapon)
    {
        weapon.currentAmmoCount--;
        weapons.Shoting(weapon.damage);
    }

    void Reload(WeaponItem weapon)
    {
        weapon.currentAmmoCount = weapon.maxAmmoCount;
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * moveDirection;
        moveDirection *= moveSpeed;
        characterController.Move(moveDirection * Time.deltaTime);

        transform.Rotate(Vector3.up, mouseX * sensitivity);

        cameraRotationX -= mouseY * sensitivity;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -50f, 50f);

        cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            moveSpeed = maxSpeed; 
        else moveSpeed = minSpeed;
    }

    void Jump()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime); 

        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);            
        }
    }

    public void ApplayDamage(float damage)
    {
        CurrentHealth -= damage;

        if(CurrentHealth <= 0)
            Death();
    }

    void Death()
    {
        int currentLossesCount = PlayerPrefs.GetInt("LossesCount");
        int lossesCount = currentLossesCount + 1;
        PlayerPrefs.SetInt("LossesCount", lossesCount);

        explosionSmoke.Play();
        losePanel.SetActive(true);
        gameObject.layer = 0;

        GetComponent<MeshRenderer>().enabled = false;
        Invoke("OnMenu", 6);
        enabled = false;
    }

    void OnMenu()
    {
        SceneManager.LoadScene(0);
    }
}
