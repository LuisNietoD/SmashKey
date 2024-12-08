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
    
    
    private bool inGame = true;
    
    private void OnEnable()
    {
        GameController.OnGameEnd += GameEnd;
    }
    
    private void OnDisable()
    {
        GameController.OnGameEnd -= GameEnd;
    }

    private void GameEnd()
    {
        inGame = false;
    }
    
    private void Update()
    {
        if (!inGame) return;
        
        if (Input.GetMouseButton(0))
        {
            foreach (var weapon in weapons)
            {
                weapon.AutoShoot();
            }
        }
    }
}