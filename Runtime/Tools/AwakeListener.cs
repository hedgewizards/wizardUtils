using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.Tools
{
    public class AwakeListener : MonoBehaviour
    {
        public UnityEvent OnAwake;

        // Use this for initialization
        void Awake()
        {
            OnAwake?.Invoke();
        }
    }
}