using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject target;
	public float yOffset = 3f;

	[Header("At Side")]
	public Vector3 sidePosition;
	public Vector3 sideRotation;
	[Header("At Top")]
	public Vector3 topPosition;
	public Vector3 topRotation;

	bool m_isFollowing;
	Vector3 currentPos;

	public void MoveToSide (float duration, Action callback) {
		StartCoroutine (MoveTo (sidePosition, sideRotation, duration, callback));
	}

	public void MoveToTop (float duration, Action callback) {
		StartCoroutine (MoveTo (topPosition, topRotation, duration, callback));
	}

	public IEnumerator MoveTo (Vector3 position, Vector3 rotation, float duration, Action callback) {
		float timer = 0;
		Vector3 startPos = transform.localPosition;
		Quaternion startRot = transform.localRotation;
		Quaternion endRot = Quaternion.Euler (rotation);
		while (timer < duration) {
			this.transform.localPosition = Vector3.Lerp (startPos, position, timer / duration);
			this.transform.localRotation = Quaternion.Lerp (startRot, endRot, timer / duration);
			yield return null;
			timer += Time.deltaTime;
		}
		this.transform.localPosition = position;
		this.transform.localRotation = endRot;
		if (callback != null) callback.Invoke ();
	}

	public void SetFollowing (bool value) {
		m_isFollowing = value;
	}

	void Update () {
		if (m_isFollowing) {
			currentPos = this.transform.localPosition;
			this.transform.localPosition = new Vector3 (currentPos.x, target.transform.position.y + yOffset, currentPos.z);
		}
	}
}
