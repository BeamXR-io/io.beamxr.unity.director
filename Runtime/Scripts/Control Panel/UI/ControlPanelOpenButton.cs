using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class ControlPanelOpenButton : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private BeamControlPanel _controlPanel = null;

        private void Awake()
        {
            FindParts();
        }

        private void OnValidate()
        {
            FindParts();
        }

        private void FindParts()
        {
            if(_controlPanel == null)
            {
                _controlPanel = FindFirstObjectByType<BeamControlPanel>(FindObjectsInactive.Include);
            }
        }

        public void ToggleControlPanel()
        {
            if (_controlPanel != null)
            {
                _controlPanel.ToggleControlPanel();
            }
        }
    }
}