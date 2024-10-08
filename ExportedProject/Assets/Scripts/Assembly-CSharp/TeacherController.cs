using UnityEngine;
using UnityEngine.AI;

public class TeacherController : MonoBehaviour
{
	public Transform[] obstacleTFrom;

	public NavMeshObstacle[] obstacle;

	public float moveSpeed;

	public float rotateSpeed;

	public Animator ruleAnim;

	public AudioSource clapAudio;

	private GameObject gobj;

	private Rigidbody rbodi;

	private bool isFirstObstacle;

	private bool isMop;

	public float currentSpeed;

	public bool isFreeze;

	public bool isSlow;

	private void Start()
	{
		gobj = GetComponent<GameObject>();
		rbodi = GetComponent<Rigidbody>();
		currentSpeed = moveSpeed;
		UpdateRotateSpeed();
	}

	public void UpdateRotateSpeed()
	{
		rotateSpeed = PlayerPrefs.GetFloat("Sensitivity", 70f);
	}

	private void OnCollisionEnter(Collision other)
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Door" && (bool)other.GetComponent<Door>())
		{
			other.GetComponent<Door>().MY_OpenDoor(_isOn: true);
		}
		else if (other.tag == "Boost")
		{
			switch (other.GetComponent<Boost>().type)
			{
			case BoostType.FreezeS:
				Sing_Game.This.canvasGame.boostText.text = GameplayManager.This.studentFreeze.ToString();
				Sing_Game.This.studentContr.SetFreeze();
				break;
			case BoostType.FreezeT:
				Sing_Game.This.canvasGame.boostText.text = GameplayManager.This.teacherFreeze.ToString();
				isFreeze = true;
				UpdateSpeed();
				Invoke("UnFreeze", GameplayManager.This.boostFreezeTime);
				break;
			case BoostType.SlowS:
				Sing_Game.This.canvasGame.boostText.text = GameplayManager.This.studentSlow.ToString();
				Sing_Game.This.studentContr.SetSlow();
				break;
			case BoostType.SlowT:
				Sing_Game.This.canvasGame.boostText.text = GameplayManager.This.teacherSlow.ToString();
				isSlow = true;
				UpdateSpeed();
				Invoke("UnSlow", GameplayManager.This.boostSlowTime);
				break;
			}
			Sing_Game.This.canvasGame.boostScreen.SetActive(value: true);
			other.GetComponent<Boost>().gameObject.SetActive(value: false);
			Invoke("HideBoostScreen", 3f);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Door" && (bool)other.GetComponent<Door>())
		{
			other.GetComponent<Door>().MY_OpenDoor(_isOn: false);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			isFreeze = !isFreeze;
			UpdateSpeed();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			isSlow = !isSlow;
			UpdateSpeed();
		}
	}

	public void MY_SetMove()
	{
		rbodi.velocity = base.transform.forward * currentSpeed;
		EnableRule(_isOn: true);
		clapAudio.Play();
	}

	public void MY_SetStop()
	{
		rbodi.velocity = Vector3.zero;
		MY_UpdateObstaclePosition();
		EnableRule(_isOn: false);
	}

	public void MY_AddRotation(float _angle)
	{
		base.transform.Rotate(0f, _angle * rotateSpeed, 0f);
	}

	public void MY_StudentVisionEnable(bool isOn)
	{
		obstacle[0].enabled = isOn;
		obstacle[1].enabled = isOn;
	}

	public void MY_UpdateObstaclePosition()
	{
		obstacleTFrom[(!isFirstObstacle) ? 1u : 0u].position = base.transform.position;
		isFirstObstacle = !isFirstObstacle;
	}

	public void MY_MopEnter(bool _isOn, Transform _parent = null)
	{
		isMop = _isOn;
		UpdateSpeed();
		base.transform.parent = _parent;
	}

	public void MY_Lose()
	{
		currentSpeed = 0f;
	}

	public void UA_ReturnRotation()
	{
		base.transform.Rotate(0f, 180f, 0f);
	}

	private void EnableRule(bool _isOn)
	{
		ruleAnim.SetBool("isOn", _isOn);
	}

	private void UpdateSpeed()
	{
		if (isFreeze)
		{
			currentSpeed = 0f;
			return;
		}
		float num = moveSpeed;
		if (isSlow)
		{
			num -= GameplayManager.This.boostSlowSpeedTeacher;
		}
		if (isMop)
		{
			currentSpeed = num / 2f;
		}
		else
		{
			currentSpeed = num;
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

	private void HideBoostScreen()
	{
		Sing_Game.This.canvasGame.boostScreen.SetActive(value: false);
	}
}
