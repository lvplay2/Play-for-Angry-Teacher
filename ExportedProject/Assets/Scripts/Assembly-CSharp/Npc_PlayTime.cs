using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc_PlayTime : MonoBehaviour, INpcEvent, INpcZone
{
	private enum PlayTimeState
	{
		None = 0,
		MovePoints = 1,
		StudentRush = 2
	}

	public NavMeshAgent navMeshAgent;

	public Transform movingPointsParent;

	public float navSpeed = 3f;

	public GameObject npcZone;

	public AudioSource audioSource;

	public AudioClip audioJumpEnd;

	public AudioClip randomAudio;

	private List<Vector3> movingPoints = new List<Vector3>();

	private int movingPointsCount;

	private Vector3 aimPoint;

	private PlayTimeState currentState;

	private Coroutine rushCoroutine;

	private Transform studentTarger;

	private bool isRelax;

	private bool isMop;

	private void Start()
	{
		foreach (Transform item in movingPointsParent)
		{
			movingPoints.Add(item.position);
		}
		movingPointsCount = movingPoints.Count;
		studentTarger = Sing_Game.This.studentContr.transform;
		currentState = PlayTimeState.MovePoints;
		navMeshAgent.speed = navSpeed;
		SetRandomTarger();
		InvokeRepeating("AudioRandom", 40f, Random.Range(35, 45));
	}

	public void MY_MarkerEnter(Vector3 _pos)
	{
		if (aimPoint == _pos && currentState == PlayTimeState.MovePoints)
		{
			SetRandomTarger();
		}
	}

	public void MY_ZoneEnter(Collider other)
	{
		if (other.tag == "Student" && currentState != PlayTimeState.StudentRush)
		{
			if (rushCoroutine != null)
			{
				StopCoroutine(rushCoroutine);
			}
			rushCoroutine = StartCoroutine("MoveToTarget");
			currentState = PlayTimeState.StudentRush;
		}
	}

	public void MY_ZoneExit(Collider other)
	{
		if (other.tag == "Student" && currentState == PlayTimeState.StudentRush)
		{
			if (rushCoroutine != null)
			{
				StopCoroutine(rushCoroutine);
			}
			currentState = PlayTimeState.MovePoints;
			SetRandomTarger();
		}
	}

	public void MY_ZoneStay(Collider other)
	{
	}

	public void MY_GetPlay()
	{
		if (!isRelax)
		{
			isRelax = true;
			npcZone.SetActive(value: false);
			navMeshAgent.speed = 0f;
			Sing_Game.This.studentContr.MY_PlayTimePlay(_isOn: true);
			if (rushCoroutine != null)
			{
				StopCoroutine(rushCoroutine);
			}
			Invoke("EndPlaying", 5f);
		}
	}

	public void MY_MopEnter(bool _isOn, Transform _parent = null)
	{
		isMop = _isOn;
		if (_isOn)
		{
			if (rushCoroutine != null)
			{
				StopCoroutine(rushCoroutine);
			}
			base.transform.parent = _parent;
			base.transform.position = _parent.position;
			navMeshAgent.enabled = false;
			Invoke("ActivateNavAgent", Random.Range(5, 10));
			CancelInvoke("RelaxEnd");
		}
		else
		{
			base.transform.parent = null;
			CancelInvoke("ActivateNavAgent");
			navMeshAgent.enabled = true;
			navMeshAgent.SetDestination(aimPoint);
			navMeshAgent.speed = navSpeed;
			currentState = PlayTimeState.MovePoints;
			npcZone.SetActive(value: false);
			Invoke("RelaxEnd", 30f);
		}
	}

	private void ActivateNavAgent()
	{
		navMeshAgent.enabled = true;
		base.transform.parent = null;
		navMeshAgent.SetDestination(aimPoint);
	}

	private void EndPlaying()
	{
		navMeshAgent.speed = navSpeed;
		currentState = PlayTimeState.MovePoints;
		SetRandomTarger();
		Sing_Game.This.studentContr.MY_PlayTimePlay(_isOn: false);
		Invoke("RelaxEnd", 30f);
		CancelInvoke("AudioRandom");
		InvokeRepeating("AudioRandom", 40f, Random.Range(35, 45));
		audioSource.clip = audioJumpEnd;
		audioSource.Play();
	}

	private void RelaxEnd()
	{
		isRelax = false;
		npcZone.SetActive(value: true);
	}

	private void SetRandomTarger()
	{
		Vector3 vector;
		do
		{
			vector = movingPoints[Random.Range(0, movingPointsCount)];
		}
		while (vector == aimPoint);
		aimPoint = vector;
		SetTarget(aimPoint);
	}

	private void SetTarget(Vector3 _position)
	{
		aimPoint = _position;
		if (navMeshAgent.enabled)
		{
			navMeshAgent.SetDestination(aimPoint);
		}
	}

	private IEnumerator MoveToTarget()
	{
		while (true)
		{
			SetTarget(studentTarger.position);
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void AudioRandom()
	{
		audioSource.clip = randomAudio;
		audioSource.Play();
	}
}
