using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioProcess : MonoBehaviour {

	int outputSampleRate = 48000;

	public float[] testAudio;

	void Awake() {
		outputSampleRate = AudioSettings.outputSampleRate;
	}

	void Start() {
		AudioSource audioSource = GetComponent<AudioSource> ();
		if (audioSource != null && audioSource.clip != null) {
			outputSampleRate = audioSource.clip.frequency;
		}


//		float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
//		audioSource.clip.GetData(samples, 0);
//
//		audioSource.clip.GetData(audioplay.testAudio, 0);
//
//		for (int i = 0; i < samples.Length; i++) {
//			samples [i] = 0;
//		}
//
//		audioSource.clip.SetData (samples,0);


	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		if (data.Length == 0)
			return;

		//stereo or more channels	
			int monoralDataLength = data.Length / channels;
			//convert stereo sound to monoral
			audioplay.monoral_data = new double[monoralDataLength];

			for (int i = 0; i < monoralDataLength; i++)
			{
				audioplay.monoral_data[i] = data[i * channels];
			}

			//fill monoral data to channels
//			for (int i = 0; i < data.Length; i+=channels)
//			{				
//					data[i] = 0;				
//			}
			memBuffer(data,0,0.0f,data.Length);
	}

	void copyBuffer(double[] src_buffer, double[] dst_buffer, int src_startIndex, int dst_startIndex, int size) {
		//if this case happened. mean buffer over run
		if (src_buffer.Length < src_startIndex + size || dst_buffer.Length < dst_startIndex + size) {
			Debug.Log ("error");
		}

		for (int i = 0; i < size; i++) {
			dst_buffer [dst_startIndex + i] = src_buffer [src_startIndex + i];
		}
	}

	void memBuffer(float[] dst_buffer, int dst_startIndex, float value, int size) {
		for (int i = 0; i < size; i++) {
			dst_buffer [dst_startIndex + i] = value;
		}
	}

	public audioPlay audioplay;

}
