using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlay : MonoBehaviour {

//	double[][] _buffer;
//	int[] _bufferUseSize;
//	int _bufferingSeek = 0;
//	int _playingSeek = 0;
//	int _bufferingIndex = 0;
//	int _playingIndex = 0;
//	int _bufferNum = 4;
//	float _bufferTime = 0.3f;
//	//Change Buffer Time
//	float _bufferLimitTime = 0.4f;
//	//All Buffer Time
//	bool _started = false;

	public double[] monoral_data;

	public int position = 0;
	public int samplerate = 44100;
	public float frequency = 440;

	public bool enable;

	public double[] testAudio;

	void Start()
	{
		float[] testAudio2 = new float[20];
		memBuffer (testAudio2, 0, 0.5f, 20);

		if (enable) {
			AudioClip myClip = AudioClip.Create ("test", samplerate * 2, 1, samplerate, true, OnAudioRead);
			myClip.SetData (testAudio2, 0);
			AudioSource aud = GetComponent<AudioSource> ();
			aud.clip = myClip;
			//aud.loop = true;
			//aud.mute = true;
			aud.Play ();
		}


	}

	void OnAudioRead(float[] data)
	{
		assignAudio(data);
	}

//	void OnAudioSetPosition(int newPosition)
//	{
//		position = newPosition;
//	}

	void assignAudio(float[] data){
		if (data.Length < monoral_data.Length) {
			for (int i = 0; i < data.Length; i ++) {
				
					data [i] = (float)monoral_data [i];
				
			}	
		}

	}

	void Update(){
		for (int i = 0; i < testAudio.Length && testAudio.Length < monoral_data.Length; i++) {
			testAudio [i] = monoral_data [i];
		}
	}

	void memBuffer(float[] dst_buffer, int dst_startIndex, float value, int size)
	{
		for (int i = 0; i < size; i++)
		{
			dst_buffer[dst_startIndex + i] = value;
		}
	}
}
