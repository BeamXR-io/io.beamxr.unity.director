using System.Collections;
using System.Collections.Generic;
using BeamXR.Streaming.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class BeamPanelGroup : MonoBehaviour
    {
        [System.Serializable]
        public class PanelInfo
        {
            public Button button;
            public ButtonAnimator animator;
            public GameObject panel;
        }

        public List<PanelInfo> _panels = new List<PanelInfo>();

        private PanelInfo _currentPanel = null;
        private PanelInfo _previousPanel = null;

        protected virtual void Awake()
        {
            for (int i = 0; i < _panels.Count; i++)
            {
                PanelInfo panel = _panels[i];
                _panels[i].button.onClick.AddListener(() => ChangePanel(panel));
            }
        }

        public void OpenPanel(GameObject panel)
        {
            int ind = -1;
            ind = _panels.FindIndex(x => x.panel == panel);

            if (ind == -1)
            {
                BeamLogger.LogError($"Unable to find panel: {panel.name}");
                return;
            }
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
            ChangePanel(_panels[ind]);
        }

        public void GoToPreviousPanel()
        {
            if (_previousPanel == null)
                return;

            ChangePanel(_previousPanel);
            _previousPanel = null;
        }

        private void ChangePanel(PanelInfo info)
        {
            _previousPanel = _currentPanel;
            for (int i = 0; i < _panels.Count; i++)
            {
                _panels[i].animator.ForcePressed(info == _panels[i]);
                _panels[i].panel.SetActive(info == _panels[i]);
            }
            _currentPanel = info;
        }

        protected virtual void OnEnable()
        {
            if (_currentPanel == null)
            {
                ChangePanel(_panels[0]);
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < _panels.Count; i++)
            {
                if (_panels[i].button == null && _panels[i].animator != null)
                {
                    _panels[i].animator = null;
                }

                if (_panels[i].button != null && _panels[i].animator == null)
                {
                    _panels[i].animator = _panels[i].button.GetComponent<ButtonAnimator>();
                }
            }
        }
    }
}