using UnityEngine;
using DuloGames.UI;
using Tears_Of_Void.Stats;
using System.Collections;
using System;
using UnityEngine.UI;
using Tears_Of_Void.Saving;

namespace Tears_Of_Void.Resources
{
    public class ExperienceBar : MonoBehaviour, ISaveable
    {
        float totalExperience;
        float experienceNeedToNextLevel;
        public Slider experienceSlider;
        BaseStats getExpForNextLevel;
        [SerializeField] ParticleSystem particleEffect;
        [SerializeField] Text text;
        Experience experience;
        Coroutine routine;
        float time;
        int counter = 1;
        float temp;
        float saveBarValue;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            getExpForNextLevel = GetComponent<BaseStats>();
        }
        private void Start()
        {
            counter = getExpForNextLevel.GetLevel();
            totalExperience = experience.GetExperiencePoints();
            if (getExpForNextLevel.GetLevel() <= 1)
            {
                experienceSlider.maxValue = CalculateLevelOneBarValues();
                text.text = totalExperience + "/" + CalculateLevelOneBarValues();
                experienceSlider.value = totalExperience;
            }
            else
            {
                experienceSlider.maxValue = CalculateBarValues();
                text.text = CalculateTotalExperienceMinusLastLevel() + "/" + CalculateBarValues();
                experienceSlider.value = CalculateTotalExperienceMinusLastLevel();
                totalExperience = experienceSlider.value;
            }
            var main = particleEffect.main;
            main.startColor = Color.green;
        }

        private float CalculateTotalExperienceMinusLastLevel()
        {
            if (counter == 1)
            {
                return totalExperience;
            }
            return totalExperience - getExpForNextLevel.XpToLevelUp(getExpForNextLevel.GetLevel() - 1);
        }

        private float CalculateLevelOneBarValues()
        {
            return getExpForNextLevel.XpToLevelUp(getExpForNextLevel.GetLevel());
        }

        private float CalculateBarValues()
        {
            return (getExpForNextLevel.XpToLevelUp(getExpForNextLevel.GetLevel()) - getExpForNextLevel.XpToLevelUp(getExpForNextLevel.GetLevel() - 1));
        }

        public void UpdateExperienceBar(float xp, float duration = 0.1f)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }

            routine = StartCoroutine(FillBar(xp, 3.1f));
        }

        private IEnumerator FillBar(float exp, float duration)
        {
            time = 0;
            totalExperience += exp;
            if (counter == 1)
            {
                text.text = totalExperience + "/" + 10;
            }
            else
            {
                text.text = totalExperience + "/" + (getExpForNextLevel.XpToLevelUp(counter) - getExpForNextLevel.XpToLevelUp(counter - 1));
            }
            particleEffect.Play();
            while (time < duration)
            {
                time += Time.deltaTime;
                float percent = time / duration;
                experienceSlider.value = Mathf.Lerp(experienceSlider.value, totalExperience, percent);
                saveBarValue = experienceSlider.value;

                if (totalExperience >= experienceSlider.maxValue)
                {
                    temp = Math.Abs(experienceSlider.maxValue - totalExperience);
                    IncreaseLevel();
                }
                yield return null;
            }
            particleEffect.Stop();
        }

        public void IncreaseLevel()
        {
            counter++;
            totalExperience = 0;
            totalExperience += temp;
            float exp = (getExpForNextLevel.XpToLevelUp(counter) - getExpForNextLevel.XpToLevelUp(counter - 1));
            text.text = totalExperience + "/" + exp;
            experienceNeedToNextLevel = exp;
            experienceSlider.maxValue = experienceNeedToNextLevel;
        }

        public object CaptureState()
        {
            return new Tuple<float ,float>(saveBarValue, totalExperience);
        }

        public void RestoreState(object state)
        {
            (float saveBarValue,float totalExperience) = (Tuple<float ,float>)state;
        }
    }
}