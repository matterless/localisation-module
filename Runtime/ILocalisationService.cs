using System;
using UnityEngine;

namespace Matterless.Localisation
{
    public interface ILocalisationService
    {
        event Action onLanguageChanged;
        void SetLanguage(LocalisationModel model);
        string Translate(string textTag, params object[] args);
        void RegisterUnityUIComponents(GameObject rootObject);
    }
}