using System;
using UnityEngine;
using UnityEngine.Networking;
[Serializable]
public class Skill : NetworkBehaviour, SkillBehavior
{
	public string skillName;
	public int cooldown;
	protected GameObject player;
	protected GameObject cooldownSkill;
	protected GameObject skillLine;
	protected Animator anim;
	protected float lastHorizontal;
	protected float lastVertical;

	public virtual void Start()
	{
		SetLocalPlayer();

		anim = player.GetComponent<Animator>();
		skillLine = FindObjectInPlayer("SkillLine");
	}

	public virtual void ButtonDown() { }
	public virtual void ButtonUp() { }
	public virtual void ButtonDirection(float vertical, float horizontal)
	{
		if (horizontal != 0 || vertical != 0)
		{
			lastHorizontal = horizontal;
			lastVertical = vertical;
		}
	}

	public NetworkIdentity networkIdentitiy
	{
		get { return GetComponent<NetworkIdentity>(); }
	}

	public NetworkIdentity GetNetworkIdentity()
	{
		return GetComponent<NetworkIdentity>();
	}

	protected void SpawnSkill(String nameObject, GameObject skillPrefab, GameObject player)
	{
		GameObject skillSpwanPosition = player.transform.FindChild(nameObject).gameObject;
		GameObject fireball = (GameObject)Instantiate(skillPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
		NetworkServer.Spawn(fireball);
	}

	protected GameObject FindObjectInPlayer(String name)
	{
		return player.transform.FindChild(name).gameObject;
	}

	private void SetLocalPlayer()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (var item in players)
		{
			if (item.GetComponent<NetworkIdentity>().hasAuthority)
			{
				player = item;
			}
		}
	}

	public void SetCooldown(GameObject coolDown)
	{
		cooldownSkill = coolDown;
	}
}