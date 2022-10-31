using System.Collections.Generic;

namespace Matterless.Localisation
{
    [System.Serializable]
    public class LocalisationModel
    {
        public int version { get; set; }
        public Dictionary<string, string> dictionary { get; set; }
    }
}
