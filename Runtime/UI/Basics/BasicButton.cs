using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardUtils.UI.Basics
{
    public class BasicButton : MonoBehaviour, IListableButton
    {
        public TextMeshProUGUI ButtonText;

        public void AddOnClickListener(UnityAction action)
        {
            GetComponent<Button>().onClick.AddListener(action);
        }

        public void SetText(string text)
        {
            ButtonText.text = text;
        }
    }
}