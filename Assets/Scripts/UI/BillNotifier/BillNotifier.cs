using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BillNotifier : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Text _text;

    private Transform _cameraTransform;
    private Coroutine _faceCameraCoroutine;
    private Vector3 _defaultScale;
    
    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _defaultScale = transform.localScale;
    }
    
    private IEnumerator FaceCamera(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.LookAt(transform.position + _cameraTransform.forward, Vector3.up);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public async void ShowFloatingBill(int amount)
    {
        _text.text = $"+{amount}";
        _canvas.enabled = true;
        transform.localScale = Vector3.zero;
        transform.DOScale(_defaultScale, 0.25f);
        
        if(_faceCameraCoroutine != null)
            StopCoroutine(_faceCameraCoroutine);
        _faceCameraCoroutine = StartCoroutine(FaceCamera(3));
        
        await UniTask.Delay(3000);
        
        _canvas.enabled = false;
    }
}
