using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace StudentHistory.Scripts.UI.Puase
{
   public class Resume : MonoBehaviour
   {
      [SerializeField] private GameObject _panel;
      [SerializeField] private Button _button;
      [SerializeField] private Image _pauseButton;

      private void Start() => _button.onClick.AddListener( delegate { StartCoroutine(Method()); } );

      private IEnumerator Method()
      {
         Animation _anim = _panel.GetComponent<Animation>();
         AnimationClip clip = _anim.GetClip("PanelAnimationDisabled");
         _anim.Play("PanelAnimationDisabled");
         yield return new WaitForSeconds(clip.length);
         _pauseButton.enabled = true;
         _panel.SetActive(false);
         //PauseButtonOn.GetComponent<UIController>().enabled = true; 
      }
   }
}
