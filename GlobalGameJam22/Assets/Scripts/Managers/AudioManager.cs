using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource MusicSource1;
	private AudioSource MusicSource2;

	public static AudioManager Instance = null;

	private GameObject player;

	[HideInInspector] public List<AudioClip> audioClips = new List<AudioClip>();
	[HideInInspector] public List<GameObject> audioSourceObjects;
	public GameObject audioSourceObjectToPool;
	public int amountToPool;

	public clips currentMusicClip;
	[Range(0f, 0.5f)]
	public float musicVolume;
	public float timeToFade;
	private float timeElapsed = 0f;

	public enum clips
	{
		PlayerHitOhno,
		PlayerHitAhh,
		PlayerHitOof,
		BackGroundMusic,
		FootStep1,
		FootStep2,
		FootStep3,
		FootStep4,
		FootStep5
	};

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		audioClips = Resources.LoadAll("Sounds/", typeof(AudioClip)).Cast<AudioClip>().ToList();
	}

	private void Start()
	{
		MusicSource1 = gameObject.AddComponent<AudioSource>();
		MusicSource1.loop = true;

		SetMusic(currentMusicClip);

		audioSourceObjects = new List<GameObject>();

		GameObject tmp;

		for (int i = 0; i < amountToPool; i++)
		{
			tmp = Instantiate(audioSourceObjectToPool, GameObject.Find("AudioManager").transform);
			tmp.SetActive(false);
			audioSourceObjects.Add(tmp);
		}
	}

	/// <summary>
	/// Method that gets a not used audio source object from the audio source ObjectPool
	/// </summary>
	/// <returns></returns>
	public GameObject GetPooledAudioSourceObject()
	{
		for (int i = 0; i < amountToPool; i++)
		{
			if (!audioSourceObjects[i].activeInHierarchy)
			{
				return audioSourceObjects[i];
			}
		}
		return null;
	}

	/// <summary>
	/// Handles picking the player hit sound effect.
	/// </summary>
	public void HandlePlayerHitSound()
	{ 
		// cahnge to player object
		player = GameObject.Find("Player");

		GameObject audioSource = GetPooledAudioSourceObject();
		audioSource.transform.localPosition = player.transform.position;
		audioSource.SetActive(true);

		Play(GetRandomPlayerHitClip(), audioSource.GetComponent<AudioSource>());
	}

	private clips GetRandomPlayerHitClip()
	{
		int index = Random.Range(0, 3);

        if (index == 0)
        {
            foreach (Transform audioSource in transform)
            {
                if (audioSource.GetComponent<AudioSource>().isPlaying && 
					audioSource.GetComponent<AudioSource>().clip.name == "PlayerHitOhno")
                {
					index = Random.Range(1, 3);
				}
            }
        }

		return (clips)index;
	}

	public void HandlePlayerFootSteps()
    {
		// cahnge to player object
		player = GameObject.Find("Player");

		GameObject audioSource = GetPooledAudioSourceObject();
		audioSource.transform.localPosition = player.transform.position;
		audioSource.SetActive(true);

		Play(GetRandomFootStepClip(), audioSource.GetComponent<AudioSource>());
	}

	private clips GetRandomFootStepClip()
	{
		int index = Random.Range(4, 9);

        foreach (Transform audioSource in transform)
        {
            while (audioSource.GetComponent<AudioSource>().isPlaying &&
                   audioSource.GetComponent<AudioSource>().clip.name == ((clips)index).ToString())
            {
                index = Random.Range(4, 9);
            }
        }

        return (clips)index;
	}

	/// <summary>
	/// Play a single clip through the sound effects source.
	/// </summary>	
	public void Play(clips clipName, AudioSource source)
	{
		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		source.clip = toBePlayedClip;
		source.Play();

		StartCoroutine(WaitForEndOfSound(source.gameObject, toBePlayedClip.length));
	}

	/// <summary>
	/// Enumerator that disables the audio source from the audioSourcesPool after the duration of the playing clip
	/// </summary>
	public IEnumerator WaitForEndOfSound(GameObject audioSourceObj, float clipDuration)
	{
		yield return new WaitForSeconds(clipDuration);

		if (audioSourceObj != null)
		{
			audioSourceObj.SetActive(false);
		}
	}

	/// <summary>
	/// Play a single clip through the looping music source.
	/// </summary>
	/// <param name="clip"></param>
	public void SetMusic(clips clipName)
	{
		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		StopAllCoroutines();
		StartCoroutine(FadeMusic(clipName, toBePlayedClip));
	}

	/// <summary>
	/// The coroutine that handles the fading in and out of the building and driving soundtracks.
	/// </summary>
	/// <param name="clipName"></param>
	/// <param name="toBePlayedClip"></param>
	/// <returns></returns>
	private IEnumerator FadeMusic(clips clipName, AudioClip toBePlayedClip)
	{
		timeElapsed = 0f;

		if (clipName == clips.BackGroundMusic)
		{
			MusicSource1.clip = toBePlayedClip;
			MusicSource1.Play();

			while (timeElapsed < timeToFade)
			{
				MusicSource1.volume = Mathf.Lerp(0, musicVolume, timeElapsed / timeToFade);
				timeElapsed += Time.deltaTime;
				yield return null;
			}
		}
	}

	/// <summary>
	/// Stops the sounds played by the audio source.
	/// </summary>
	/// <param name="source"></param>
	public void Stop(AudioSource source)
	{
		source.Stop();
	}
}
