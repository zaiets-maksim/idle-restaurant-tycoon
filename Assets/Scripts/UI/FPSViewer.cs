using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPSViewer : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private int _targetFPS = 60; 
    [SerializeField] private float _refreshDelay = 0.5f;
    
    private int _fps;

    private void Awake()
    {
        SetTargetFPS();
        StartCoroutine(RefreshFPSCounter());
    }

    private void SetTargetFPS() => Application.targetFrameRate = _targetFPS;

    private IEnumerator RefreshFPSCounter()
    {
        while (true)
        {
            _fps = Mathf.CeilToInt(1.0f / Time.deltaTime);
            _text.text = _fps.ToString();

            yield return new WaitForSeconds(_refreshDelay);
        }
    }
}