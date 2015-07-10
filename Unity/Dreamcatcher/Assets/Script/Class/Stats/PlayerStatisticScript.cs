using UnityEngine;
using System.Collections;

public class PlayerStatisticScript : EntityStatisticScript
{

    private int comboPoints = 0;

    public override void TakeDamage(int damage)
    {
		Debug.Log ("player taking damage");
        this.CurrentHealth -= damage;
        if(this.CurrentHealth <= 0)
        {
            Application.LoadLevel("EndScene");
        }
    }

    public void AddComboPoints(int points)
    {
        comboPoints += points;
    }
}
