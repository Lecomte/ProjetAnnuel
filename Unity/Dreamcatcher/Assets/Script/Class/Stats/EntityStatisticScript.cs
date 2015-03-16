using UnityEngine;
using System;
using System.Collections;

public class EntityStatisticScript : MonoBehaviour {

    public int MaxHealth;
    public int CurrentHealth;
    public int Damage;
    public int Resistance;

    public void TakeDamage(int damage)
    {
        this.CurrentHealth -= (damage - this.Resistance);
    }

    public void TemporaryIncreaseDamage(int value, int secondes)
    {
        this.IncreaseDamage(value);
        StartCoroutine(executeAfterTime(DecreaseDamage, secondes, value));
    }

    public void IncreaseDamage(int value)
    {
        this.Damage += value;
    }

    public void DecreaseDamage(int value)
    {
        this.Damage -= value;
        if(this.Damage < 1)
        {
            this.Damage = 0;
        }
    }

    private IEnumerator executeAfterTime(Action<int> function, int secondes, int value)
    {
        yield return new WaitForSeconds(secondes);
        function(value);
    }
}
