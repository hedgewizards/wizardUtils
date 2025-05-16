using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Audio
{
    public struct AudioLoopInstance
    {
        public AdvancedSoundEffect Sound { get; private set; }
        public AdvancedAudioSource Source { get; private set; }

        public AudioLoopInstance(AdvancedSoundEffect sound, AdvancedAudioSource source)
        {
            Sound = sound;
            Source = source;
        }
    }
}
