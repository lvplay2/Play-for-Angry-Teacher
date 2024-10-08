using UnityEngine;

public class ExitWall : MonoBehaviour
{
	public MeshRenderer wallRenderer;

	public BoxCollider wallCollider;

	public void EnableWall(bool _isOn)
	{
		wallRenderer.enabled = _isOn;
		wallCollider.enabled = _isOn;
	}
}
