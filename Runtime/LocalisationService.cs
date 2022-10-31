using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Scripting;

namespace Matterless.Localisation
{
    public class LocalisationService : ILocalisationService
    {
        private const string PREFIX = "t::";

        // localisation dictionary
        private Dictionary<string, string> m_Dictionary;
        // list of LocalisationMono components
        private List<LocalisationMono> m_LocalisationMonoList;

        [Preserve]
        public LocalisationService()
        {
            m_LocalisationMonoList = new List<LocalisationMono>();
        }

        private void LocaliseAllUiComponents()
        {
            // lasy -> remove null values from lists
            m_LocalisationMonoList.RemoveAll(item => item == null);

            // localise all text
            foreach (var item in m_LocalisationMonoList)
                item.Translate(this);
        }

        private bool HasPrefix(string text) => text.Length > PREFIX.Length && text.StartsWith(PREFIX);

        private string GetPrefix(string text) => text.Substring(PREFIX.Length, text.Length - PREFIX.Length);

        #region ILocalisationService
        public event Action onLanguageChanged;

        public void SetLanguage(LocalisationModel model)
        {
            if(model == null)
                throw new Exception("Localisation model is null.");

            if(model.dictionary == null)
                throw new Exception("Localisation dictionary in model is null.");

            // set dictionary
            m_Dictionary = model.dictionary;
            // invoke event
            onLanguageChanged?.Invoke();
            // localise all ui
            LocaliseAllUiComponents();
        }

        public void RegisterUnityUIComponents(GameObject rootObject)
        {
            // register all Text in children
            foreach (var uiText in rootObject.GetComponentsInChildren<Text>(true))
            {
                if (HasPrefix(uiText.text))
                {
                    var localisationMono = LocalisationMono.Create(uiText, GetPrefix(uiText.text));
                    localisationMono.Translate(this);
                    m_LocalisationMonoList.Add(localisationMono);
                }
            }

            // register all TextMeshProUGUI in children
            foreach (var uiText in rootObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                var localisationMono = LocalisationMono.Create(uiText, GetPrefix(uiText.text));
                localisationMono.Translate(this);
                m_LocalisationMonoList.Add(localisationMono);
            }
        }

        public string Translate(string textTag, params object[] args)
        {
            if (m_Dictionary == null)
                throw new Exception("Localisation dictionary is null.");

            if (m_Dictionary.ContainsKey(textTag))
                return string.Format(m_Dictionary[textTag], args);

            Debug.LogWarning($"TextTag is missing from dictionary: {textTag}");

            return textTag;
        }
        #endregion
    }
}