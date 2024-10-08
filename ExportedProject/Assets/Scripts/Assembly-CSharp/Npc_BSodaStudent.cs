using UnityEngine;

public class Npc_BSodaStudent : MonoBehaviour, INpcZone
{
	public Item_BSoda bSoda;

	public int shootRandomPersent;

	private bool isShoot;

	private void Start()
	{
	}

	public void MY_Enable(Vector3 _pos)
	{
		base.transform.position = _pos;
		base.gameObject.SetActive(value: true);
	}

	public void MY_Disable()
	{
		CancelInvoke("MY_Disable");
		base.gameObject.SetActive(value: false);
	}

	public void MY_ZoneEnter(Collider other)
	{
		if (other.tag == "Teacher")
		{
			isShoot = Random.Range(0, 100) < shootRandomPersent;
		}
	}

	public void MY_ZoneExit(Collider other)
	{
	}

	public void MY_ZoneStay(Collider other)
	{
		if (other.tag == "Teacher" && isShoot)
		{
			Vector3 direction = other.transform.position - base.transform.position;
			if (Physics.Raycast(base.transform.position, direction, out var hitInfo, 15f) && hitInfo.transform.tag == "Teacher")
			{
				Debug.DrawLine(base.transform.position - base.transform.right, hitInfo.point, Color.green);
				isShoot = false;
				Vector3 dir = other.transform.position - base.transform.position;
				dir.Normalize();
				bSoda.MY_Shoot(base.transform.position, dir);
				Invoke("MY_Disable", 3f);
			}
		}
	}
}
