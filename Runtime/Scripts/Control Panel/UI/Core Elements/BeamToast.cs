using System.Collections;
using TMPro;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class BeamToast : MonoBehaviour
    {
        private Vector3 _originalScale = Vector3.zero;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private float _transitionTime = 0.2f;

        [SerializeField]
        private float _transitionScale = 0.8f;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        internal void SetupToast(string message, float distance = 0.5f, float time = 3)
        {
            Position(distance);
            _text.text = message;
            gameObject.SetActive(true);
            StartCoroutine(ToastRoutine(time));
        }

        private void Position(float distance)
        {
            Transform camTransform = UnityEngine.Camera.main.transform;
            transform.rotation = camTransform.rotation;
            transform.position = camTransform.position;
            transform.position = camTransform.position + (camTransform.forward * distance);
            Vector3 pos = transform.position;
            pos.y -= 0.05f;
            transform.position = pos;
        }

        private IEnumerator ToastRoutine(float time)
        {
            yield return Animation(true);
            yield return new WaitForSeconds(time - (_transitionTime * 2f));
            yield return Animation(false);
            gameObject.SetActive(false);
        }

        private IEnumerator Animation(bool direction)
        {
            float lerp = 1;
            while (lerp > 0)
            {
                ApplyVisual(direction, lerp);
                lerp -= Time.unscaledDeltaTime * (1f / _transitionTime);
                yield return null;
            }
            lerp = 0;
            ApplyVisual(direction, lerp);
        }

        private void ApplyVisual(bool direction, float lerp)
        {
            _canvasGroup.alpha = direction ? 1f - lerp : lerp;
            transform.localScale = direction ? Vector3.Lerp(_originalScale, _originalScale * _transitionScale, lerp) : Vector3.Lerp(_originalScale * _transitionScale, _originalScale, lerp);
        }

        private void OnValidate()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            if (_text == null)
            {
                _text = GetComponentInChildren<TextMeshProUGUI>(true);
            }
        }
    }
}