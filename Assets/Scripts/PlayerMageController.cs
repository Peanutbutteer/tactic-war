using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.Networking;

public class PlayerMageController : NetworkBehaviour
{

    public float speed = 15f;
    new Rigidbody rigidbody;
    Vector3 movement;
    Animator anim;
    public GameObject mageBody;
    public GameObject staff;
    [SyncVar]
    public Color color;

    [SyncVar]
    public string playerName;

    [SyncVar]
    private int m_PlayerId;

    [Server]
    public void SetPlayerId(int playerId)
    {
        this.m_PlayerId = playerId;
    }

    public int playerId
    {
        get { return m_PlayerId; }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        float horizontal = CnInputManager.GetAxis("Horizontal");
        float vertical = CnInputManager.GetAxis("Vertical");

        if (anim.GetBool("Attack") == false && anim.GetBool("Blink") == false && anim.GetBool("AttackStaff") == false)
        {
            if (CnInputManager.GetButton("WalkButton"))
            {
                rigidbody.rotation = Util.Turning(horizontal, vertical);
            }
            Move(horizontal, vertical);
            Animating(horizontal, vertical);
        }


    }

    void Move(float x, float y)
    {
        movement.Set(x, 0, y);

        movement = movement.normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + movement);

    }


    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f && anim.GetBool("Attack") == false;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("Walk Forward", walking);
    }

    public override void OnStartClient()
    {
        int meshMage = 0;
        string texture = "";
        if (color == Color.blue)
        {
            meshMage = 0;
            texture = "MagesTexture";
        }
        else if (color == Color.magenta)
        {
            meshMage = 1;
            texture = "MagesTexture";
        }
        else if (color == Color.red)
        {
            meshMage = 2;
            texture = "MagesTexture";
        }
        else if (color == Color.yellow)
        {
            meshMage = 3;
            texture = "MagesTexture";
        }
        else if (color == Color.white)
        {
            meshMage = 0;
            texture = "MagesTexture2";
        }
        else if (color == Color.black)
        {
            meshMage = 3;
            texture = "MagesTexture2";
        }
        else if (color == Color.green)
        {
            meshMage = 2;
            texture = "MagesTexture2";
        }
        Mesh skinMage = Resources.LoadAll<Mesh>("Mages")[meshMage];
        if (skinMage != null)
        {
            mageBody.GetComponent<SkinnedMeshRenderer>().sharedMesh = skinMage;
        }
        Mesh skinStaff = Resources.LoadAll<Mesh>("Mages")[meshMage+5];
        if(skinStaff!=null)
        {
            staff.GetComponent<MeshFilter>().mesh = skinStaff;
        }
        Texture text = Resources.Load(texture) as Texture;
        if (text != null)
        {
            mageBody.GetComponent<Renderer>().material.mainTexture = text;
            staff.GetComponent<Renderer>().material.mainTexture = text;
        }
    }
}
