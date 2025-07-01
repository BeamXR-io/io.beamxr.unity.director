using TMPro;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class DeviceCodeUI : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        TextMeshProUGUI[] _textElements;

        private void Awake()
        {
            if (_textElements == null || _textElements.Length != 8)
            {
                _textElements = GetComponentsInChildren<TextMeshProUGUI>();
            }
        }

        private void OnValidate()
        {
            if (_textElements == null || _textElements.Length != 8)
                _textElements = GetComponentsInChildren<TextMeshProUGUI>();
        }

        public void SetCode(string code)
        {
            int ind = code.IndexOf('-');
            code = code.Remove(ind, 1);
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == '-')
                {
                    i--;
                    continue;
                }

                _textElements[i].text = code[i].ToString();
            }
        }
    }
}