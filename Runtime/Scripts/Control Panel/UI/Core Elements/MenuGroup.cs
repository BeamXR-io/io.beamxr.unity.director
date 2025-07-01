using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class MenuGroup : MonoBehaviour
    {
        private HashSet<ButtonAnimator> _animator = null;

        private void Awake()
        {
            FindParts();
            ApplyToggles();
        }

        private void FindParts()
        {
            if (_animator == null || _animator.Count == 0)
            {
                _animator = GetComponentsInChildren<ButtonAnimator>(true).ToHashSet();
            }
        }

        private void ApplyToggles()
        {
            foreach (var button in _animator)
            {
                button.OnClick += () => { ChangeSelected(button); };
            }
        }

        public void ChangeSelected(ButtonAnimator animator)
        {
            FindParts();

            foreach (var button in _animator)
            {
                if (button == animator)
                {
                    button.ForcePressed(true);
                }
                else
                {
                    button.ForcePressed(false);
                }
            }
        }

        public void ToggleInteractable(bool interactable)
        {
            FindParts();

            foreach (var button in _animator)
            {
                button.Selectable.interactable = interactable;
            }
        }

        private void OnEnable()
        {
            float z = 0;
            foreach (var item in _animator)
            {
                if(item._initialZValue > z)
                {
                    z = item._initialZValue;
                }
            }
            
            if (z == 0)
                return;

            foreach (var item in _animator)
            {
                item._initialZValue = z;
            }
        }
    }
}