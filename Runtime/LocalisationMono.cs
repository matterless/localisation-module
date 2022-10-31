using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Matterless.Localisation
{
    public class LocalisationMono : MonoBehaviour
    {
        private Text m_Text;
        private TextMeshProUGUI m_TMPro;
        private string m_TextTag;

        internal static LocalisationMono Create(Text textComponent, string textTag)
        {
            var mono = textComponent.gameObject.AddComponent<LocalisationMono>();
            mono.m_Text = textComponent;
            mono.m_TextTag = textTag;
            return mono;
        }

        internal static LocalisationMono Create(TextMeshProUGUI textComponent, string textTag)
        {
            var mono = textComponent.gameObject.AddComponent<LocalisationMono>();
            mono.m_TMPro = textComponent;
            mono.m_TextTag = textTag;
            return mono;
        }

        internal void Translate(ILocalisationService localisationService)
        {
            if (m_Text != null)
                m_Text.text = localisationService.Translate(m_TextTag);

            if (m_TMPro != null)
                m_TMPro.text = localisationService.Translate(m_TextTag);
        }
    }
}