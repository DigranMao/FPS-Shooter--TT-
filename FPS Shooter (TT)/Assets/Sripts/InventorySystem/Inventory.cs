using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private QuickSlot[] quickSlots = new QuickSlot[3];
    [SerializeField] private RectTransform[] slot = new RectTransform[3];
    [SerializeField] private Material[] weaponMaterials;
    [SerializeField] private GameObject weaponModel;
    [SerializeField] private Vector3 originalScaleSlot = new Vector3(1, 1, 1), maxScaleSlot = new Vector3(1.2f, 1.2f, 1.2f);

    private Renderer weaponRenderer;

    public int selectedQuickSlotIndex;

    void Start()
    {
        weaponRenderer = weaponModel.GetComponent<Renderer>();
        foreach(QuickSlot quickSlot in quickSlots)
        {
            quickSlot.weaponItem.currentAmmoCount = quickSlot.weaponItem.maxAmmoCount;
        }
    }

    public void SelectQuickSlot(int index)
    {
        selectedQuickSlotIndex = index;

        for (int i = 0; i < slot.Length; i++)
        {
            if (i == index)
            {
                slot[i].localScale = maxScaleSlot;
                weaponRenderer.material = GetWeaponMaterial(index);
            }            
            else slot[i].localScale = originalScaleSlot;
        }
    }

    public WeaponItem GetWeaponInQuickSlot(int index)
    {
        if (index >= 0 && index < quickSlots.Length)
            return quickSlots[index].weaponItem;
        else return null;
    }

    private Material GetWeaponMaterial(int index)
    {
        if (index >= 0 && index < weaponMaterials.Length)
            return weaponMaterials[index];
        else return null;
    }
}
