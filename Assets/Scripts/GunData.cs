using NUnit.Framework.Internal;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float damage = 25f;
    public int startAmmoRemain= 80;
    public int magCapcity = 25;
    public float timeBetFire = 00.12f; //발사 간격
    public float reloadTime = 1.8f;
}
