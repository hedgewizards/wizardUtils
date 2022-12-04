using System;
using UnityEngine;

namespace WizardUtils.Materials
{
    public class SmartMaterialFloat : MonoBehaviour
    {
        public Renderer Target;

        public string Parameter;
        int ParameterId;

        public void Awake()
        {
            ParameterId = Shader.PropertyToID(Parameter);
        }

        public void SetParameter(float value)
        {
            Target.material.SetFloat(ParameterId, value);
        }
    }
}
