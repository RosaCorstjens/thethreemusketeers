using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    protected Animator anim;
    protected Rigidbody rb;
    protected Transform myTransform;

    protected const float WALK_SPEED = .25f;
    protected const float DIRECTION_SPEED = 3.0f;

    protected float inputDelay = 0.1f;

    protected float forwardSpeed, turnSpeed, strafeSpeed;

    protected bool isWalking = false;

    protected void Start()
    {
        myTransform = transform;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        forwardSpeed = turnSpeed = strafeSpeed = 0;
    }

    protected void Update()
    {
        Move();
        Turn();
        Strafe();
    }

    public void MoveMeForward(float forwardSpeed) { this.forwardSpeed = forwardSpeed; }
    public void TurnMe(float turnSpeed) { this.turnSpeed = turnSpeed; }
    public void MoveMeSideways(float strafeSpeed) { this.strafeSpeed = strafeSpeed; }
    public void ToggleRun() { isWalking = !isWalking; }

    protected void Move()
    {
        if (isWalking)
        {
            anim.SetFloat("MoveZ", Mathf.Clamp(forwardSpeed, -.5f, .5f));
        }
        else
        {
            anim.SetFloat("MoveZ", forwardSpeed);
        }
    }

    protected void Turn()
    {
        //anim.SetFloat("Turn", turnSpeed / 1.2f);//, inputDelay, Time.deltaTime);
    }

    protected void Strafe()
    {      
        if (isWalking)
        {
            anim.SetFloat("MoveX", Mathf.Clamp(strafeSpeed, -.25f, .25f));
        }
        else
        {
            anim.SetFloat("MoveX", strafeSpeed);
        }
    }

    protected void BasicAttack()
    {
        anim.SetTrigger("Attack01");
    }
}
