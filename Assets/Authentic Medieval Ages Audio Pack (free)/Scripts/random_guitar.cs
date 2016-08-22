using UnityEngine;
using System.Collections;

public class random_guitar : MonoBehaviour {

	public AudioSource audio_guitarA;
	public AudioSource audio_guitarB;
	int guitarA;
	int guitarB;
	Object[] AudioArray_guitarA;
	Object[] AudioArray_guitarB;
	public float bpm = 100.0F;
	public int beatsPerMeasure = 4;
	private double singleMeasureTime;
	private double delayEvent;
	private bool running = false;
	private int i;
	
	// Use this for initialization
	void Start () {
		bpm = 100.0F;
		beatsPerMeasure = 4;
		int i = 0;
		singleMeasureTime = AudioSettings.dspTime + 2.0F;
		running = true;
		guitarA = Random.Range (0, 4);
		guitarB = Random.Range (0, 4);
		
		
		
		//Load the GUITAR A samples into the AudioClip Array
		audio_guitarA = (AudioSource)gameObject.AddComponent <AudioSource>();

		AudioArray_guitarA = Resources.LoadAll("random/minor/4-4/100/guitar",typeof(AudioClip));
		
		Debug.Log("The Length of the GUITAR A array is " + AudioArray_guitarA.Length);
		audio_guitarA.clip = AudioArray_guitarA[guitarA] as AudioClip;

		
		//Load the GUITAR B samples into the AudioClip Array
		audio_guitarB = (AudioSource)gameObject.AddComponent <AudioSource>();

		AudioArray_guitarB = Resources.LoadAll("random/minor/4-4/100/guitar",typeof(AudioClip));
		
		Debug.Log("The Length of the GUITAR B array is " + AudioArray_guitarB.Length);
		
		audio_guitarB.clip = AudioArray_guitarB[guitarB] as AudioClip;
		
	
		
		
	}	
	
	// Update is called once per frame
	void Update () {
		if (!running)
			return;
		double time = AudioSettings.dspTime;
		if (i == 0) {



			if (time + 1.0F > singleMeasureTime) {
				guitarA = Random.Range (0, 4);
				audio_guitarA.clip = AudioArray_guitarA[guitarA] as AudioClip;
				audio_guitarA.PlayScheduled (time);

				
			}
		}
		if (i==4 ){
			if (time + 1.0F > singleMeasureTime) {
				guitarB = Random.Range (0, 4);
				audio_guitarB.clip = AudioArray_guitarB[guitarB] as AudioClip;
				audio_guitarB.PlayScheduled(time);

				
			}
		}
		
		//THE most important part of this script: this is the metronome, keeping count of the measures and making sure the audio is in sync
		if (time + 1.0F > singleMeasureTime) {
			i +=1;
			Debug.Log ("The i int equals  " + i);
			if (i==8){
				i = 0;
			}
			singleMeasureTime += 60.0F / bpm * beatsPerMeasure;
			Debug.Log("The single measure time is " + singleMeasureTime);
		}
	}
	
	
}