using UnityEngine;
using System.Collections;

public class MobStatisticScript : EntityStatisticScript
{

	int Resistance = 0;

	public bool isAttacking = false;
	public bool isDefending = false;

	public override void TakeDamage(int damage)
	{
		Debug.Log ("mob taking damage");
		if (this.CurrentHealth <= 0) // evite le overkill
			return; 
		this.CurrentHealth -= Mathf.Max(0, damage - Resistance); // dommages >= 0
		if(this.CurrentHealth <= 0)
		{
			base.unitManager.OnUnitDeath(this);
		}
	}

	public void SetDamage(int dmg) { Damage = dmg; }
	public void SetCurrentHealth (int health) { CurrentHealth = health;}
	public void SetMaxHealth (int health) { MaxHealth = health;}
	public void SetResistance (int res) {  Resistance = res; }

	public void TemporaryIncreaseResistance(int value, float secondes)
	{
		this.IncreaseDamage(value);
		StartCoroutine(executeAfterTime(DecreaseResistance, secondes, value));
	}
	
	public void IncreaseResistance(int value)
	{
		this.Resistance += value;
		isDefending = true;
	}
	
	public void DecreaseResistance(int value)
	{
		this.Resistance -= value;
		isDefending = false;
	}

}
