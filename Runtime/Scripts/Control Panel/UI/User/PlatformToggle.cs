using TMPro;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class PlatformToggle : MonoBehaviour
    {
        [SerializeField]
        private ToggleAnimator _toggle;
        public ToggleAnimator Toggle => _toggle;

        [SerializeField]
        private TextMeshProUGUI _text;
        public TextMeshProUGUI Text => _text;
    }
}