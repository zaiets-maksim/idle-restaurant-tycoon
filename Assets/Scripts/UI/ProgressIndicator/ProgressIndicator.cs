using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ProgressIndicator
{
    public class ProgressIndicator : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _filled;
        
        private Coroutine _progressCoroutine;
        private Transform _cameraTransform;
        private Vector3 _defaultPosition;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            _defaultPosition = transform.position;
        }

        private void Update()
        {
            FaceCamera();
        }

        public void AddPosition(Vector3 position)
        {
            transform.position += position;
        }

        public void ResetPosition()
        {
            transform.position = _defaultPosition;
        }

        public void StartProgress(float duration)
        {
            Enable();
            
            if(_progressCoroutine != null)
                StopCoroutine(_progressCoroutine);
            
            _progressCoroutine = StartCoroutine(Fill(duration));
        }
        
        public void StartProgress(float duration, Action callback)
        {
            Enable();
            
            if(_progressCoroutine != null)
                StopCoroutine(_progressCoroutine);
            
            _progressCoroutine = StartCoroutine(Fill(duration, callback));
        }

        public  void Complete()
        {
            _filled.fillAmount = 1f;
        }

        public void Enable()
        {
            ResetProgress();
            _canvas.enabled = true;
        }

        public void Disable()
        {
            if (_progressCoroutine != null)
            {
                StopCoroutine(_progressCoroutine);
                _progressCoroutine = null;
            }

            _canvas.enabled = false;
        }

        private void FaceCamera()
        {
            transform.LookAt(transform.position + _cameraTransform.forward, Vector3.up);
        }

        private void ResetProgress()
        {
            _filled.fillAmount = 0f;
        }

        private IEnumerator Fill(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _filled.fillAmount = elapsed / duration;
                yield return null;
            }
            _filled.fillAmount = 1f;

            Disable();
        }
        
        private IEnumerator Fill(float duration, Action callback)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _filled.fillAmount = elapsed / duration;
                yield return null;
            }
            _filled.fillAmount = 1f;
            callback.Invoke();
            Disable();
        }
    }
}
