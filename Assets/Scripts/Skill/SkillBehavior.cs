using UnityEngine;
using UnityEngine.Networking;
public interface SkillBehavior
{
	void ButtonDown();
	void ButtonUp();
	void ButtonDirection(float x, float y);
	void OnStartPlayer();
	void SetCooldown(GameObject gameObject);
	int GetCoolDownTime();
	Sprite GetButtonImage();
	NetworkIdentity GetNetworkIdentity();
}