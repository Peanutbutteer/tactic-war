using System;
using UnityEngine;
using UnityEngine.Networking;
[Serializable]
public class Skill : NetworkBehaviour, SkillBehavior
{
	public string skillName;
    protected GameObject player;
    public NetworkIdentity networkIdentitiy
	{
		get { return GetComponent<NetworkIdentity>(); }
	}
	public virtual void Start()
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
    
	public virtual void ButtonDown()
	{

	}

    public virtual void ButtonUp()
    {

    }

    public virtual void ButtonHold()
    {

    }
    public virtual void ButtonDirection(float vertical, float horizontal)
	{

	}

	public NetworkIdentity GetNetworkIdentity()
	{
		return GetComponent<NetworkIdentity>();
	}


}