using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Shara script
public class AudioPlayer : MonoBehaviour
{
    private static readonly string VolumePref = "VolumePref";
    private float volumeFloat;
    public AudioSource[] volumeAudio;
    public AudioSource volumeAudios;
    void Awake()
    {

    }

    private void ContintueSettings()
    {
        volumeFloat = PlayerPrefs.GetFloat(VolumePref);

        volumeAudios.volume = volumeFloat;

        for (int i = 0; i < volumeAudio.Length; i++)
        {
            volumeAudio[i].volume = volumeFloat;
        }
    }
}
