using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

    [SerializeField]
    private AudioSource playerAudio;
    [SerializeField]
    private AudioClip walkingSound;
    [SerializeField]
    private AudioSource[] audioSources;
    [SerializeField]
    private AudioClip[] randomNightSounds;
    [SerializeField]
    private float nightSoundProbability = 0.08f;
    [SerializeField]
    private AudioSource[] loopingAudioSources;
    private float[] loopingAudioSourcesVolume;

    private Player player;

	// Use this for initialization
	void Start () {
        if (playerAudio == null)
        {
            Debug.Log("Please add the PlayerAudio object (with an audio source component) to the player and reference it here.");
        }
        player = GetComponent<Player>();
        loopingAudioSourcesVolume = new float[loopingAudioSources.Length];
        for (int i = 0; i < loopingAudioSources.Length; i++)
        {
            loopingAudioSourcesVolume[i] = loopingAudioSources[i].volume;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (player.walking == true && playerAudio.isPlaying == false)
        {
            playerAudio.clip = walkingSound;
            playerAudio.volume = 0.2f;
            playerAudio.Play();
        } 
        if (player.walking == true && playerAudio.isPlaying == true)
        {
            playerAudio.volume = Mathf.Min(playerAudio.volume + Time.deltaTime, 0.3f);
        }
        if (player.walking == false && playerAudio.isPlaying == true)
        {
            playerAudio.volume = Mathf.Max(playerAudio.volume - Time.deltaTime * 1.5f, 0.0f);
            if (playerAudio.volume <= 0.0f)
            {
                playerAudio.Pause();
            }
        }

        foreach(AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying == false)
            {
                if (Random.Range(0f, 1f) < nightSoundProbability * Time.deltaTime)
                {
                    audioSource.PlayOneShot(randomNightSounds[Random.Range(0, randomNightSounds.Length)], Random.Range(0.3f, 0.8f));
                }
            }
        }

    }
    

    public void fadeAmbientSoundWithDistance(float _curDistance, float _minVolumeDistance, float _maxVolumeDistance)
    {
        for (int i = 0; i < loopingAudioSources.Length; i++)
        {
            fadeWithDistance(loopingAudioSources[i], _curDistance, _minVolumeDistance, _maxVolumeDistance, 0.0f, loopingAudioSourcesVolume[i]);
        }
        foreach (AudioSource audioSource in audioSources)
        {
            fadeWithDistance(audioSource, _curDistance, _minVolumeDistance, _maxVolumeDistance, 0.0f, 0.5f);
        }
    }

    public void fadeWithDistance(AudioSource _audioSource, float _curDistance, float _minVolumeDistance, float _maxVolumeDistance,
        float _minVolume, float _maxVolume)
    {
        _audioSource.volume = Mathf.Max(_minVolume + (_maxVolume - _minVolume) * (_curDistance - _minVolumeDistance) / (_maxVolumeDistance - _minVolumeDistance), 0.0f);
    }

    public void fadeInAmbiantSound()
    {
        for (int i = 0; i < loopingAudioSources.Length; i++)
        {
            loopingAudioSources[i].volume = Mathf.Min(loopingAudioSources[i].volume + Time.deltaTime, loopingAudioSourcesVolume[i]);
        }
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = Mathf.Min(audioSource.volume + Time.deltaTime, 1.0f);
        }
    }
}
