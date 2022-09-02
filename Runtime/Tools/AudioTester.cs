using System.Collections;
using UnityEngine;

namespace WizardUtils
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioTester : MonoBehaviour
    {
        public void PlayNow()
        {
            GetComponent<AudioSource>().Play();
        }
    }
}