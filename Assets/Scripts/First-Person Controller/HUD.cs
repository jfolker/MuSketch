using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;

	public float[] bpm;

	//A collection of songs for the user to choose from to mix and modulate with incoming sound from the AudioListener
	public AudioSource[] songs;

	public AudioLowPassFilter loPass;
	public AudioHighPassFilter hiPass;
	public AudioDistortionFilter distortionFilter;
	//public AudioEchoFilter echoFilter;
	public AudioChorusFilter chorusFilter;

	private AudioSource selectedSong;
	private string[] trackList;
	private string[] effectList;
	private int i, j;

	enum AudioEffect {HiPass=0, LoPass=1, distortionFilter = 2, chorusFilter=3};
	private bool fxlock;

	public float[] dbTimeStamps; //Downbeat Timestamps 
	public float updateTime = 20f; //How frequently we check for condition match. 
	public GameObject[] observers;
	
	private int dbCounter;
	private float currentSample;
		
	// Use this for initialization
	void Start () {
		fxlock = true;
		i = 0;
		j = 0;
		trackList = new string[] {songs[0].clip.name, songs[1].clip.name, songs[2].clip.name, songs[3].clip.name};
		effectList = new string[] {"Hi-Pass", "Lo-Pass", "Distortion Filter", "Chorus Filter"};
	}

	void OnGUI(){
		GUI.TextField (new Rect (Screen.width*(1.0f-0.45f), Screen.height*(0.05f), Screen.width*(.4f), Screen.width*(.3f)), 
		               "Track " + i  + ": "+ trackList[i] + ", " + bpm[i] + " BPM \nEffect: " + effectList[j]);
	}

	// Update is called once per frame
	void Update () {
		//Navigating the GUI
		if (Input.GetKeyUp(KeyCode.Keypad4)) {
			i--;
		} else if (Input.GetKeyUp(KeyCode.Keypad6)) {
			i++;
		} else if (Input.GetKeyUp(KeyCode.Keypad8)) {
			j--;
		} else if (Input.GetKeyUp(KeyCode.Keypad2)) {
			j++;
		}
		i = Mathf.Clamp (i, 0, trackList.Length-1);
		j = Mathf.Clamp (j, 0, effectList.Length-1);

		if(Input.GetKeyUp(KeyCode.Alpha0)){
			songs[0].audio.Stop();
			songs[1].audio.Stop();
			songs[2].audio.Stop();
			songs[3].audio.Stop();
		} else if(Input.GetKeyUp(KeyCode.Alpha1)){
			if(songs[0].isPlaying){
				songs[0].audio.Stop();
			}
			else{
				songs[0].audio.Play();
			}
			//StartCoroutine(OnDownBeat(songs[0]));
		} else if(Input.GetKeyUp(KeyCode.Alpha2)){
			if(songs[1].isPlaying){
				songs[1].audio.Stop();
			}
			else{
				songs[1].audio.Play();
			}
			//StartCoroutine(OnDownBeat(songs[1]));
		} else if(Input.GetKeyUp(KeyCode.Alpha3)){
			if(songs[2].isPlaying){
				songs[2].audio.Stop();
			}
			else{
				songs[2].audio.Play();
			}
			//StartCoroutine(OnDownBeat(songs[2]));
		} else if(Input.GetKeyUp(KeyCode.Alpha4)){
			if(songs[3].isPlaying){
				songs[3].audio.Stop();
			}
			else{
				songs[3].audio.Play();
			}
			//StartCoroutine(OnDownBeat(songs[3]));
		}
		if (Input.GetKeyUp (KeyCode.Q)) {
			songs[i].loop = !songs[i].loop;
		}
		if (!fxlock) {
			if(j==(int)AudioEffect.distortionFilter)
				distortionFilter.distortionLevel = Input.mousePosition.y/Screen.height;
			else if(j==(int)AudioEffect.LoPass)
				loPass.cutoffFrequency = Mathf.Clamp(Mathf.Pow(16000.0f, Input.mousePosition.y/Screen.height)+60, 60, 16000);
			else if(j==(int)AudioEffect.HiPass)
				hiPass.cutoffFrequency = Mathf.Clamp(Mathf.Pow(16000.0f, Input.mousePosition.y/Screen.height)+60, 60, 16000);
			else if(j==(int)AudioEffect.chorusFilter)
				chorusFilter.depth = Mathf.Clamp(100*Input.mousePosition.y/Screen.height, 0, 100);
		}

		if (Input.GetKeyUp (KeyCode.RightShift)||Input.GetKeyUp (KeyCode.LeftShift)||Input.GetKeyUp (KeyCode.E)) {
			fxlock = !fxlock;
		}
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
