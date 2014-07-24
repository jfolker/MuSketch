using UnityEngine;
using System.Collections;

// This needs to be attached to the audio source objects that needs to be synced to downbeats
[RequireComponent(typeof(AudioSource))]
public class SyncPlay : MonoBehaviour 
{
	public float checkDBtime = 10f;

	void Start()
	{

	}

	void Update()
	{
		//key status update
	}

	public void IsDownBeat()
	{
		// if key is pressed once(true - play, pressed again - false for 'loop' tracks only)
		// check tracks tempo (with tempo = 0, only play one shot on next downbeat)
		// 

		// play

	}

}