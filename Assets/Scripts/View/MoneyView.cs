using Infrastructure;
using Services.CurrencyService;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private Text _text;
    
        private readonly ICurrencyService _currencyService;
    
        public MoneyView()
        {
            _currencyService = ProjectContext.Instance.CurrencyService;
        }

        private void OnEnable()
        {
            _currencyService.OnMoneyChanged += UpdateText;
        }

        private void OnDisable()
        {
            _currencyService.OnMoneyChanged -= UpdateText;
        }

        private void UpdateText(int amount) => _text.text = amount.ToString();
    }
}
