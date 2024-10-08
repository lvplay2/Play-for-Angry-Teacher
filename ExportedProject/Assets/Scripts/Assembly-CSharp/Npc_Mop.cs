using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc_Mop : MonoBehaviour, INpcEvent, INpcZone
{
	private enum MopState
	{
		None = 0,
		MovePoints = 1,
		MoveHome = 2
	}

	public Transform tformParent;

	public NavMeshAgent navMeshAgent;

	public Transform movingPointsParent;

	public Transform homePosition;

	public AudioSource audioSource;

	private List<Vector3> movingPoints = new List<Vector3>();

	private int movingPointsCount;

	private Vector3 aimPoint;

	private MopState currentState;

	private int pointsLeft;

	private Vector2Int minMaxTimer;

	private void Start()
	{
		foreach (Transform item in movingPointsParent)
		{
			movingPoints.Add(item.position);
		}
		movingPointsCount = movingPoints.Count;
		minMaxTimer = GameplayManager.This.mopMinMaxTimer[MultiSceneManager.This.GetDifficulty()];
		Invoke("MY_EnableMop", Random.Range(minMaxTimer.x, minMaxTimer.y));
	}

	public void MY_EnableMop()
	{
		audioSource.enabled = true;
		pointsLeft = 3;
		currentState = MopState.MovePoints;
		SetRandomTarger();
	}

	public void MY_MarkerEnter(Vector3 _pos)
	{
		if (!(aimPoint == _pos))
		{
			return;
		}
		if (currentState == MopState.MovePoints)
		{
			if (pointsLeft > 0)
			{
				pointsLeft--;
				SetRandomTarger();
			}
			else
			{
				currentState = MopState.MoveHome;
				SetTarget(homePosition.position);
			}
		}
		else if (currentState == MopState.MoveHome)
		{
			audioSource.enabled = false;
			Invoke("MY_EnableMop", Random.Range(minMaxTimer.x, minMaxTimer.y));
		}
	}

	public void MY_ZoneEnter(Collider other)
	{
		if (other.tag == "Teacher")
		{
			Sing_Game.This.teacherContr.MY_MopEnter(_isOn: true, tformParent);
		}
		else if (other.tag == "Student")
		{
			Sing_Game.This.studentContr.MY_MopEnter(_isOn: true, tformParent);
		}
		else if (other.tag == "PlayTime")
		{
			Sing_Game.This.playTime.MY_MopEnter(_isOn: true, tformParent);
		}
	}

	public void MY_ZoneStay(Collider other)
	{
		if (other.tag == "Teacher")
		{
			Sing_Game.This.cameraMove.MY_CheckDistance();
		}
	}

	public void MY_ZoneExit(Collider other)
	{
		if (other.tag == "Teacher")
		{
			Sing_Game.This.teacherContr.MY_MopEnter(_isOn: false);
		}
		else if (other.tag == "Student")
		{
			Sing_Game.This.studentContr.MY_MopEnter(_isOn: false);
		}
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
}
