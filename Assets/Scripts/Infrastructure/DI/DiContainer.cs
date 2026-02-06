using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.DI
{
    public class DiContainer
    {
        private readonly Dictionary<Type, object> _services = new();
        private readonly Dictionary<Type, Func<object>> _lazyFactories = new();
        private readonly Transform _monoServicesRoot;

        public DiContainer(Transform monoServicesRoot = null)
        {
            _monoServicesRoot = monoServicesRoot;
        }

        public BindingBuilder<TContract> Bind<TContract>()
        {
            return new BindingBuilder<TContract>(this, _monoServicesRoot);
        }

        internal void Register<TContract>(TContract instance)
        {
            var type = typeof(TContract);
            
            if (_services.ContainsKey(type) || _lazyFactories.ContainsKey(type))
            {
                Debug.LogWarning($"[DiContainer] Service {type.Name} is already registered. Overwriting.");
            }
            
            _lazyFactories.Remove(type);
            _services[type] = instance;
        }

        internal void RegisterLazy<TContract>(Func<TContract> factory)
        {
            var type = typeof(TContract);
            
            if (_services.ContainsKey(type) || _lazyFactories.ContainsKey(type))
            {
                Debug.LogWarning($"[DiContainer] Service {type.Name} is already registered. Overwriting.");
            }
            
            _services.Remove(type);
            _lazyFactories[type] = () => factory();
        }

        public TContract Resolve<TContract>()
        {
            var type = typeof(TContract);
            
            // Сначала проверяем готовые инстансы
            if (_services.TryGetValue(type, out var service))
            {
                return (TContract)service;
            }
            
            // Затем проверяем lazy-фабрики
            if (_lazyFactories.TryGetValue(type, out var factory))
            {
                var instance = (TContract)factory();
                _services[type] = instance;
                _lazyFactories.Remove(type);
                return instance;
            }
            
            throw new InvalidOperationException($"[DiContainer] Service {type.Name} is not registered.");
        }

        public bool TryResolve<TContract>(out TContract service)
        {
            var type = typeof(TContract);
            
            if (_services.TryGetValue(type, out var obj))
            {
                service = (TContract)obj;
                return true;
            }
            
            service = default;
            return false;
        }

        public bool IsRegistered<TContract>()
        {
            var type = typeof(TContract);
            return _services.ContainsKey(type) || _lazyFactories.ContainsKey(type);
        }
    }
}
