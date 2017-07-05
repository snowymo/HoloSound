using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackableAudio : Holojam.Tools.SynchronizableTrackable {

	[SerializeField] string label = "audio";
	[SerializeField] string scope = "";

	[SerializeField] bool host = true;
	[SerializeField] bool autoHost = false;

	public double[] audio_data;

	public void SetLabel(string label) { this.label = label; }
	public void SetScope(string scope) { this.scope = scope; }

	public void SetHost(bool host) { this.host = host; }
	public void SetAutoHost(bool autoHost) { this.autoHost = autoHost; }

	// Point the property overrides to the public inspector fields

	public override string Label { get { return label; } }
	public override string Scope { get { return scope; } }

	public override bool Host { get { return host; } }
	public override bool AutoHost { get { return autoHost; } }

	public override void ResetData() {
		data = new Holojam.Network.Flake(0,0,0,0,1);
	}

	// Override Sync() to include the scale vector
	protected override void Sync() {
		//base.Sync();

		if (Sending) {
			Encode (audio_data);
		} else {
			Decode (data.bytes);
		}
	}

	protected override void Update() {
		if (autoHost) host = Sending; // Lock host flag
		base.Update();
	}
		
	public void ParseFromInt(byte[] value, int length) {
		//Byte[] bytes = BitConverter.GetBytes(length);
//		for (int i = 0; i < 4; i++) {
//			value [i] = bytes [i];
//		}
//		print ("use for loop:" + BitConverter.ToString( value));
		int[] testint = new int[1];
		testint [0] = length;
		Buffer.BlockCopy (testint, 0, value, 0, 4);
		//print ("use buffer copy:" + BitConverter.ToString( value));
	}

	public void ParseFromDouble(byte[] value, double[] audio) {
		Buffer.BlockCopy (audio, 0, value, 4, audio.Length * 8);
		print ("use buffer copy:" + BitConverter.ToString( value));
//		for (int i = 0; i < audio.Length; i++) {
//			Byte[] bytes = BitConverter.GetBytes(audio[i]);	
//			for (int j = 0; i < 8; i++) {
//				value [j] = bytes [j];
//			}
//		}
	}

	public void Encode(double[] audioData){
		//add length at the beginning
		data.bytes = new byte[audio_data.Length*8+4];
		ParseFromInt (data.bytes, audio_data.Length);
		ParseFromDouble (data.bytes, audio_data);
	}

	public void ParsetoDouble(byte[] value, int index, double[] audio) {
		//return BitConverter.ToInt32(value, index) < 0 ? BitConverter.ToInt16(value, index) + 0x10000 : BitConverter.ToInt16(value, index);
		Buffer.BlockCopy (value, index, audio, 0, audio.Length * 8);
	}
	public int ParsetoInt(byte[] value) {
		return BitConverter.ToInt16(value, 0);
	}

	public void Decode(byte[] audioBytes){
		if (audioBytes.Length <= 4)
			return;
		print ("audioBytes:" + BitConverter.ToString( audioBytes));
		int len = ParsetoInt (audioBytes);
		if (len > 0) {
			audio_data = new double[len];
			ParsetoDouble (audioBytes, 4, audio_data);
		}
	}

}
