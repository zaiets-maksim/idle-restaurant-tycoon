using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.DI
{
    public class BindingBuilder<TContract>
    {
        private readonly DiContainer _container;
        private readonly Transform _monoServicesRoot;
        private TContract _instance;
        private Func<TContract> _factory;

        public BindingBuilder(DiContainer container, Transform monoServicesRoot)
        {
            _container = container;
            _monoServicesRoot = monoServicesRoot;
        }

        #region Regular Classes

        /// <summary>
        /// Bind to a concrete implementation type (creates new instance)
        /// </summary>
        public BindingBuilder<TContract> To<TImpl>() where TImpl : TContract, new()
        {
            _factory = () => new TImpl();
            return this;
        }

        /// <summary>
        /// Bind to an existing instance
        /// </summary>
        public BindingBuilder<TContract> FromInstance(TContract instance)
        {
            _instance = instance;
            return this;
        }

        /// <summary>
        /// Bind using a factory method
        /// </summary>
        public BindingBuilder<TContract> FromMethod(Func<TContract> factory)
        {
            _factory = factory;
            return this;
        }

        #endregion

        #region MonoBehaviour

        /// <summary>
        /// Create a new GameObject with the component attached (like Zenject)
        /// </summary>
        public BindingBuilder<TContract> FromNewComponentOnNewGameObject<TImpl>(string name = null) where TImpl : MonoBehaviour, TContract
        {
            _factory = () =>
            {
                var typeName = name ?? typeof(TImpl).Name;
                var go = new GameObject($"[Service] {typeName}");
                
                if (_monoServicesRoot != null)
                    go.transform.SetParent(_monoServicesRoot);
                
                Object.DontDestroyOnLoad(go);
                
                return (TContract)go.AddComponent<TImpl>();
            };
            return this;
        }

        /// <summary>
        /// Find existing component in scene hierarchy (like Zenject)
        /// </summary>
        public BindingBuilder<TContract> FromComponentInHierarchy<TImpl>() where TImpl : MonoBehaviour, TContract
        {
            _factory = () =>
            {
                var component = Object.FindObjectOfType<TImpl>();
                
                if (component == null)
                    throw new InvalidOperationException($"[DiContainer] Could not find {typeof(TImpl).Name} in hierarchy.");
                
                return component;
            };
            return this;
        }

        /// <summary>
        /// Instantiate a prefab and get component from it (like Zenject)
        /// </summary>
        public BindingBuilder<TContract> FromComponentInNewPrefab(MonoBehaviour monoBehaviour)
        {
            _factory = () =>
            {
                var go = Object.Instantiate(monoBehaviour);
                
                if (_monoServicesRoot != null)
                    go.transform.SetParent(_monoServicesRoot);
                
                Object.DontDestroyOnLoad(go);
                
                var component = go.GetComponent<TContract>();
                
                if (component == null)
                    throw new InvalidOperationException($"[DiContainer] Prefab {monoBehaviour.name} does not contain {typeof(TContract).Name}.");
                
                return component;
            };
            return this;
        }

        /// <summary>
        /// Add component to existing GameObject
        /// </summary>
        public BindingBuilder<TContract> FromNewComponentOn<TImpl>(GameObject gameObject) where TImpl : MonoBehaviour, TContract
        {
            _factory = () => (TContract)gameObject.AddComponent<TImpl>();
            return this;
        }

        #endregion

        /// <summary>
        /// Register as singleton (creates instance immediately)
        /// </summary>
        public void AsSingle()
        {
            TContract instance;
            
            if (_instance != null)
                instance = _instance;
            else if (_factory != null)
                instance = _factory();
            else
                throw new InvalidOperationException(
                    $"[DiContainer] No binding source specified for {typeof(TContract).Name}. " +
                    "Use To<T>(), FromInstance(), FromMethod(), or one of the MonoBehaviour methods.");
            
            _container.Register(instance);
        }

        /// <summary>
        /// Register as lazy singleton (creates instance on first Resolve)
        /// </summary>
        public void AsLazy()
        {
            if (_instance != null)
            {
                _container.Register(_instance);
                return;
            }
            
            if (_factory == null)
            {
                throw new InvalidOperationException(
                    $"[DiContainer] No binding source specified for {typeof(TContract).Name}. " +
                    "Use To<T>(), FromMethod(), or one of the MonoBehaviour methods.");
            }
            
            _container.RegisterLazy(_factory);
        }
    }
}
