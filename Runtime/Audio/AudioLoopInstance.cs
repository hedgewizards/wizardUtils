using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Audio
{
    public struct AudioLoopInstance
    {
        internal AdvancedSoundEffect Sound;
        internal AdvancedAudioSource Source;

        public AudioLoopInstance(AdvancedSoundEffect sound, AdvancedAudioSource source)
        {
            Sound = sound;
            Source = source;
        }
    }
}
