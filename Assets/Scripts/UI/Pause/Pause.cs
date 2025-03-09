using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _button;

    private void Start() => _button.onClick.AddListener(Method);

    private void Method()
    {
        _panel.SetActive(true);
        
        Animation _anim = _panel.GetComponent<Animation>(); 
        _anim.Play("PanelAnimationEnable");

        //SaveLoad save = new SaveLoad();
        
        //save.SaveScene(SceneManager.GetActiveScene().name);

        //thisObj.GetComponent<UIController>().enabled = false;
        GetComponent<Image>().enabled = false;
    }
}
