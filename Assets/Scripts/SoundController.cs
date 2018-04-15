using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public AudioClip[] yells;

	public AudioSource bgm;
	public AudioSource soundEffect;

	public void PlayYell () {
		if (!soundEffect.isPlaying) {
			soundEffect.clip = yells[Random.Range (0, yells.Length)];
			soundEffect.Play ();
		}
	}

}
