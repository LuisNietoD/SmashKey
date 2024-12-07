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
