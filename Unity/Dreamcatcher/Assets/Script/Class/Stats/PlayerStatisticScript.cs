using UnityEngine;
using System.Collections;

public class PlayerStatisticScript : EntityStatisticScript
{

    public override void TakeDamage(int damage)
    {
        this.CurrentHealth -= damage;
        if(this.CurrentHealth <= 0)
        {
            Application.LoadLevel("EndScene");
        }
    }
}
