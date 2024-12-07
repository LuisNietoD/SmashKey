using System;
using LTX.Singletons;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
}
public interface IWeapon
{
    public int damage { get; }
    public void AutoShoot();
}

public interface IEnemy
{
    public int damage { get; }
    public int health { get; set; }
    public void Attack();
    public void Hit(int damageAmount);
}
