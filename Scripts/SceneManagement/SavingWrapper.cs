using System.Collections;
using System.Collections.Generic;
using Tears_Of_Void.Saving;
using UnityEngine;

namespace Tears_Of_Void.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 0.2f;
        
        private void Awake() 
        {
           StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene() 
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        }
        
        private void Update() 
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}