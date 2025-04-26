using Infrastructure;
using Services.CurrencyService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Cheats
{
    public class AddStarsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int _amount;
        
        private ICurrencyService _currencyService;

        [Inject]
        public void Constructor(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        
        private void Start()
        {
            _button.onClick.AddListener(MoreStars);
        }

        private void MoreStars() => _currencyService.AddStars(_amount);
    }
}
