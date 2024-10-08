using UnityEngine;

public class Sing_Menu : MonoBehaviour
{
	public static Sing_Menu This { get; private set; }

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
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
