using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PawDirControl : MonoBehaviour {

    public KeyCode startBindKey = KeyCode.Space;
    public float atBottomY = 5.53f;
    public float atBottomTime = 15f;

    enum State {
        Idle,
        Started,
        MoveDown,
        AtBottom,
        MoveBack
    }
    State m_state = State.Idle;
    Vector3 m_initPos;
    float m_atBottomTimer;

	void Start () {
        m_initPos = transform.position;
	}
	
    public bool TriggerStart () {
        return Input.GetKeyDown(startBindKey);
    }

    public void PawDirectionControl() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(1 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(0, 0, 1 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(0, 0, -1 * Time.deltaTime);
        }
    }

    public bool MoveDown () {
        if (transform.localPosition.y < atBottomY) return true;
        transform.Translate(0, -1 * Time.deltaTime, 0);
        return false;
    }

    public bool MoveToInitPos() {
        if (Mathf.Abs(transform.position.y - m_initPos.y) > 0.5f) {
            transform.Translate(0, 1 * Time.deltaTime, 0);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, m_initPos, 2 * Time.deltaTime);
        }
        return Vector3.Distance(transform.position, m_initPos) < 0.5f;
    }
}
