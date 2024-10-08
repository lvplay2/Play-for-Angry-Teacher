using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StudentAI : MonoBehaviour
{
	private enum StudentState
	{
		ToNotebook = 0,
		ToExit = 1
	}

	public Transform navMovePointsParent;

	public Transform exitNavMovePointsParent;

	private float speed = 2.5f;

	private float staminaSpeed = 3.2f;

	private NavMeshAgent navMeshAgent;

	private StudentState state;

	private List<Vector3> _movePointsPos;

	private int _markerCount;

	private Vector3 _currentTargetPosition;

	private Coroutine staminaTick;

	private Coroutine staminaRestoreTick;

	private float staminaValue = 10f;

	[HideInInspector]
	private bool isDangerous;

	private bool isStamina;

	private bool isRestore;

	private bool isMop;

	private bool isPlayTime;

	private bool isFreeze;

	private bool isSlow;

	protected void Start()
	{
		speed = GameplayManager.This.studentSpeed[MultiSceneManager.This.GetDifficulty()];
		staminaSpeed = GameplayManager.This.studentStaminaSpeed[MultiSceneManager.This.GetDifficulty()];
		navMeshAgent = GetComponent<NavMeshAgent>();
		if (navMovePointsParent == null)
		{
			Debug.LogError("Set (navMovePointsParent) variable on " + base.gameObject.name);
			Application.Quit();
		}
		_markerCount = navMovePointsParent.childCount;
		_movePointsPos = new List<Vector3>();
		for (int i = 0; i < _markerCount; i++)
		{
			_movePointsPos.Add(navMovePointsParent.GetChild(i).position);
		}
		state = StudentState.ToNotebook;
		SetTarget(_movePointsPos[Random.Range(0, 2)]);
		UpdateSpeed();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "RoomEnter"))
		{
			return;
		}
		if (other.GetComponent<RoomEnter>().IsEnter() && _markerCount > 1 && Sing_Game.This.canvasGame.sliderDistance.value < GameplayManager.This.roomEnterMinDistance)
		{
			Vector3 vector = _movePointsPos[Random.Range(0, _markerCount)];
			while (_currentTargetPosition == vector)
			{
				vector = _movePointsPos[Random.Range(0, _markerCount)];
			}
			SetTarget(vector);
		}
		else
		{
			other.GetComponent<RoomEnter>().MY_Enter();
		}
	}

	public void MY_NotebookGet(Vector3 _position)
	{
		if (!(_currentTargetPosition == _position) || state != 0)
		{
			return;
		}
		_movePointsPos.Remove(_position);
		_markerCount--;
		Sing_Game.This.MY_NextBook();
		if (_markerCount <= 0)
		{
			_markerCount = exitNavMovePointsParent.childCount;
			_movePointsPos.Clear();
			for (int i = 0; i < _markerCount; i++)
			{
				_movePointsPos.Add(exitNavMovePointsParent.GetChild(i).position);
			}
			state = StudentState.ToExit;
			Sing_Game.This.storyGame.MY_EnableExitWalls(_isOn: false);
			SetTarget(_movePointsPos[Random.Range(0, _markerCount)]);
		}
		else if (Sing_Game.This.bookCount < 2)
		{
			SetTarget(_movePointsPos[0]);
		}
		else
		{
			SetTarget(_movePointsPos[Random.Range(0, _markerCount)]);
		}
	}

	public void MY_GetExit(Vector3 _position, string _name)
	{
		if (_currentTargetPosition == _position && state == StudentState.ToExit)
		{
			_movePointsPos.Remove(_position);
			_markerCount--;
			if (_markerCount == 0)
			{
				Sing_Game.This.MY_Lose();
				return;
			}
			Sing_Game.This.storyGame.MY_LockExit(int.Parse(_name));
			SetTarget(_movePointsPos[Random.Range(0, _markerCount)]);
		}
	}

	public void MY_SetDangerous(bool _isDang)
	{
		if (isDangerous == _isDang)
		{
			return;
		}
		isDangerous = _isDang;
		UpdateSpeed();
		if (isDangerous)
		{
			EnableRestoreStamina(_isOn: false);
			if (staminaValue > 0f)
			{
				EnableStamina(_isOn: true);
			}
		}
		else
		{
			EnableStamina(_isOn: false);
			if (staminaValue < 5f)
			{
				EnableRestoreStamina(_isOn: true);
			}
		}
	}

	public void MY_MopEnter(bool _isOn, Transform _parent = null)
	{
		isMop = _isOn;
		UpdateSpeed();
		if (_isOn)
		{
			EnableRestoreStamina(_isOn: true);
			base.transform.parent = _parent;
			base.transform.position = _parent.position;
			navMeshAgent.enabled = false;
			Invoke("ActivateNavAgent", Random.Range(5, 10));
		}
		else
		{
			base.transform.parent = null;
			CancelInvoke("ActivateNavAgent");
			navMeshAgent.enabled = true;
			navMeshAgent.SetDestination(_currentTargetPosition);
		}
		UpdateSpeed();
	}

	public void MY_PlayTimePlay(bool _isOn)
	{
		isPlayTime = _isOn;
		if (_isOn)
		{
			EnableRestoreStamina(_isOn: true);
		}
		else if (!isMop)
		{
			EnableRestoreStamina(_isOn: false);
		}
		UpdateSpeed();
	}

	public void MY_Win()
	{
		if (!isMop)
		{
			Sing_Game.This.MY_Win();
		}
	}

	public void SetFreeze()
	{
		isFreeze = true;
		UpdateSpeed();
		Invoke("UnFreeze", GameplayManager.This.boostFreezeTime);
	}

	public void SetSlow()
	{
		isSlow = true;
		UpdateSpeed();
		Invoke("UnSlow", GameplayManager.This.boostSlowTime);
	}

	private void ActivateNavAgent()
	{
		navMeshAgent.enabled = true;
		base.transform.parent = null;
		isMop = false;
		navMeshAgent.SetDestination(_currentTargetPosition);
		if (isPlayTime)
		{
			EnableRestoreStamina(_isOn: true);
		}
		else
		{
			EnableRestoreStamina(_isOn: false);
		}
		UpdateSpeed();
	}

	private void SetTarget(Vector3 _position)
	{
		_currentTargetPosition = _position;
		if (navMeshAgent.enabled)
		{
			navMeshAgent.SetDestination(_position);
		}
	}

	private void EnableStamina(bool _isOn)
	{
		isStamina = _isOn;
		UpdateSpeed();
		if (staminaTick != null)
		{
			StopCoroutine(staminaTick);
		}
		if (_isOn)
		{
			staminaTick = StartCoroutine("StaminaTick");
		}
	}

	private void EnableRestoreStamina(bool _isOn)
	{
		if (staminaRestoreTick != null)
		{
			StopCoroutine(staminaRestoreTick);
		}
		if (_isOn)
		{
			staminaRestoreTick = StartCoroutine("RestoreStamina");
		}
	}

	private IEnumerator StaminaTick()
	{
		while (staminaValue > 0f)
		{
			staminaValue -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		EnableStamina(_isOn: false);
	}

	private IEnumerator RestoreStamina()
	{
		isRestore = true;
		UpdateSpeed();
		while (staminaValue < 10f && isRestore)
		{
			staminaValue += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		isRestore = false;
		UpdateSpeed();
	}

	private void UpdateSpeed()
	{
		if (isMop || isPlayTime || isFreeze)
		{
			navMeshAgent.speed = 0f;
		}
		else if (isDangerous && isStamina)
		{
			if (isSlow)
			{
				navMeshAgent.speed = staminaSpeed - GameplayManager.This.boostSlowSpeedStudent;
			}
			else
			{
				navMeshAgent.speed = staminaSpeed;
			}
		}
		else if (isSlow)
		{
			navMeshAgent.speed = speed - GameplayManager.This.boostSlowSpeedStudent;
		}
		else
		{
			navMeshAgent.speed = speed;
		}
	}

	private void UnFreeze()
	{
		isFreeze = false;
		UpdateSpeed();
	}

	private void UnSlow()
	{
		isSlow = false;
		UpdateSpeed();
	}
}
