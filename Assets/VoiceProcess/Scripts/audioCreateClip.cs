using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioCreateClip : MonoBehaviour {

	public double[] monoral_data;

	public float[] fdata;

	public double[] test;

	public int position = 0;

	public int samplerate = 48000;

	public float frequency = 440;

	AudioSource aud;

	AudioClip aclip;

	public audioSetup asetup;

	public TrackableAudio taudio;

//	void Start(){
//		AudioClip myClip = AudioClip.Create ("test", samplerate/2, 2, samplerate, true, onAudioRead, onAudioSetPosition);
//		aud = GetComponent<AudioSource> ();
//		aud.clip = myClip;
//		aclip = aud.clip;
//		//aud.Play ();
//	}

//	void onAudioRead(float[] data){
//		//print ("data.length:" + data.Length);
//		int len = data.Length < monoral_data.Length ? data.Length : monoral_data.Length;
//		int count = 0;
//		while (count < len) {
//			//data [count] = Mathf.Sign (Mathf.Sin (2 * Mathf.PI * frequency * position / samplerate) / 2);
//			//data [count] = Mathf.Sign(Mathf.Sin(Mathf.Deg2Rad* count)) / (count % 20 + 0.5f) * 0.001f;
//			data[count] =  (float)monoral_data [count];
//			position++;
//			count++;
//		}
//		//print (data [0]);print (data [1]);print (data [2]);
//		//fdata = new float[monoral_data.Length];
//		//copyBuffer (monoral_data, data, 0, 0, monoral_data.Length);
////		aclip.SetData (fdata, 0);
//	}

//	void onAudioSetPosition(int newPosition){
//		//position = newPosition;
//	}

	void Update(){
		//print ("monoral_data.length:" + monoral_data.Length);
		if (test.Length < taudio.audio_data.Length) {
			for (int i = 0; i < test.Length; i++) {
				test [i] = taudio.audio_data [i];
			}
		}

	}

//	void copyBuffer(double[] src_buffer, float[] dst_buffer, int src_startIndex, int dst_startIndex, int size)
//	{
//		//if this case happened. mean buffer over run
//		if (src_buffer.Length < src_startIndex + size || dst_buffer.Length < dst_startIndex + size)
//		{
//			Debug.Log("error");
//		}
//
//		for (int i = 0; i < size; i++)
//		{
//			dst_buffer[dst_startIndex + i] = (float)src_buffer[src_startIndex + i];
//		}
//	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		print ("channel:" + channels);
		if (data.Length == 0)
			return;
		if (taudio.audio_data.Length == 0)
			return;
		//fill monoral data to channels
		for (int i = 0; i < data.Length; i+=channels)
		{
			int j = (int)(i / channels);
			for (int k = 0; k < channels; k++)
			{
				// final
				data[i+k] = (float)taudio.audio_data[j]*0.1f + 0.2f;
			}
		}
	}
}
