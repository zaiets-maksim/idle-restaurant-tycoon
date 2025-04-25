using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPhrasesViewer : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private List<string> _phrases;
    [SerializeField] private float _phraseSwitchInterval = 1.5f;

    private Coroutine _coroutine;
    private int _currentIndex;

    private void Start()
    {
        _coroutine = StartCoroutine(ShowPhrases());
    }

    private void OnDisable()
    {
        Stop();
    }

    private IEnumerator ShowPhrases()
    {
        while (true)
        {
            yield return new WaitForSeconds(_phraseSwitchInterval);
            
            _text.text = _phrases[_currentIndex];

            if (_currentIndex == _phrases.Count - 1)
                _currentIndex = 0;
            else
                _currentIndex++;
        }

    }

    public void Stop() => StopCoroutine(_coroutine);
}


