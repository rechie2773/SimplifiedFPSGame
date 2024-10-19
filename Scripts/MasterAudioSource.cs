using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MasterAudioSource : MonoBehaviour
{
    public Slider slider;
    private List<AudioSource> audioSources;
    void Awake()
    {
        audioSources = new List<AudioSource>();
        // Find all AudioSources in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            audioSources.Add(source);
        }
        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnValueChanged);
            Debug.Log("Slider assigned and listener added.");
        }
        else
        {
            Debug.LogError("Slider is not assigned in inspector");
        }
    }
    void Start()
    {
        if (slider != null)
        {
            slider.value = 0.5f;
            // Apply the initial slider value to all audio sources
            OnValueChanged(slider.value);
            Debug.Log("Slider initialized to middle value.");
        }
    }
    public void OnValueChanged(float value)
    {
        Debug.Log("Slider value changed: " + value);
        foreach (AudioSource source in audioSources)
        {
            source.volume = value;
            Debug.Log("AudioSource volume set to: " + source.volume);
        }
    }
}
