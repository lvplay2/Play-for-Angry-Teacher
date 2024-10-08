using UnityEngine;

public class DefineSwitcher : MonoBehaviour
{
	public DefineVariable[] variables;

	private void Awake()
	{
		DefineVariable[] array = variables;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == DefineVariable.PC)
			{
				return;
			}
		}
		base.gameObject.SetActive(value: false);
	}
}
