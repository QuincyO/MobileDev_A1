using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Quincy.Interfaces
{
    public interface IDamageable
    { 
        public void TakeDamage(float damage,GameObject attacker);
        public void Heal(float heal);
        
    }
}

