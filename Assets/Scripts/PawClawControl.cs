using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawClawControl : MonoBehaviour {

    public KeyCode grabBindKey;
    public float grabSpeed = 3f;
    public float recoverySpeed = -0.1f;
    public float gradDurability = 0.1f;
    public Vector2 animeBound = new Vector2 (0, 0.95f);
    
    public SoundController sound;

    Animator m_anim;
    float m_value = 0;
    float m_isGrabing = 0f;

	void Awake () {
        m_anim = GetComponent<Animator>();
        m_anim.speed = 0;
        m_value = 0;
	}

    void Update () {
        if (m_isGrabing <= 0f && Input.GetKeyDown(grabBindKey)){
            m_isGrabing = gradDurability;
            sound.PlayYell ();
        }
        
        if (m_isGrabing > 0f) {
            m_isGrabing -= Time.deltaTime;
            m_value += grabSpeed * Time.deltaTime;
        } else {
            m_value += recoverySpeed * Time.deltaTime;
        }

        m_value = Mathf.Clamp (m_value, animeBound.x, animeBound.y);
        m_anim.Play("Abc", -1, m_value);
    }
}
