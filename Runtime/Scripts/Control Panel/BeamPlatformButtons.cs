using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BeamXR.Streaming.Core;
using System.Collections;
using BeamXR.Streaming.Core.Models;
using System.Collections.Generic;

namespace BeamXR.Director.ControlPanel
{
    public class BeamPlatformButtons : BeamComponent
    {
        [SerializeField]
        private GameObject _buttonPrefab;

        public class StreamButton
        {
            public Button button;
            public TextMeshProUGUI text;
            public StreamPlatform platform;
        }

        private List<StreamButton> _buttons = new List<StreamButton>();

        protected override void Awake()
        {
            base.Awake();
            _unityEvents.OnStreamStarted.AddListener(StreamStarted);
            _unityEvents.OnStreamEnded.AddListener(StreamEnded);
        }

        private void OnEnable()
        {
            RefreshButtonStates();
            _unityEvents.OnStreamPlatformStateChanged.AddListener(StreamPlatformChanged);
        }

        private void OnDisable()
        {
            _unityEvents.OnStreamPlatformStateChanged.RemoveListener(StreamPlatformChanged);
        }

        private void OnDestroy()
        {
            if (_unityEvents != null)
            {
                _unityEvents.OnStreamStarted.RemoveListener(StreamStarted);
                _unityEvents.OnStreamEnded.RemoveListener(StreamEnded);
            }
        }

        private void StreamStarted()
        {
            ClearButtons();
            foreach (var item in BeamStreamingManager.Instance.SessionState.StreamPlatforms)
            {
                AddButton(item);
            }
        }

        private void StreamEnded()
        {
            ClearButtons();
        }

        private void RefreshButtonStates()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].button.interactable = true;
                UpdateButton(_buttons[i]);
            }
        }

        private void StreamPlatformChanged(StreamPlatform platform)
        {
            int ind = _buttons.FindIndex(x => x.platform == platform);
            if(ind != -1)
            {
                UpdateButton(_buttons[ind]);
            }
        }

        private void AddButton(StreamPlatform platform)
        {
            GameObject go = GameObject.Instantiate(_buttonPrefab, transform);
            StreamButton streamButton = new StreamButton()
            {
                button = go.GetComponentInChildren<Button>(),
                text = go.GetComponentInChildren<TextMeshProUGUI>(),
                platform = platform
            };

            streamButton.button.onClick.AddListener(() => ClickButton(streamButton));

            UpdateButton(streamButton);

            _buttons.Add(streamButton);
        }

        private void UpdateButton(StreamButton button)
        {
            button.text.text = (button.platform.IsStreaming ? "Stop" : "Start") + "\n" + button.platform.GetVisualPlatformName();
        }

        private void ClickButton(StreamButton button)
        {
            StartCoroutine(DelayClick(button));
            BeamStreamingManager.Instance.ToggleSocialStream(button.platform);
        }

        private IEnumerator DelayClick(StreamButton button)
        {
            button.button.interactable = false;
            yield return new WaitForSeconds(5f);
            button.button.interactable = true;
        }

        private void ClearButtons()
        {
            if (_buttons == null)
                return;

            foreach (var button in _buttons)
            {
                if(button != null && button.button != null && button.button.gameObject != null)
                {
                    Destroy(button.button.gameObject);
                }
            }
        }
    }
}