using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public Animator anim;

	public Transform nearPoint;

	public Transform farPoint;

	public void MY_CheckDistance()
	{
		if (Physics.Raycast(nearPoint.position, farPoint.position - nearPoint.position, out var hitInfo, 2f))
		{
			anim.SetFloat("Distance", Vector3.Distance(nearPoint.position, hitInfo.point) - 0.5f);
		}
		else
		{
			anim.SetFloat("Distance", 1.6f);
		}
	}
}
