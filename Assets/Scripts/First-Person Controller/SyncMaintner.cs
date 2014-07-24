using UnityEngine;
using System.Collections;

// Adapted from Christian Floisand's explaination (https://christianfloisand.wordpress.com/2014/01/23/beat-synchronization-in-unity/)
// Maintaining to downbeat timestamps analyzed (as in such an application with different types of audio with/without tempo,
// being triggered at different times by the user.)

public class SyncMainter : MonoBehaviour 
{
	public float[] dbTimeStamps; //Downbeat Timestamps 
	public float updateTime = 20f; //How frequently we check for condition match. 
	public GameObject[] observers;
	public AudioSource audioSource;

	private int dbCounter;
	private float currentSample;

	void Awake()
	{
		dbCounter = 0;
	}

	void Update()
	{
		StartCoroutine(OnDownBeat(audioSource));
	}

	IEnumerator OnDownBeat(AudioSource audioSource)
	{

		while (audioSource.isPlaying) 
		{
			// get current sample position
			currentSample = (float)AudioSettings.dspTime * audioSource.clip.frequency;

			// match to downbeat(s) sample position
			if (currentSample >= (dbTimeStamps[dbCounter] * audioSource.clip.frequency)) 
			{
				foreach (GameObject obj in observers) 
				{
					obj.GetComponent<SyncPlay>().IsDownBeat();
				}

				dbCounter++;

				// for loop and wrap around
				if (dbCounter >= dbTimeStamps.Length)
					dbCounter = 0;
			}

			//How frequently should it update to check the loop for downbeat true.
			yield return new WaitForSeconds(updateTime/1000f);
		}
	}

}
