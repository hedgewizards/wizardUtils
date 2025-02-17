using System.Collections.Generic;
using UnityEngine;
using WizardUtils.Audio.GlobalSounds;

namespace WizardUtils.Audio
{
    public class SoundService
    {
        private readonly GameObject Parent;
        private readonly GlobalSoundManifest GlobalSoundManifest;
        private readonly PooledAudioTypeManifest PooledAudioTypeManifest;
        private AudioType DefaultAudioType;

        Dictionary<GlobalSoundDescriptor, AudioSource> globalSoundsDictionary = new Dictionary<GlobalSoundDescriptor, AudioSource>();
        Dictionary<PooledAudioTypeDescriptor, SoundPool> soundsDictionary = new Dictionary<PooledAudioTypeDescriptor, SoundPool>();

        public SoundService(GameObject parent,
            GlobalSoundManifest globalSoundManifest,
            PooledAudioTypeManifest pooledAudioTypeManifest)
        {
            Parent = parent;
            GlobalSoundManifest = globalSoundManifest;
            PooledAudioTypeManifest = pooledAudioTypeManifest;
        }

        /// <summary>
        /// Steal an audio source from the pool to use yourself.<br/>
        /// you can either return it later, or destroy it when you are done (possibly by changing its transform parent!)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AdvancedAudioSource BorrowAudioSource(PooledAudioTypeDescriptor type)
        {
            SoundPool pool = GetSoundPool(type);
            return pool.BorrowAudioSource();
        }

        public void ReturnBorrowedAudioSource(PooledAudioTypeDescriptor type, AdvancedAudioSource source)
        {
            SoundPool pool = GetSoundPool(type);
            pool.ReturnBorrowedAudioSource(source);
        }

        public void PlaySound(AdvancedSoundEffect sound, Vector3 position)
        {
            SoundPool pool = GetSoundPool(sound.AudioType);
            pool.PlaySound(sound, position);
        }

        public void PlaySound(AdvancedSoundEffect sound, Transform soundParent)
        {
            SoundPool pool = GetSoundPool(sound.AudioType);
            pool.PlaySound(sound, soundParent);
        }

        public void PlaySound(AudioClip clip, Vector3 position)
        {
            SoundPool pool = GetSoundPool(PooledAudioTypeManifest.DefaultAudioType);
            pool.PlaySound(clip, position);
        }

        public void PlaySound(AudioClip clip, Transform soundParent = null)
        {
            SoundPool pool = GetSoundPool(PooledAudioTypeManifest.DefaultAudioType);
            pool.PlaySound(clip, soundParent);
        }

        public void PlayGlobalSound(GlobalSoundDescriptor sound)
        {
            AudioSource obj;
            if (!globalSoundsDictionary.TryGetValue(sound, out obj))
            {
                var newSoundObj = UnityEngine.Object.Instantiate(sound.Prefab, Parent.transform);
                obj = newSoundObj.GetComponent<AudioSource>();
                globalSoundsDictionary.Add(sound, obj);
            }
            obj.Play();
        }
        private SoundPool GetSoundPool(PooledAudioTypeDescriptor audioType)
        {
            SoundPool pool;
            if (!soundsDictionary.TryGetValue(audioType, out pool))
            {
                pool = new SoundPool(Parent.transform, audioType);
                soundsDictionary.Add(audioType, pool);
            }

            return pool;
        }

    }
}
