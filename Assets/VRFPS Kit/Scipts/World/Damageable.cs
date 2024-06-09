using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    /// <summary>
    /// Simply tracks health, allowing for further behaviour extention by composition
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        public float health;
        [HideInInspector] public float startHealth;

        private void Start()
        {
            startHealth = health;
        }
        
        public void TakeDamage(float damage)
        {
            health -= damage;
        }
        
        public void ResetHealth()
        {
            health = startHealth;
        }
    }
}