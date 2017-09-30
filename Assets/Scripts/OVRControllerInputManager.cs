using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRControllerInputManager : MonoBehaviour
{
    public GameObject TeleportMarker;
    public GameObject World;
    public Transform Player;
    public State handHoldingState;
    public OVRInput.Controller hand;
    private float RayLength = 0.3f;
    private GameObject currentObj;
    private GameObject ball;
    public enum State
    {
        EMPTY,
        TOUCHING,
        HOLDING
    };

    void Start()
    {
        OVRTouchpad.Create();
        OVRTouchpad.TouchHandler += TouchpadHandler;
        this.handHoldingState = State.EMPTY;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, RayLength) && this.handHoldingState == State.EMPTY)
        {
            if (hit.collider.tag == "Throwable")
            {
                if (OVRInput.Get (OVRInput.Axis1D.PrimaryHandTrigger, hand) >= 0.5f && this.handHoldingState == State.EMPTY) {
                    currentObj = hit.collider.gameObject;
                    currentObj.transform.position = this.transform.position;
                    Rigidbody rigidbody = currentObj.GetComponent<Rigidbody>();
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    rigidbody.useGravity = false;
                  
                    hit.transform.parent = transform;
                    MeshRenderer m = currentObj.GetComponent<MeshRenderer>();
                    Material curM = m.material;
                    curM.SetColor("_Color", Color.red);
                    this.handHoldingState = State.HOLDING;
                }
            } else if (hit.collider.tag == "metal" || hit.collider.tag == "trampoline" || hit.collider.tag == "wood" || hit.collider.tag == "teleporter" || hit.collider.tag == "teleport_target")
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, hand) >= 0.5f && this.handHoldingState == State.EMPTY)
                {
                    currentObj = hit.collider.gameObject;
                    Rigidbody rigidbody = currentObj.GetComponent<Rigidbody>();
                    hit.transform.parent = transform;
                    this.handHoldingState = State.HOLDING;
                }
            }
        }

        if (this.handHoldingState == State.HOLDING) {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, hand) <= 0.5f)
            {
                Rigidbody rigidbody = currentObj.GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.velocity = OVRInput.GetLocalControllerVelocity(hand);
                currentObj.transform.parent = null;
                if (currentObj.tag == "Throwable") {
                    MeshRenderer m = currentObj.GetComponent<MeshRenderer>();
                    Material curM = m.material;
                    curM.SetColor("_Color", Color.blue);
                }
                currentObj = null;
                this.handHoldingState = State.EMPTY;
            }
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, hand))
        {
            RayLength = 10f;
            if (Physics.Raycast(ray, out hit, RayLength))
            {
                if (hit.collider.tag == "ground")
                {
                    if (!TeleportMarker.activeSelf)
                    {
                        RayLength = 0.2f;
                        TeleportMarker.SetActive(true);
                    }
                    TeleportMarker.transform.position = hit.point;
                    if (OVRInput.GetDown(OVRInput.Button.One, hand))
                    {
                        teleportOnButtonPress();
                    }
                }
                else
                {
                    RayLength = 0.2f;
                    TeleportMarker.SetActive(false);
                }
            }
            else
            {
                RayLength = 0.2f;
                TeleportMarker.SetActive(false);
            }
        }
        else {
            RayLength = 0.2f;
            TeleportMarker.SetActive(false);
        }
        
    }

    void teleportOnButtonPress()
    {
        if (TeleportMarker.activeSelf)
        {
            Vector3 markerPosition = TeleportMarker.transform.position;
            Player.position = new Vector3(markerPosition.x, Player.position.y,
                markerPosition.z);
        }
    }

    void TouchpadHandler(object sender, System.EventArgs e)
    {

        OVRTouchpad.TouchArgs args = (OVRTouchpad.TouchArgs)e;
        if (args.TouchType == OVRTouchpad.TouchEvent.SingleTap)
        {
            // Need to check where the user is looking to teleport him
            if (TeleportMarker.activeSelf)
            {
                Vector3 markerPosition = TeleportMarker.transform.position;
                Player.position = new Vector3(markerPosition.x, Player.position.y,
                    markerPosition.z);
            }
        }
    }
}

