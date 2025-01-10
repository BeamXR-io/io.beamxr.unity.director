using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class AudioEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler
    {

        [SerializeField, Tooltip("If true, no audio will play when the UI element's interactable state is false.")]
        private bool _playOnlyWhenInteractable = true;

        private Selectable _selectable;
        private bool _interactable { get { return _selectable != null ? _selectable.interactable : true; } }

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField, Space]
        private AudioEvent _pointerEnter;
        [SerializeField]
        private AudioEvent _pointerClick, _pointerDown;

        [System.Serializable]
        public class AudioEvent
        {
            public AudioClip clip;
            public float pitch;
        }

        private void OnValidate()
        {
            FindParts();
        }

        private void Awake()
        {
            FindParts();
        }

        private void FindParts()
        {
            if (_selectable == null)
            {
                _selectable = GetComponent<Selectable>();
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            PlayAudio(_pointerEnter);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            PlayAudio(_pointerClick);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            PlayAudio(_pointerDown);
        }

        private void PlayAudio(AudioEvent audioEvent)
        {
            if (_playOnlyWhenInteractable)
            {
                if (!_interactable)
                    return;
            }

            if (_audioSource.isActiveAndEnabled)
            {
                _audioSource.pitch = audioEvent.pitch;
                _audioSource.PlayOneShot(audioEvent.clip);
            }
        }
    }
}