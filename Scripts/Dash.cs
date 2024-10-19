using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Dash : MonoBehaviour
{
    
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovementAdvanced PMA;
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;
    public float dashCd;
    private float dashCdTimer;
    public AudioSource DashAudio;

    [Header("FOV effect")]
    public PlayerCam cam;
    public float dashFOV;

    public KeyCode dashKey = KeyCode.Q;

    private void Start()
    {      
        rb = GetComponent<Rigidbody>();
        PMA = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
            Dashin();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dashin()
    {
        if (dashCdTimer > 0) return; //add timer to dash to re-use again
        else dashCdTimer = dashCd;

        PMA.dashing = true;
        PMA.maxYSpeed = maxDashYSpeed;

        cam.DoFOV(dashFOV);
        DashAudio.Play();
        
        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam; /// where you're looking
        else
            forwardT = orientation; /// where you're facing (not up or down)

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        PMA.dashing = false;
        PMA.maxYSpeed = 0;
        cam.DoFOV(85f);
        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}

