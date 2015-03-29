using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class HitMobEvent : UnityEvent<Collider, int>
{
}
