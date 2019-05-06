﻿using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace SuperMaxim.Core.Objects
{
    public class MonoBehaviourSingleton<TInterface, TImplementation> : MonoBehaviour
        where TImplementation : MonoBehaviour, TInterface
    {
        private static TInterface _default;

        public static TInterface Default
        {
            get
            {
                var res = InvalidateInstance();
                return res;
            }
        }

        private void Awake()
        {
            InvalidateInstance();
        }

        private static TInterface InvalidateInstance()
        {
            if(!Equals(_default, default(TImplementation)))
            {
                return _default;
            }

            var typeInterface = typeof(TInterface);
            var objects = FindObjectsOfType<TImplementation>();
            if(!objects.IsNullOrEmpty())
            {
                foreach (var obj in objects)
                {
                    if(typeInterface.IsAssignableFrom(obj.GetType()))
                    {
                        _default = obj;
                    }
                }
            }

            var typeImplementation = typeof(TImplementation);
            if (Equals(_default, default(TImplementation)))
            {
                var go = new GameObject(string.Format("[{0}]", typeImplementation.Name));
                _default = go.AddComponent<TImplementation>();
            }
            return _default;
        }
    }
}