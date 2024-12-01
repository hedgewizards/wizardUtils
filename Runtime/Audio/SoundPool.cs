using System.Collections.Generic;
using UnityEngine;

namespace WizardUtils.Audio
{
    public class SoundPool
    {
        private PooledAudioTypeDescriptor AudioType;
        private Transform SourceParent;
        private int MaxCount => AudioType.PoolSize;

        private Queue<PooledAudioSource> InactivePool;
        private List<PooledAudioSource> ActiveList;

        public SoundPool(Transform parent, PooledAudioTypeDescriptor audioType)
        {
            SourceParent = parent;
            AudioType = audioType;
            InactivePool = new Queue<PooledAudioSource>();
            ActiveList = new List<PooledAudioSource>();
        }

        public void PlaySound(AdvancedSoundEffect effect, Transform soundParent = null)
        {
            var source = Pop();
            source.PlayAdvancedSound(effect, soundParent);
        }

        public void PlaySound(AudioClip clip, Transform soundParent = null)
        {
            var source = Pop();
            source.PlaySound(clip, 1, soundParent);
        }

        private PooledAudioSource Pop()
        {
            PooledAudioSource source;
            if (InactivePool.Count != 0)
            {
                source = InactivePool.Dequeue();
            }
            else if (ActiveList.Count >= MaxCount)
            {
                Debug.LogWarning($"overtaxed SoundPool of type {AudioType}. sound artifacts may occur");
                source = ActiveList[0];
                ActiveList.RemoveAt(0);
            }
            else
            {
                GameObject newSourceObject = Object.Instantiate(AudioType.Prefab, SourceParent);
                newSourceObject.name = $"Pooled Audio {InactivePool.Count + ActiveList.Count} {AudioType.name}";
                source = newSourceObject.GetComponent<PooledAudioSource>();
                if (source == null) throw new MissingComponentException($"Missing PooledAudioSource for {AudioType} prefab {AudioType.Prefab}");
                source.OnFree += Source_OnFree;
            }

            ActiveList.Add(source);
            return source;
        }

        private void Source_OnFree(object sender, System.EventArgs e)
        {
            var source = sender as PooledAudioSource;
            ActiveList.Remove(source);
            InactivePool.Enqueue(source);
        }
    }
}
