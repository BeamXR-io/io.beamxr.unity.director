using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class PanelReturn : MonoBehaviour
    {
        [SerializeField]
        private BeamPanelGroup _panelGroup = null;

        [SerializeField]
        private ButtonAnimator[] _buttons = null;

        private bool _bound = false;

        private void FindParts()
        {
            if(_buttons == null || _buttons.Length == 0)
            {
                _buttons = GetComponentsInChildren<ButtonAnimator>(true);
            }
        }

        private void OnEnable()
        {
            FindParts();
            if (!_bound)
            {
                foreach (var button in _buttons)
                {
                    button.OnClick += () => _panelGroup.GoToPreviousPanel();
                }
            }
        }

        private void OnValidate()
        {
            FindParts();
        }
    }
}