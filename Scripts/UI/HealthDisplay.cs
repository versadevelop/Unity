using System;
using Tears_Of_Void.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Tears_Of_Void.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Image fill,fillGlobe;
        [SerializeField] Text textCurrentHealth;
        [SerializeField] Health healthComponent = null;

        private void OnEnable()
        {
            healthComponent.onDamageTaken += UpdateUI;
        }

        private void OnDisable()
        {
            healthComponent.onDamageTaken -= UpdateUI;
        }

        public void UpdateUI()
        {
            float healthPct = healthComponent.GetHealthPct();
            GetComponent<Text>().text = String.Format("{0:0}%", healthPct);
            textCurrentHealth.text = healthComponent.GetCurrentHealth().ToString();
            float calculateHealth = healthPct / 100;
            fill.fillAmount = calculateHealth ;
            fillGlobe.fillAmount = calculateHealth;
        }
    }
}