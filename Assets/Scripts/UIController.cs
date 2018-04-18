using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	[Header ("UI Bar")]
	public RectTransform topBar;
	public RectTransform botBar;
	public float showDuration = 2f;
	public Text scoreText;
	public Text roundText;

	[Header ("RawImage")]
	public RawImage rawImage;
	public float rollingSpeed = 1f;
	public float rollingDuration = 2f;
	public Color failedTextColor;
	public Texture failedTexture;
	public Color gotchaTextColor;
	public Texture gotchaTexture;
	
	float m_halfBarHeight;
	Rect m_uvRect;

	void Awake () {
		m_halfBarHeight = topBar.sizeDelta.y / 2f;
		topBar.anchoredPosition = new Vector2 (0, m_halfBarHeight);
		botBar.anchoredPosition = new Vector2 (0, -m_halfBarHeight);

		rawImage.gameObject.SetActive (false);
		m_uvRect = rawImage.uvRect;
	}

	public void SetScore (int score) {
		scoreText.text = string.Format("GOTCHA-{0:00}", score);
	}

	public void SetRound (int round) {
		roundText.text = string.Format("ROUND-{0:000}", round);
	}

	public IEnumerator ShowUIBar (Action callback) {
		Vector2 pos = new Vector2 (0, m_halfBarHeight);
		Vector2 neg = new Vector2 (0, -m_halfBarHeight);
		float timer = 0f;
		while (timer < rollingDuration) {
			topBar.anchoredPosition = Vector2.Lerp (pos, neg, timer / showDuration);
			botBar.anchoredPosition = Vector2.Lerp (neg, pos, timer / showDuration);
			yield return null;
			timer += Time.deltaTime;
		}
		if (callback != null) callback.Invoke ();
	}

	public IEnumerator ShowFailed (Action callback) {
		rawImage.gameObject.SetActive (true);
		rawImage.texture = failedTexture;
		Color clean = failedTextColor;
		clean.a = 0f;
		float timer = 0f;
		while (timer < rollingDuration) {
			if (timer < rollingDuration * 0.25f) {
				rawImage.color = Color.Lerp (clean, failedTextColor, timer * 4f / rollingDuration);
			} else if (timer > rollingDuration * 0.75f) {
				rawImage.color = Color.Lerp (failedTextColor, clean, timer * 4f / rollingDuration - 3f);
			}
			m_uvRect.x = (m_uvRect.x + rollingSpeed * Time.deltaTime) % 1f;
			rawImage.uvRect = m_uvRect;
			timer += Time.deltaTime;
			yield return null;
		}
		rawImage.gameObject.SetActive (false);
		if (callback != null) callback.Invoke ();
	}

	public IEnumerator ShowGotcha (Action callback) {
		rawImage.gameObject.SetActive (true);
		rawImage.texture = gotchaTexture;
		Color clean = gotchaTextColor;
		clean.a = 0f;
		float timer = 0f;
		while (timer < rollingDuration) {
			if (timer < rollingDuration * 0.25f) {
				rawImage.color = Color.Lerp (clean, gotchaTextColor, timer * 4f / rollingDuration);
			} else if (timer > rollingDuration * 0.75f) {
				rawImage.color = Color.Lerp (gotchaTextColor, clean, timer * 4f / rollingDuration - 3f);
			}
			m_uvRect.x = (m_uvRect.x + rollingSpeed * Time.deltaTime) % 1f;
			rawImage.uvRect = m_uvRect;
			timer += Time.deltaTime;
			yield return null;
		}
		rawImage.gameObject.SetActive (false);
		if (callback != null) callback.Invoke ();
	}

}
