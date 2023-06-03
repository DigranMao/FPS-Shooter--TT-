using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsPanel : MonoBehaviour
{
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Inventory inventory;
    [SerializeField] private TextMeshProUGUI healthText, patronsText;
    private float maxHealth;
    private PlayerController player;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        maxHealth = player.CurrentHealth;
    }

    void Update()
    {
        healthText.text = player.CurrentHealth.ToString();

        float normalizedHealth = player.CurrentHealth / maxHealth;
        Color textColor = healthGradient.Evaluate(normalizedHealth);
        healthText.color = textColor;

        WeaponItem selectedWeapon = inventory.GetWeaponInQuickSlot(inventory.selectedQuickSlotIndex);
        if (selectedWeapon != null)
            patronsText.text = (selectedWeapon.currentAmmoCount + " / " + selectedWeapon.maxAmmoCount).ToString();

    }
}
