using Tears_Of_Void.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Tears_Of_Void.Resources
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        private void Start()
        {
            GetComponent<Text>().text = baseStats.GetLevel() + "";
        }
        private void OnEnable()
        {
            baseStats.onLevelUp += UpdateText;
        }
        private void OnDisable()
        {
            baseStats.onLevelUp -= UpdateText;
        }
        private void UpdateText()
        {
            GetComponent<Text>().text = baseStats.GetLevel() + "";
        }
    }
}