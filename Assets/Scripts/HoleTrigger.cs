using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour {

	public event System.Action onSomethingPassed;

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Object") {
			StartCoroutine (DelayDestory(collider.gameObject));
			if (onSomethingPassed != null) onSomethingPassed.Invoke ();
		}
	}

	IEnumerator DelayDestory (GameObject go) {
		yield return new WaitForSeconds (1f);
		Destroy (go);
	} 
}
