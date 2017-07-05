using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioFilter : MonoBehaviour {


	int outputSampleRate = 48000;

	void Awake()
	{
		outputSampleRate = AudioSettings.outputSampleRate;
	}

	void Start()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		if (audioSource != null && audioSource.clip != null)
		{
			outputSampleRate = audioSource.clip.frequency;
			print ("outputSampleRate:" + outputSampleRate);
		}

		_buffer = new double[_bufferNum][];
		for (int i = 0; i < _bufferNum; i++)
		{
			_buffer[i] = new double[(int)(outputSampleRate * _bufferLimitTime)];
		}
		_bufferUseSize = new int[_bufferNum];

		_started = true;

		taudio = GetComponent<TrackableAudio> ();
	}

	double[][] _buffer;
	int[] _bufferUseSize;
	int _bufferingSeek = 0;
	int _playingSeek = 0;
	int _bufferingIndex = 0;
	int _playingIndex = 0;
	int _bufferNum = 3;
	float _bufferTime = 0.01f;           //Change Buffer Time
	float _bufferLimitTime = 0.5f;  //All Buffer Time
	bool _started = false;



	public audioCreateClip aplay;
	public TrackableAudio taudio;
	void OnAudioFilterRead(float[] data, int channels)
	{
		print ("channel:" + channels);
		if (data.Length == 0)
			return;

		if (!_started)
			return;

		int monoralDataLength = data.Length / channels;
		aplay.monoral_data = new double[monoralDataLength];


		//monoral
		if (channels == 1)		{
			for (int i = 0; i < data.Length; i++){
				aplay.monoral_data[i] = data[i];
			}

			OnProcess(aplay.monoral_data);

			for (int i = 0; i < data.Length; i++){
				aplay.monoral_data[i] = (float)data[i];
			}
		}else{
			//convert stereo sound to monoral
			for (int i = 0; i < monoralDataLength; i++){
				aplay.monoral_data[i] = data[i * channels];
			}

			OnProcess(aplay.monoral_data);

			//fill monoral data to channels
			for (int i = 0; i < data.Length; i+=channels){
				int j = (int)(i / channels);
				for (int k = 0; k < channels; k++){
					// final
					data[i+k] = 0;//(float)aplay.monoral_data[j]*0.1f;
				}
			}
		}
		if (taudio.Host) {
			taudio.audio_data = new double[monoralDataLength];
			copyBuffer (aplay.monoral_data, taudio.audio_data, 0, 0, aplay.monoral_data.Length);
		}
	}

	void OnProcess(double[] monoral_data)
	{
		//process charge buffer
		if (_bufferingSeek < _bufferTime * outputSampleRate)
		{
			copyBuffer(monoral_data, _buffer[_bufferingIndex], 0, _bufferingSeek, monoral_data.Length);
			_bufferingSeek += monoral_data.Length;
			_bufferUseSize[_bufferingIndex] = _bufferingSeek;
		}
		else //change to next buffer
		{
			_bufferingIndex = (_bufferingIndex + 1) % _bufferNum;
			_bufferingSeek = 0;
			copyBuffer(monoral_data, _buffer[_bufferingIndex], 0, _bufferingSeek, monoral_data.Length);
			_bufferingSeek += monoral_data.Length;
			_bufferUseSize[_bufferingIndex] = _bufferingSeek;
		}

		//wait for change buffer
		if ((_playingIndex + 2) % _bufferNum == _bufferingIndex){
			//process play sound
			if (_playingSeek + monoral_data.Length < _bufferUseSize[_playingIndex]){
				copyBuffer(_buffer[_playingIndex], monoral_data, _playingSeek, 0, monoral_data.Length);
				_playingSeek += monoral_data.Length;
			}
			else //change to next buffer
			{
				int copyBufferSize = _bufferUseSize[_playingIndex] - _playingSeek;
				copyBuffer(_buffer[_playingIndex], monoral_data, _playingSeek, 0, copyBufferSize);  
				_playingIndex = (_playingIndex + 1) % _bufferNum; 
				_playingSeek = 0;

				int nextCopyBufferSize = monoral_data.Length - copyBufferSize;
				copyBuffer(_buffer[_playingIndex], monoral_data, _playingSeek, copyBufferSize, nextCopyBufferSize);
				_playingSeek += nextCopyBufferSize;
			}
		}
		else{
			memBuffer(monoral_data, 0, 0f, monoral_data.Length);
		}
	}

	void copyBuffer(double[] src_buffer, double[] dst_buffer, int src_startIndex, int dst_startIndex, int size)
	{
		//if this case happened. mean buffer over run
		if (src_buffer.Length < src_startIndex + size || dst_buffer.Length < dst_startIndex + size){
			Debug.Log("error");
		}

		for (int i = 0; i < size; i++){
			dst_buffer[dst_startIndex + i] = src_buffer[src_startIndex + i];
		}
	}

	void memBuffer(double[] dst_buffer, int dst_startIndex, double value, int size)
	{
		for (int i = 0; i < size; i++){
			dst_buffer[dst_startIndex + i] = value;
		}
	}
}
