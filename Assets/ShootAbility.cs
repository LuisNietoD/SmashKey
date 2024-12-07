using System;
using System.Collections.Generic;
using LTX.Singletons;
using UnityEngine;


public class ShootAbility : MonoSingleton<ShootAbility>
{
    public List<GameObject> weaponObject = new List<GameObject>();
    public List<IWeapon> weapons = new List<IWeapon>();

    private void Start()
    {
        foreach (var w in weaponObject)
        {
            weapons.Add(w.GetComponent<IWeapon>());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (var weapon in weapons)
            {
                weapon.AutoShoot();
            }
        }
    }
}