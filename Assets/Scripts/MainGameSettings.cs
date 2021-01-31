using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainSettings", menuName = "ScriptableObject/MainSettings")]
public class MainGameSettings : ScriptableObject
{
  [Tooltip("Время перезарядки скила для нахождения следов игрока")]
  public float hunterSkill1Time;
  [Tooltip("Время действия скила для нахождения следов игрока")]
  public float hunterSkill1Duration;
  [Tooltip("Сорость выстрелов")]
  public float shootRate;

  [Tooltip("Жизнь призрака")]
  public float healt;
  [Tooltip("Время перезарядки скила для прятания следов игрока")]
  public float hidingSkill1Time;
  [Tooltip("Время действия скила для нахождения следов игрока")]
  public float hidingSkill1Duration;
  [Tooltip("Время перезарядки скила для превращение в предмет")]
  public float hidingSkill2Time;

  [Tooltip("Начальное время суток")]
  public float currentTimeOfDay;
  [Tooltip("Скорость смены дня и ночи")]
  public int dayNightCycleSpeed;
}
