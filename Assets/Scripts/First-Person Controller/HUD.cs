using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;

	//A collection of songs for the user to choose from to mix and modulate with incoming sound from the AudioListener
	public AudioClip[] clips;
	public AudioSource[] songs;

	public AudioLowPassFilter loPass;
	public AudioHighPassFilter hiPass;
	public AudioDistortionFilter distortionFilter;

	private string[] trackList;
	private string[] effectList;
	private int i, j;

	enum AudioEffect {HiPass=0, LoPass=1, distortionFilter = 2};
	private bool fxlock;
		
	// Use this for initialization
	void Start () {
		fxlock = false;
		i = 0;
		j = 0;
		trackList = new string[] {songs[0].clip.name, songs[1].clip.name, songs[2].clip.name, songs[3].clip.name};
		effectList = new string[] {"Hi-Pass", "Lo-Pass", "Distortion Filter"};
	}

	void OnGUI(){
		GUI.TextField (new Rect (Screen.width*(1.0f-0.45f), Screen.height*(0.05f), Screen.width*(.4f), Screen.width*(.3f)), 
		               "Track: " + trackList[i] + "\nEffect: " + effectList[j]);
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
			songs[0].audio.Play();
		} else if(Input.GetKeyUp(KeyCode.Alpha2)){
			songs[1].audio.Play();
		} else if(Input.GetKeyUp(KeyCode.Alpha3)){
			songs[2].audio.Play();
		} else if(Input.GetKeyUp(KeyCode.Alpha4)){
			songs[3].audio.Play();
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			songs[i].loop = !songs[i].loop;
		}
		if (!fxlock) {
			if(j==(int)AudioEffect.distortionFilter)
				distortionFilter.distortionLevel = Input.mousePosition.y/Screen.height;
			else if(j==(int)AudioEffect.LoPass)
				loPass.cutoffFrequency = Mathf.Clamp(Mathf.Pow(22000.0f, Input.mousePosition.y/Screen.height), 60, 16000);
			else if(j==(int)AudioEffect.HiPass)
				hiPass.cutoffFrequency = Mathf.Clamp(Mathf.Pow(22000.0f, Input.mousePosition.y/Screen.height), 60, 16000);
		}

		if (Input.GetKeyUp (KeyCode.RightShift)||Input.GetKeyUp (KeyCode.LeftShift)) {
			fxlock = !fxlock;
		}
	}
}
