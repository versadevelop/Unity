using UnityEngine;
using UnityEngine.UI;
using Tears_Of_Void.Combat;
using System;

namespace Tears_Of_Void.Resources
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] Fighter fighter;
        [SerializeField] AIHealth health;
        [SerializeField] Image fill;
        [SerializeField] Text currentHealth;
        float healthPercentage;
        
        private void OnEnable()
        {
            fighter.onDamageTaken += UpdateEnemyHealth;
        }
        private void OnDisable()
        {
            fighter.onDamageTaken -= UpdateEnemyHealth;
        }
        public void UpdateEnemyHealth()
        {
            health = fighter.GetTarget();
            if(health == null) return;
            healthPercentage = health.GetHealthPct();
            GetComponent<Text>().text = String.Format("{0:0}%", healthPercentage);
            currentHealth.text = health.GetCurrentHealth().ToString();
            fill.fillAmount = healthPercentage / 100;
        }
    }
}