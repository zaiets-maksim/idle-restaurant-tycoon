using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPhrasesViewer : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private List<string> _phrases;
    [SerializeField] private float _phraseSwitchInterval = 1.5f;

    private List<string> _shuffledPhrases;
    private Coroutine _coroutine;
    private int _currentIndex;

    private void Start()
    {
        ShufflePhrases();
        _coroutine = StartCoroutine(ShowPhrases());
    }

    private void OnDisable()
    {
        Stop();
    }

    private void ShufflePhrases()
    {
        _shuffledPhrases = _phrases.OrderBy(_ => Random.value).ToList();
        _currentIndex = 0;
    }
    
    private IEnumerator ShowPhrases()
    {
        while (true)
        {
            yield return new WaitForSeconds(_phraseSwitchInterval);
            
            _text.text = _shuffledPhrases[_currentIndex];

            if (_currentIndex == _shuffledPhrases.Count - 1)
                _currentIndex = 0;
            else
                _currentIndex++;
        }

    }

    public void Stop() => StopCoroutine(_coroutine);
}


