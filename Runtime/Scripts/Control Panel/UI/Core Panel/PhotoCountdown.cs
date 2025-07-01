using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class PhotoCountdown : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _countdownText = null;

        [SerializeField]
        private Image _backgroundImage = null;

        [SerializeField]
        private Color _backgroundColor = Color.black;
        private Color _transparentWhite = new Color(1, 1, 1, 0);

        [SerializeField]
        private float _flashTime = 0.2f;

        public void StartCountdown(float time)
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
            StartCoroutine(Countdown(time));
        }

        private IEnumerator Countdown(float time)
        {
            time -= Time.unscaledDeltaTime;
            int rounded = (int)time+1;
            _countdownText.text = rounded.ToString();

            float lerp = time % 1;
            _backgroundImage.color = _backgroundColor;
            while(time > 0)
            {
                if((int)time + 1 != rounded)
                {
                    rounded = (int)time + 1;
                    _countdownText.text = rounded.ToString();
                }

                lerp = time % 1;

                _countdownText.transform.localScale = Vector3.Lerp(Vector3.one * 0.75f, Vector3.one, lerp);

                time -= Time.unscaledDeltaTime;
                yield return null;
            }
            float flashTime = _flashTime;
            _countdownText.text = "";
            while (flashTime > 0)
            {
                flashTime -= Time.unscaledDeltaTime * (1.0f / _flashTime);
                _backgroundImage.color = Color.Lerp(_transparentWhite, Color.white, flashTime);
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}