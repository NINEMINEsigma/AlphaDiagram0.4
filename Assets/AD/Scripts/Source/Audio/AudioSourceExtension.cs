using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AD.Utility
{
    [Serializable]
    public class AudioSourcePackage
    {
        public AudioMixer audioMixer = AudioSourceExtension.ADMixer;
        public string TargetGroupName = "Master";
        public string TargetPitch_Attribute_Name = "MasterPitch";
        public string TargetPitchshifterPitch_Attribute_Name = "PitchShifterPitch";

        public float TargetPitchValue = 1.0f;
        public float Speed { get { return TargetPitchValue; } set { TargetPitchValue = value; } }
    }

    public static class AudioSourceExtension
    {
        public static AudioMixer ADMixer = null;


        public static AudioSource SetSpeed(this AudioSource self, AudioSourcePackage package)
        {
            if (AudioSourceExtension.ADMixer == null) AudioSourceExtension.ADMixer = package.audioMixer;

            if (package.TargetPitchValue > 0)
            {
                self.pitch = 1;
                package.audioMixer.SetFloat(package.TargetPitch_Attribute_Name, package.TargetPitchValue);
                float TargetPitchshifterPitchValue = 1.0f / package.TargetPitchValue;
                package.audioMixer.SetFloat(package.TargetPitchshifterPitch_Attribute_Name, TargetPitchshifterPitchValue);
                self.outputAudioMixerGroup = package.audioMixer.FindMatchingGroups(package.TargetGroupName)[0];
            }
            else
            {
                self.pitch = -1;
                package.audioMixer.SetFloat(package.TargetPitch_Attribute_Name, -package.TargetPitchValue);
                float TargetPitchshifterPitchValue = -1.0f / package.TargetPitchValue;
                package.audioMixer.SetFloat(package.TargetPitchshifterPitch_Attribute_Name, TargetPitchshifterPitchValue);
                self.outputAudioMixerGroup = package.audioMixer.FindMatchingGroups(package.TargetGroupName)[0];
            }
                return self;
        }

        public static void PlayClipAtPoint(this AudioClip self, Vector3 point, float volume)
        {
            AudioSource.PlayClipAtPoint(self, point, volume);
        }
    }
}