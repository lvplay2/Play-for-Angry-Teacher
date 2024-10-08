using UnityEngine;

public class Door : MonoBehaviour
{
	public Sprite closeDoorImage;

	public Sprite openDoorImage;

	public AudioClip soundOpen;

	public AudioClip soundClose;

	private SpriteRenderer spriteRenderer;

	private AudioSource audioSource;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();
	}

	public void MY_OpenDoor(bool _isOn)
	{
		if (_isOn)
		{
			spriteRenderer.sprite = openDoorImage;
			audioSource.clip = soundOpen;
		}
		else
		{
			spriteRenderer.sprite = closeDoorImage;
			audioSource.clip = soundClose;
		}
		audioSource.Play();
	}
}
