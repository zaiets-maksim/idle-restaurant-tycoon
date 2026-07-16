using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Connect4.Scripts.Infrastructure
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private Animation _animation;
        [SerializeField] private bool _isCustomDelay;
        [SerializeField] private float _delay = 0.5f;

        [Header("Move Up Settings")]
        [SerializeField] private float _moveUpDuration = 0.6f;
        [SerializeField] private Ease _moveUpEase = Ease.InOutQuad;
        [SerializeField] private Image Image;

        public event Action OnComplete;


        private Tween _moveTween;

        private void Awake() => DontDestroyOnLoad(this);

        public void Show()
        {
            Image.rectTransform.anchoredPosition = Vector2.zero;
            gameObject.SetActive(true);
            _animation.Play();
        }

        public void Hide() => StartCoroutine(GoUp());

        private IEnumerator GoUp()
        {
            float delay = _isCustomDelay ? _delay : _animation.clip.length;
            yield return new WaitForSeconds(delay);
            if (!_isCustomDelay) _animation.Stop();

            float targetY = Image.rectTransform.rect.height;

            _moveTween?.Kill();
            _moveTween = Image.rectTransform
                .DOAnchorPosY(targetY, _moveUpDuration)
                .SetEase(_moveUpEase)
                .SetLink(gameObject);

            yield return _moveTween.WaitForCompletion();

            OnComplete?.Invoke();
            gameObject.SetActive(false);
            _animation.Stop();
            _animation.Rewind();
        }
    }
}