using UnityEngine;
using System;
using System.Collections;

public abstract class EntityStatisticScript : MonoBehaviour {

    [SerializeField]
    protected int MaxHealth;
    [SerializeField]
    protected int CurrentHealth;
    [SerializeField]
    protected int Damage;

	[SerializeField]
	protected int EntityType; // à garder pour identifier lors du pulling

	[SerializeField]
	protected UnitManager unitManager; // Lorsque l'on prends des dégats et qu'on meurt, il faut prévenir de la mort de l'unité

    public abstract void TakeDamage(int damage);

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

    protected IEnumerator executeAfterTime(Action<int> function, float secondes, int value)
    {
        yield return new WaitForSeconds(secondes);
        function(value);
    }

    public int getDamage()
    {
        return Damage;
    }

	public int getCurrentHealth()
	{
		return CurrentHealth;
	}

	public int getEntityType()
	{
		return EntityType;
	}
}
