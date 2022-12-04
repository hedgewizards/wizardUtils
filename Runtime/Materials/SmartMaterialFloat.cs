using System;
using UnityEngine;

namespace WizardUtils.Materials
{
    public class SmartMaterialFloat : MonoBehaviour
    {
        public Renderer Target;

        [HideInInspector]
        public string Parameter;
        [HideInInspector]
        public int ParameterId;

        public void UpdateParameterId()
        {
            ParameterId = Shader.PropertyToID(Parameter);
        }

        public void SetParameter(float value)
        {
            Target.material.SetFloat(ParameterId, value);
        }
    }
}
