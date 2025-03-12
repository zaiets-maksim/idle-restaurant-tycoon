using Infrastructure;
using Services.CurrencyService;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class StarsView : MonoBehaviour
    {
        [SerializeField] private Text _text;
    
        private readonly ICurrencyService _currencyService;
    
        public StarsView()
        {
            _currencyService = ProjectContext.Instance.CurrencyService;
        }

        private void OnEnable()
        {
            _currencyService.OnStarsChanged += UpdateText;
        }

        private void OnDisable()
        {
            _currencyService.OnStarsChanged -= UpdateText;
        }

        private void UpdateText(int amount) => _text.text = amount.ToString();
    }
}
