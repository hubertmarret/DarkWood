using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAudio : MonoBehaviour {

    [SerializeField]
    public AudioSource wolfFarAudio;
    [SerializeField]
    public AudioSource wolfCloseAudio;
    [SerializeField]
    public AudioClip[] randomWolfFarSounds;
    [SerializeField]
    public AudioClip[] randomWolfCloseSounds;
    [SerializeField]
    private AudioClip onSightPlayer;
    [SerializeField]
    private AudioClip onChasePlayer;
    [SerializeField]
    private float wolfFarSoundProbability = 0.5f;
    [SerializeField]
    private float wolfCloseSoundProbability = 0.8f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float distancePlayerThreshold = 65f;
    [SerializeField]
    private float distanceHowl = 130.0f;

    private float lightBaseShrinkFactor;

	// Use this for initialization
	void Start () {
		if (wolfFarAudio == null || wolfCloseAudio == null)
        {
            Debug.Log("Please reference the wolf's audio sources");
        }
        lightBaseShrinkFactor = player.GetComponent<LanternLightManager>().lightShrinkFactor;
	}
	
	// Update is called once per frame
	void Update () {
        float distancePlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distancePlayer < distancePlayerThreshold)
        {
            Debug.Log("Got close");
            PlayRandomSounds(wolfCloseAudio, randomWolfCloseSounds, wolfCloseSoundProbability, 0.95f, 1.05f);
            player.GetComponent<LanternLightManager>().lightShrinkFactor = lightBaseShrinkFactor * distancePlayerThreshold / distancePlayer;
        } else if (distancePlayer < distanceHowl)
        {
            Debug.Log("Got far");
            PlayRandomSounds(wolfFarAudio, randomWolfFarSounds, wolfFarSoundProbability);
        }
    }

    public void PlayRandomSounds(AudioSource _audioSource, AudioClip[] _audioClips)
    {
        if (_audioSource.isPlaying == false)
        {
            _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)]);
        }
    }

    public void PlayRandomSounds(AudioSource _audioSource, AudioClip[] _audioClips, float _probability, float lowPitchRange = 1.0f, float highPitchRange = 1.0f)
    {
        if (_audioSource.isPlaying == false)
        {
            if (Random.Range(0f, 1f) < _probability * Time.deltaTime)
            {
                _audioSource.pitch = Random.Range(lowPitchRange, highPitchRange);
                _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)], Random.Range(0.3f, 0.8f));
            }
        }
    }
}
