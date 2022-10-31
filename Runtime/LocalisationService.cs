using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Matterless.Localisation
{
    public class LocalisationService : ILocalisationService
    {
        private const string PREFIX = "t::";

        // localisation dictionary
        private Dictionary<string, string> m_Dictionary;
        // list of text ui components
        private List<Text> m_TextList = new List<Text>();
        // list of TMPro ui components
        private List<TextMeshProUGUI> m_TMProList = new List<TextMeshProUGUI>();


        private void LocaliseAllUiComponents()
        {
            // lasy -> remove null values from lists
            m_TextList.RemoveAll(item => item == null);
            m_TMProList.RemoveAll(item => item == null);

            // localise all text
            foreach (var item in m_TextList)
                Localise(item);

            // localise all tmpro
            foreach (var item in m_TMProList)
                Localise(item);
        }

        private void Localise(Text uiText)
        {
            // translate if the string starts with the PREFIX
            if (HasPrefix(uiText.text))
                uiText.text = Translate(uiText.text);
        }

        private void Localise(TextMeshProUGUI uiText)
        {
            // translate if the string starts with the PREFIX
            if (HasPrefix(uiText.text))
                uiText.text = Translate(uiText.text);
        }

        private bool HasPrefix(string text) => text.Length > PREFIX.Length && text.StartsWith(PREFIX);

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
            foreach (var uiText in rootObject.GetComponentsInChildren<Text>())
            {
                Localise(uiText);
                m_TextList.Add(uiText);
            }

            // register all TextMeshProUGUI in children
            foreach (var uiText in rootObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                Localise(uiText);
                m_TMProList.Add(uiText);
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