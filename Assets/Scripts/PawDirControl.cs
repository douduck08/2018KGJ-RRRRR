using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawDirControl : MonoBehaviour {

    public KeyCode startBindKey = KeyCode.Space;
    public float atBottomTime = 15f;

    public int moveDown = 2;
    public int money = 1000;
    public bool canCatch = true;
    public bool insertCoin = false;
    public bool gameStart = false;
    public Vector3 initPos;
    public float stayTime = 5f;
    public bool init = false;
    public bool timing = false;

    enum Status {
        Idle,
        Started,
        MoveDown,
        AtBottom,
        MoveBack
    }
    Status m_status = Status.Idle;
    float m_atBottomTimer;

	// Use this for initialization
	void Start () {
        money = 1000;
        initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        switch (m_status) {
            case Status.Idle:
                if (Input.GetKeyDown(startBindKey)) {
                    m_status = Status.Started;
                }
                break;
            case Status.Started:
                PawDirectionControl ();
                if (Input.GetKeyDown(startBindKey)) {
                    m_status = Status.MoveDown;
                }
                break;
            case Status.MoveDown:
                PawDirectionControl ();
                MoveDown ();
                break;
            case Status.AtBottom:
                PawDirectionControl ();
                if (m_atBottomTimer > 0f) {
                    m_atBottomTimer -= Time.deltaTime;
                } else {
                    m_status = Status.MoveBack;
                }
                break;
            case Status.MoveBack:
                if (MoveToInitPos()) {
                    m_status = Status.Idle;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(m_status == Status.MoveDown && other.tag == "Bottom") {
            m_status = Status.AtBottom;
            m_atBottomTimer = atBottomTime;
        }
    }

    void PawDirectionControl() {
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

    void MoveDown () {
        transform.Translate(0, -1 * Time.deltaTime, 0);
    }

    void GoUp() {
        moveDown = 1;
    }

    bool MoveToInitPos() {
        if (Mathf.Abs(transform.position.y - initPos.y) > 0.5f) {
            transform.Translate(0, 1 * Time.deltaTime, 0);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, initPos, 2 * Time.deltaTime);
        }
        return Vector3.Distance(transform.position, initPos) < 0.5f;
    }
}
