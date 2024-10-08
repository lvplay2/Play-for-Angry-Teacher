using UnityEngine;

public class Boost : MonoBehaviour
{
	public BoostType type;

	private void Awake()
	{
		type = (BoostType)Random.Range(0, 4);
	}
}
