using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ncp_Director : MonoBehaviour, INpcEvent
{
	public NavMeshAgent navMeshAgent;

	public Transform movingPointsParent;

	public float navSpeed = 3f;

	private List<Vector3> movingPoints = new List<Vector3>();

	private int movingPointsCount;

	private Vector3 aimPoint;

	private void Start()
	{
		foreach (Transform item in movingPointsParent)
		{
			movingPoints.Add(item.position);
		}
		movingPointsCount = movingPoints.Count;
		navMeshAgent.speed = navSpeed;
		SetRandomTarger();
	}

	public void MY_MarkerEnter(Vector3 _pos)
	{
		if (aimPoint == _pos)
		{
			SetRandomTarger();
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
