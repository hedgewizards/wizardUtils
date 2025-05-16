using System.Collections.Generic;
using UnityEngine;

namespace WizardUtils.Audio
{
    public class SoundPool
    {
        private PooledAudioTypeDescriptor AudioType;
        private Transform SourceParent;
        private int MaxCount => AudioType.PoolSize;

        private Queue<PooledAdvancedAudioSource> InactivePool;
        private List<PooledAdvancedAudioSource> ActiveList;

        public SoundPool(Transform parent, PooledAudioTypeDescriptor audioType)
        {
            SourceParent = parent;
            AudioType = audioType;
            InactivePool = new Queue<PooledAdvancedAudioSource>();
            ActiveList = new List<PooledAdvancedAudioSource>();
        }

        public AdvancedAudioSource BorrowAudioSource()
        {
            var source = Pop();
            ActiveList.Remove(source);
            source.OnFree -= Source_OnFree;

            return source;
        }

        public void ReturnBorrowedAudioSource(AdvancedAudioSource source, bool isBusy)
        {
            PooledAdvancedAudioSource pooledSource = (PooledAdvancedAudioSource)source;
            if (ActiveList.Contains(pooledSource)
                || InactivePool.Contains(pooledSource))
            {
                return;
            }

            pooledSource.OnFree += Source_OnFree;
            pooledSource.transform.SetParent(SourceParent);
            
            if (isBusy)
            {
                ActiveList.Add(pooledSource);
            }
            else
            {
                InactivePool.Enqueue(pooledSource);
            }
        }

        public void PlaySound(AdvancedSoundEffect effect, Transform soundParent = null)
        {
            var source = Pop();
            source.PlayAdvancedSound(effect, soundParent);
        }

        public void PlaySound(AdvancedSoundEffect effect, Vector3 position)
        {
            var source = Pop();
            source.PlayAdvancedSound(effect, position);
        }

        public void PlaySound(AudioClip clip, Transform soundParent = null)
        {
            var source = Pop();
            source.PlaySound(clip, 1, soundParent);
        }

        public void PlaySound(AudioClip clip, Vector3 position)
        {
            var source = Pop();
            source.PlaySound(clip, 1);
        }

        private PooledAdvancedAudioSource Pop()
        {
            PooledAdvancedAudioSource source;
            if (InactivePool.Count != 0)
            {
                source = InactivePool.Dequeue();
            }
            else if (MaxCount > 0 && ActiveList.Count >= MaxCount)
            {
                Debug.LogWarning($"overtaxed SoundPool of type {AudioType}. sound artifacts may occur");
                source = ActiveList[0];
                ActiveList.RemoveAt(0);
            }
            else
            {
                GameObject newSourceObject = Object.Instantiate(AudioType.Prefab, SourceParent);
                newSourceObject.name = $"Pooled Audio {InactivePool.Count + ActiveList.Count} {AudioType.name}";
                source = newSourceObject.GetComponent<PooledAdvancedAudioSource>();
                if (source == null) throw new MissingComponentException($"Missing PooledAudioSource for {AudioType} prefab {AudioType.Prefab}");
                source.OnFree += Source_OnFree;
            }

            ActiveList.Add(source);
            return source;
        }

        private void Source_OnFree(object sender, System.EventArgs e)
        {
            var source = sender as PooledAdvancedAudioSource;
            ActiveList.Remove(source);
            InactivePool.Enqueue(source);
        }
    }
}
