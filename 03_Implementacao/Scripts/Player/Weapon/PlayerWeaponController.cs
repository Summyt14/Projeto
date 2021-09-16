using System;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject[] weapons;
    [SerializeField] private GameObject playerHand;

    private GameObject EquippedWeapon { get; set; }
    private IWeapon _equippedWeapon;

    public void EquipWeapon(GameObject weaponToEquip)
    {
        if (EquippedWeapon != null) UnequipWeapon();

        EquippedWeapon = Instantiate(weaponToEquip, playerHand.transform.position, playerHand.transform.rotation);
        _equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
        EquippedWeapon.transform.SetParent(playerHand.transform);
    }

    public void UnequipWeapon()
    {
        Destroy(playerHand.transform.GetChild(0).gameObject);
    }

    public void PerformWeaponAttack(float damage)
    {
        _equippedWeapon.PerformAttack(damage);
    }
}