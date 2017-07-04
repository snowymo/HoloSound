using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSetup : MonoBehaviour {

	public AudioSource audio;

	public int step = 0;

	// Use this for initialization
	IEnumerator Start () {
		//for (;step != 1;) {
			print("step:" + step);
		//}
			

			audio = GetComponent<AudioSource> ();
			if (Microphone.devices.Length == 0)
				yield break;
			audio.clip = Microphone.Start (null, true, 5, AudioSettings.outputSampleRate);
			print ("microphone:" + AudioSettings.outputSampleRate);
			while (Microphone.GetPosition (null) <= 0) {
				yield return 0;
			}
			//audio.mute = true;
			audio.Play ();

			print ("audio.frequency:" + audio.clip.frequency + "\taudio.length:" + audio.clip.length + "\taudio.samples" + audio.clip.samples + "\taudio.channel:" + audio.clip.channels);
		
	}
}
