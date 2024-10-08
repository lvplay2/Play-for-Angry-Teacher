using UnityEngine;

public interface INpcZone
{
	void MY_ZoneEnter(Collider other);

	void MY_ZoneExit(Collider other);

	void MY_ZoneStay(Collider other);
}
