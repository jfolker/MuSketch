using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;

	//A collection of songs for the user to choose from to mix and modulate with incoming sound from the AudioListener
	public AudioSource song1;
	public AudioSource song2;
	public AudioSource song3;
	public AudioSource song4;
	
	public struct AudioNode{
		public AudioSource audio;
		public AudioReverbFilter reverbFilter;
		public AudioLowPassFilter loPass;
		public AudioHighPassFilter hiPass;
		public AudioDistortionFilter distortionFilter;
	}

	private AudioNode[] audioNodes;

	private string[] trackList;
	private string[] effectList;
	private int i, j;

	enum AudioEffect {reverbFilter=0, HiPass=1, LoPass=2, distortionFilter = 3};
		
	// Use this for initialization
	void Start () {
		audioNodes = new AudioNode[4];
		for (int c=0; c<audioNodes.Length; c++) {
			audioNodes [c] = new AudioNode ();
		}

		audioNodes[0].audio = song1;
		audioNodes[1].audio = song2;
		audioNodes[2].audio = song3;
		audioNodes[3].audio = song4;

		for (int k=0; k<audioNodes.Length; k++) {
			audioNodes[k].reverbFilter = new AudioReverbFilter();
			audioNodes[k].distortionFilter = new AudioDistortionFilter();
			audioNodes[k].loPass = new AudioLowPassFilter();
			audioNodes[k].hiPass = new AudioHighPassFilter();

			/*
			audioNodes[k].reverbFilter.audio= audioNodes[k].audio;
			audioNodes[k].distortionFilter.audio = audioNodes[k].audio;
			audioNodes[k].loPass.audio = audioNodes[k].audio;
			audioNodes[k].hiPass.audio = audioNodes[k].audio;
			*/
		}
		i = 0;
		j = 0;
		trackList = new string[] {song1.audio.name, song2.audio.name, song3.audio.name, song4.audio.name};
		effectList = new string[] {"Reverb Filter", "Hi-Pass", "Lo-Pass", "Distortion Filter"};
	}

	void OnGUI(){
		GUI.TextField (new Rect (Screen.width-192, 10, 160, 32), "Track: " + trackList[i] + "\nEffect: " + effectList[j]);
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
			audioNodes[1].audio.Stop();
			audioNodes[2].audio.Stop();
			audioNodes[3].audio.Stop();
			audioNodes[4].audio.Stop();
		}
		else if(Input.GetKeyUp(KeyCode.Alpha1)){
			audioNodes[1].audio.Play();
		}
		else if(Input.GetKeyUp(KeyCode.Alpha2)){
			audioNodes[2].audio.Play();
		}
		else if(Input.GetKeyUp(KeyCode.Alpha3)){
			audioNodes[3].audio.Play();
		}
		else if(Input.GetKeyUp(KeyCode.Alpha4)){
			audioNodes[4].audio.Play();
		}

		if (Input.GetKeyUp (KeyCode.RightShift)||Input.GetKeyUp (KeyCode.LeftShift)) {
			if(j==(int)AudioEffect.reverbFilter)
				audioNodes[i].reverbFilter.reverbLevel = Input.mousePosition.y/Screen.height;
			else if(j==(int)AudioEffect.distortionFilter)
				audioNodes[i].distortionFilter.distortionLevel = Input.mousePosition.y/Screen.height;
			else if(j==(int)AudioEffect.LoPass)
				audioNodes[i].loPass.cutoffFrequency = Mathf.Clamp(Mathf.Pow(22000.0f, Input.mousePosition.y/Screen.height), 60, 16000);
			else if(j==(int)AudioEffect.HiPass)
				audioNodes[i].hiPass.cutoffFrequency = Mathf.Clamp(Mathf.Pow(22000.0f, Input.mousePosition.y/Screen.height), 60, 16000);
		}

	}
}
