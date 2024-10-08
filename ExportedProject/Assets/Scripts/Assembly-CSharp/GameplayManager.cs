using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	public float roomEnterMinDistance;

	public float[] studentSpeed;

	public float[] studentStaminaSpeed;

	public float[] teacherSpeedEasy;

	public float[] teacherSpeedNormal;

	public float[] teacherSpeedHard;

	public Vector2Int[] mopMinMaxTimer;

	public float boostSlowTime = 5f;

	public float boostSlowSpeedTeacher = 20f;

	public float boostSlowSpeedStudent = 1f;

	public float boostFreezeTime = 5f;

	public string teacherFreeze = "You is freezed";

	public string teacherSlow = "You is slowed";

	public string studentFreeze = "Student is freezed";

	public string studentSlow = "Student is slowed";

	public static GameplayManager This { get; private set; }

	private void Awake()
	{
		This = this;
	}
}
