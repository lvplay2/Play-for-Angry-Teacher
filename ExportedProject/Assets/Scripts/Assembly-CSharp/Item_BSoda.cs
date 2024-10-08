using UnityEngine;

public class Item_BSoda : MonoBehaviour
{
	public float shootTime;

	public float speed;

	private Rigidbody rbody;

	private Vector3 direction;

	private GameObject spraySprite;

	private void Awake()
	{
		rbody = GetComponent<Rigidbody>();
		spraySprite = base.transform.GetChild(0).gameObject;
	}

	private void Update()
	{
		rbody.velocity = direction;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Teacher")
		{
			spraySprite.SetActive(value: false);
			Sing_Game.This.canvasGame.MY_EnableSpray(isOn: true);
		}
	}

	public void MY_Shoot(Vector3 startPos, Vector3 _dir)
	{
		base.transform.position = startPos;
		base.transform.LookAt(Sing_Game.This.teacherContr.transform.position);
		base.gameObject.SetActive(value: true);
		direction = _dir * speed;
		base.enabled = true;
		Invoke("ShootOver", shootTime);
	}

	private void ShootOver()
	{
		Sing_Game.This.canvasGame.MY_EnableSpray(isOn: false);
		base.enabled = false;
		base.gameObject.SetActive(value: false);
		spraySprite.SetActive(value: true);
	}
}
