using System.Collections;
using UnityEngine;

public class Sing_Game : MonoBehaviour
{
	public Story_Game storyGame;

	public TeacherController teacherContr;

	public float teacherMoveTime;

	public Transform directionArrow;

	public CameraMove cameraMove;

	public Npc_BSodaStudent npcBSoda;

	public StudentAI studentContr;

	public int visionDistance;

	public Canvas_Game canvasGame;

	public TouchPanel touchPanel;

	public JoystickX joystickX;

	public Rule rule;

	public Door[] startDoors;

	public Npc_PlayTime playTime;

	public Material studentMat;

	[HideInInspector]
	public int bookCount;

	private Transform teacherTform;

	private Transform studentTform;

	private float teacherCurrentMoveDelay;

	private float[] baldiMinSpeed = new float[7] { 2.2f, 1.8f, 1.5f, 1.2f, 1.13f, 1f, 0.9f };

	private IGetInput getInput;

	private bool autoClick = true;

	private bool isPause;

	public static Sing_Game This { get; private set; }

	private void Awake()
	{
		if (This != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		This = this;
		if (!GameObject.Find("MultiSceneManager"))
		{
			Object.Instantiate(Resources.Load("Prefabs/MultiSceneManager") as GameObject).name = "MultiSceneManager";
		}
	}

	private void Start()
	{
		switch (MultiSceneManager.This.GetDifficulty())
		{
		case 0:
			baldiMinSpeed = GameplayManager.This.teacherSpeedEasy;
			break;
		case 1:
			baldiMinSpeed = GameplayManager.This.teacherSpeedNormal;
			break;
		case 2:
			baldiMinSpeed = GameplayManager.This.teacherSpeedHard;
			break;
		}
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		teacherTform = teacherContr.transform;
		studentTform = studentContr.transform;
		teacherCurrentMoveDelay = baldiMinSpeed[0];
		StartCoroutine("TeacherMove");
		BlockStartDoors(_isBlock: true);
	}

	private void Update()
	{
		float axis = Input.GetAxis("Mouse X");
		if (axis != 0f)
		{
			teacherContr.MY_AddRotation(Time.deltaTime * axis);
			cameraMove.MY_CheckDistance();
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			This.rule.UA_StepClick();
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			This.teacherContr.UA_ReturnRotation();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			autoClick = !autoClick;
			This.rule.UA_AutoToggleSwitch(autoClick);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPause = !isPause;
			This.canvasGame.UA_OpenPause(isPause);
		}
		float magnitude = (studentTform.position - teacherTform.position).magnitude;
		Vector3 vector = teacherTform.InverseTransformPoint(studentTform.position);
		float y = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
		studentMat.SetFloat("_Angle", studentContr.transform.localEulerAngles.y);
		directionArrow.localEulerAngles = new Vector3(90f, y, 0f);
		canvasGame.MY_UpdateDistance(magnitude);
		if (magnitude < (float)visionDistance)
		{
			teacherContr.MY_StudentVisionEnable(isOn: true);
			studentContr.MY_SetDangerous(_isDang: true);
		}
		else if (magnitude > (float)(visionDistance + 10))
		{
			teacherContr.MY_StudentVisionEnable(isOn: false);
			studentContr.MY_SetDangerous(_isDang: false);
		}
	}

	public static float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
	}

	public void MY_NextBook()
	{
		teacherCurrentMoveDelay = baldiMinSpeed[bookCount];
		rule.MY_ChangeSpeed(1f / teacherCurrentMoveDelay);
		bookCount++;
		storyGame.MY_GetBook();
		canvasGame.BookCount = bookCount;
		if (bookCount == 2)
		{
			BlockStartDoors(_isBlock: false);
		}
		StopCoroutine("TeacherMove");
		StartCoroutine("TeacherMove");
	}

	public void MY_Win()
	{
		PlayerPrefs.SetInt("Game Win", 1);
		canvasGame.MY_SaveTimer();
		MultiSceneManager.This.LoadScene("Menu");
	}

	public void MY_Lose()
	{
		teacherContr.MY_Lose();
		canvasGame.MY_Lose();
		Invoke("MenuLose", 3f);
	}

	public void MY_GetBook()
	{
	}

	private IEnumerator TeacherMove()
	{
		rule.MY_DisableRule();
		rule.MY_ChangeSpeed(1f / teacherCurrentMoveDelay);
		teacherContr.MY_SetStop();
		yield return new WaitForSeconds(1f);
		rule.MY_EnableRule();
	}

	private void BlockStartDoors(bool _isBlock)
	{
		Door[] array = startDoors;
		for (int i = 0; i < array.Length; i++)
		{
			_ = array[i];
		}
	}

	private void MenuLose()
	{
		MultiSceneManager.This.LoadScene("Menu");
	}
}
