using UnityEngine;
using Tears_Of_Void.Resources;
using System;
using Tears_Of_Void.Saving;
using Tears_Of_Void.Items;
using Tears_Of_Void.Combat;
using System.Collections.Generic;
using DuloGames.UI;

namespace Tears_Of_Void.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 60)]
        [SerializeField] int progressionLevel = 1;
        public CharacterClass characterClass = default;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject LevelUpUI = null, LevelUpParticles = null, canvas = null;
        GameObject leveledUp, lvlParticles;
        Experience experience;
        public Fighter m_Owner { get; set; }
        AIHealth currentHealth;
        AIHealth target;
        LazyValue<int> currentLevel;

        public event Action onLevelUp;

        public enum DamageType
        {
            Physical,
            Fire,
            Cold,
            Electric
        }
        public int[] elementalProtection = new int[Enum.GetValues(typeof(DamageType)).Length];
        public int[] elementalBoosts = new int[Enum.GetValues(typeof(DamageType)).Length];
        public List<BaseElementalEffect> ElementalEffects => m_ElementalEffects;
        List<BaseElementalEffect> m_ElementalEffects = new List<BaseElementalEffect>();
        public void AddElementalEffect(BaseElementalEffect effect)
        {
            target = m_Owner.GetTarget();
            effect.Applied(target);

            bool replaced = false;
            for (int i = 0; i < m_ElementalEffects.Count; ++i)
            {
                if (effect.Equals(m_ElementalEffects[i]))
                {
                    replaced = true;
                    m_ElementalEffects[i].Removed();
                    m_ElementalEffects[i] = effect;
                }
            }

            if (!replaced)
                m_ElementalEffects.Add(effect);
        }

        public void Tick()
        {
            for (int i = 0; i < m_ElementalEffects.Count; ++i)
            {
                var effect = m_ElementalEffects[i];
                effect.Update(this);

                if (effect.Done || target.IsDead())
                {
                    m_ElementalEffects[i].Removed();
                    m_ElementalEffects.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Death()
        {
            foreach (var e in ElementalEffects)
                e.Removed();

            ElementalEffects.Clear();
        }

        public void Damage(Weapons.AttackData attackData)
        {
            int totalDamage = attackData.GetFullDamage();

            target.TakeDamage(m_Owner.gameObject, totalDamage, false);
        }

        private void Awake()
        {
            experience = GetComponent<Experience>();
            m_Owner = GetComponent<Fighter>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
            progressionLevel = currentLevel.value;
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                progressionLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
                Destroy(leveledUp, 3f);
            }
        }

        private void LevelUpEffect()
        {

            leveledUp = Instantiate(LevelUpUI, canvas.transform);
            leveledUp.SetActive(true);

            lvlParticles = Instantiate(LevelUpParticles, gameObject.transform);
            lvlParticles.transform.position = new Vector3(lvlParticles.transform.position.x, 0f, lvlParticles.transform.position.z);
        }

        public float XpToLevelUp(int i)
        {
            return progression.GetStat(Stat.ExperienceToNextLevel, characterClass, i);
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, progressionLevel) + GetAdditiveModifier(stat);
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private int CalculateLevel()
        {
            experience = GetComponent<Experience>();
            if (experience == null) return progressionLevel;
            float currentExperience = experience.GetExperiencePoints();
            int maxLevel = progression.GetLevels(Stat.ExperienceToNextLevel, characterClass);
            for (int level = 1; level <= maxLevel; level++)
            {
                float XpToLevelUp = progression.GetStat(Stat.ExperienceToNextLevel, characterClass, level);
                if (XpToLevelUp > currentExperience)
                {
                    return level;
                }
            }
            return maxLevel + 1;
        }
    }
}