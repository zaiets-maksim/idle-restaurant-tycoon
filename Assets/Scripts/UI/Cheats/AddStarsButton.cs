using Infrastructure;
using Services.CurrencyService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cheats
{
    public class AddStarsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int _amount;
        
        private readonly ICurrencyService _currencyService;

        public AddStarsButton()
        {
            _currencyService = ProjectContext.Instance?.CurrencyService;
        }
        
        private void Start()
        {
            _button.onClick.AddListener(MoreStars);
        }

        private void MoreStars() => _currencyService.AddStars(_amount);
    }
}
