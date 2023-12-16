﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.GlobalSounds
{
    public class GlobalSoundService
    {
        private readonly GameObject Parent;
        public GlobalSoundManifest manifest;

        Dictionary<GlobalSoundDescriptor, AudioSource> soundsDict = new Dictionary<GlobalSoundDescriptor, AudioSource>();

        public GlobalSoundService(GameObject parent)
        {
            Parent = parent;
        }

        public void Play(GlobalSoundDescriptor sound)
        {
            AudioSource obj;
            if (!soundsDict.TryGetValue(sound, out obj))
            {
                var newSoundObj = UnityEngine.Object.Instantiate(sound.Prefab, Parent.transform);
                obj = newSoundObj.GetComponent<AudioSource>();
                soundsDict.Add(sound, obj);
            }
            obj.Play();
        }
    }
}
