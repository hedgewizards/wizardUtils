using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Tools
{
    public class DestroyInPlayMode : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}
