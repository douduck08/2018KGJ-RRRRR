using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public CameraController cameraController;
    public SoundController soundController;
	public PawDirControl pawDirControl;
	public PawClawControl[] pawClawControl;
	public UIController uiController;
	public HoleTrigger holeTrigger;

	public enum State {
        Idle,
		AtTop,
		MovingDown,
		AtBottom,
		MovingBack,
		WaitCoroutine
    }

	public State m_currentState;
	Dictionary<State, Action> m_stateAction;
	int m_score = 0;
	int m_round = 0;

	void Start () {
		holeTrigger.onSomethingPassed += Gotcha;
		m_stateAction = new Dictionary<State, Action> ();

		// set state
		m_stateAction.Add (State.Idle, () => { 
			if (pawDirControl.TriggerStart()) {
				cameraController.MoveToTop (3f, null);
				m_currentState = State.AtTop;
				m_round += 1;
				uiController.SetRound (m_round);
			}
		});

		m_stateAction.Add (State.AtTop, () => {
			pawDirControl.PawDirectionControl ();
			if (pawDirControl.TriggerStart()) {
				m_currentState = State.WaitCoroutine;
				cameraController.MoveToSide (3f, () => { 
					cameraController.SetFollowing (true);
					m_currentState = State.MovingDown;
				});
			}
		});

		m_stateAction.Add (State.MovingDown, () => {
			pawDirControl.PawDirectionControl ();
			if (pawDirControl.MoveDown ()) {
				m_currentState = State.AtBottom;
			}
		});

		m_stateAction.Add (State.AtBottom, () => { 
			pawDirControl.PawDirectionControl ();
			StartCoroutine (CountDown(pawDirControl.atBottomTime, () => {
				m_currentState = State.MovingBack;
			}));
		});

		m_stateAction.Add (State.MovingBack, () => {
			if (pawDirControl.MoveToInitPos()) {
				cameraController.SetFollowing (false);
				m_currentState = State.WaitCoroutine;
				StartCoroutine (CountDown(2f, () => {
					StartCoroutine (uiController.ShowFailed (() => {
						m_currentState = State.Idle;
					}));
				}));
			}
		});

		m_stateAction.Add (State.WaitCoroutine, () => { });

		// game open
		m_currentState = State.WaitCoroutine;
		cameraController.MoveToSide (3f, () => {
			m_currentState = State.Idle;
			StartCoroutine (uiController.ShowUIBar (null));
		});
	}

	void Update () {
		m_stateAction[m_currentState].Invoke ();
	}

	void Gotcha () {
		m_score += 1;
		uiController.SetScore (m_score);
		m_currentState = State.WaitCoroutine;
		StartCoroutine (uiController.ShowGotcha (() => {
			m_currentState = State.Idle;
		}));
	}

	IEnumerator CountDown (float time, Action callback) {
		yield return new WaitForSeconds(time);
		callback.Invoke ();
	}
}
