using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable  
{
    public void TakeDamage(float howMuch);
    public void Die();
}
