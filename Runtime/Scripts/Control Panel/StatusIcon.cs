using System.Collections;
using UnityEngine;


namespace BeamXR.Director.ControlPanel
{
    public class StatusIcon : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private UnityEngine.UI.Image _image;

        private Color _color, _transparentColor;
        private bool _pulsing = false;

        private float _pulseRate = 1f;

        private void OnValidate()
        {
            if (_image == null)
            {
                _image = GetComponent<UnityEngine.UI.Image>();
            }
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetColor(Color color)
        {
            _color = color;
            color.a = 0f;
            _transparentColor = color;
            if (!_pulsing)
            {
                _image.color = _color;
            }
        }

        public void SetPulsing(bool pulsing, float rate = -1f)
        {
            if (rate != -1)
            {
                _pulseRate = rate;
            }

            bool wasPulsing = _pulsing;
            _pulsing = pulsing;

            if (pulsing && !wasPulsing)
            {
                StartCoroutine(Pulsing());
            }
            else if (!pulsing && wasPulsing)
            {
                _image.color = _color;
                StopCoroutine(Pulsing());
            }
        }

        IEnumerator Pulsing()
        {
            for (; ; )
            {
                if (!_pulsing)
                {
                    break;
                }
                _image.color = Color.Lerp(_color, _transparentColor, (Time.unscaledTime * _pulseRate) % 1f);
                yield return null;
            }
        }

        private void OnEnable()
        {
            if (_pulsing)
            {
                StartCoroutine(Pulsing());
            }
        }
    }
}