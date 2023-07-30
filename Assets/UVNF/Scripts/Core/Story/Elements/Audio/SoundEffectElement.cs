﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoroutineManager;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Audio
{
    public class SoundEffectElement : StoryElement
    {
        public override string ElementName => "Sound Effect";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Audio();

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        public AudioClip AudioClip;
        public float Volume = 0.5f;

        public bool WaitForAudio = false;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            AudioClip = EditorGUILayout.ObjectField("Audio Clip", AudioClip, typeof(AudioClip), false) as AudioClip;
            Volume = EditorGUILayout.Slider("Volume", Volume, 0f, 1f);
            WaitForAudio = GUILayout.Toggle(WaitForAudio, "Wait For Audio");
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (WaitForAudio)
            {
                Task task = new Task(managerCallback.AudioManager.PlaySoundCoroutine(AudioClip, Volume), true);
                while (task.Running) yield return null;
            }
            else
            {
                managerCallback.AudioManager.PlaySound(AudioClip, Volume);
            }
        }
    }
}