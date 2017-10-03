using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class oculusController : MonoBehaviour {
    bool yButtonPress = false;
    bool xButtonPress = false;
    bool aButtonPress = false;
    bool bButtonPress = false;
    bool startButtonPress = false;
    bool lStickButtonPress = false;
    bool rStickButtonPress = false;

    float lTrigger = 0.0f;
    float lGrip = 0.0f;
    float rTrigger = 0.0f;
    float rGrip = 0.0f;
    float lStickXPos = 0.0f;
    float lStickYPos = 0.0f;
    float rStickXPos = 0.0f;
    float rStickYPos = 0.0f;

    Vector2 lStickXYPos;
    Vector2 rStickXYPos;

    float hSpeed = 4.0f;
    float vSpeed = 8.0f;
    float walkSpeed = 0.1f;
    public OVRInput.Controller Controller;

    // Update is called once per frame
    void Update () {
        //OVRInput.Update();
        //if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick())
        //{
        //    Debug.Log("Moving Left.");
        //}
        //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        //{
        //    Debug.Log("Moving Right");
        //}
        //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        //{
        //    Debug.Log("Moving Up");
        //}
        //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        //{
        //    Debug.Log("Moving Down");
        //}

        //var rtouch = OVRInput.Controller.RTouch;
        //var ltouch = OVRInput.Controller.LTouch;

        //transform.localPosition = OVRInput.GetLocalControllerPosition(Controller);
        //transform.localRotation = OVRInput.GetLocalControllerRotation(Controller);

        //bButtonPress = OVRInput.Get(OVRInput.Button.Two, rtouch);
        //yButtonPress = OVRInput.Get(OVRInput.Button.Two, ltouch);
        //aButtonPress = OVRInput.Get(OVRInput.Button.One, rtouch);
        //xButtonPress = OVRInput.Get(OVRInput.Button.One, ltouch);
        //startButtonPress = OVRInput.Get(OVRInput.Button.Start, ltouch);

        //lStickButtonPress = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
        //rStickButtonPress = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);

        //lGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        //lTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        //rGrip = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        //rTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        
        OVRInput.Update();

        lStickXYPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        lStickXPos = lStickXYPos.x;
        lStickYPos = lStickXYPos.y;

        rStickXYPos = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        rStickXPos = rStickXYPos.x;
        rStickYPos = rStickXYPos.y;
        Debug.Log(lStickXYPos);
        Debug.Log(rStickXYPos);
        Vector3 direction = lStickXPos * Vector3.right + lStickYPos * Vector3.up;
        transform.position = transform.position + direction;
        //Touch Button State 
        //Debug.Log("Y Button State = " + yButtonPress);
        //Debug.Log("X Button State = " + xButtonPress);
        //Debug.Log("B Button State = " + bButtonPress);
        //Debug.Log("A Button State = " + aButtonPress);
        //Debug.Log("Start Button State = " + startButtonPress);
        //Debug.Log("LStick Button State = " + lStickButtonPress);
        //Debug.Log("RStick Button State = " + rStickButtonPress);
        ////Touch Trigger, Grip. 
        //Debug.Log("Left Trigger State = " + lTrigger);
        //Debug.Log("Left Grip State = " + lGrip);
        //Debug.Log("Right Trigger State = " + rTrigger);
        //Debug.Log("Right Grip State = " + rGrip);
        //Touch Analogue Sticks
        ////Debug.Log("Left Stick X Pos = " + lStickXPos);
        ////Debug.Log("Left Stick Y Pos = " + lStickYPos);

        ////Debug.Log("Right Stick X Pos = " + rStickXPos);
        ////Debug.Log("Right Stick Y Pos = " + rStickYPos);


        ////Still To Do 
        //Debug.Log("Left Touch Rest = " + OVRInput.Get(OVRInput.Touch.PrimaryThumbRest));
        //Debug.Log("Right Touch Rest = " + OVRInput.Get(OVRInput.Touch.SecondaryThumbRest));


    }
}






//using System.Collections;
//using System.Collections.Generic; using UnityEngine;
//public class TouchControllerTracking : MonoBehaviour
//{
//    //Public Buttons
//    public bool buttonOnePress = false;
//    public bool buttonTwoPress = false;
//    public bool buttonStartPress = false;
//    public bool buttonStickPress = false;

//    //Public Capacitive Touch
//    public bool thumbRest = false;
//    public bool buttonOneTouch = false;
//    public bool buttonTwoTouch = false;
//    public bool buttonThreeTouch = false;
//    public bool buttonFourTouch = false;
//    public bool buttonTrigger = false;
//    public bool buttonStick = false;

//    //Public Near Touch
//    public bool nearTouchIndexTrigger = false;
//    public bool nearTouchThumbButtons = false;

//    //Public Trigger & Grip
//    public float trigger = 0.0f;
//    public float grip = 0.0f;

//    //Public Stick Axis
//    Vector2 stickXYPos;
//    public float stickXPos = 0.0f;
//    public float stickYPos = 0.0f;

//    //Public Define Controller (Left/Right)
//    public OVRInput.Controller Controller;


//    // Update is called once per frame
//    void Update()
//    {

//        //Controller Position & Rotation
//        transform.localPosition = OVRInput.GetLocalControllerPosition(Controller);
//        transform.localRotation = OVRInput.GetLocalControllerRotation(Controller);

//        //Controller Button State
//        buttonOnePress = OVRInput.Get(OVRInput.Button.One, Controller);
//        buttonTwoPress = OVRInput.Get(OVRInput.Button.Two, Controller);
//        buttonStartPress = OVRInput.Get(OVRInput.Button.Start, Controller);
//        buttonStickPress = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, Controller);

//        //Controller Capacitive Sensors State
//        thumbRest = OVRInput.Get(OVRInput.Touch.PrimaryThumbRest, Controller);
//        buttonOneTouch = OVRInput.Get(OVRInput.Touch.One, Controller);
//        buttonTwoTouch = OVRInput.Get(OVRInput.Touch.Two, Controller);
//        buttonThreeTouch = OVRInput.Get(OVRInput.Touch.Three, Controller);
//        buttonFourTouch = OVRInput.Get(OVRInput.Touch.Four, Controller);
//        buttonTrigger = OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, Controller);
//        buttonStick = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, Controller);

//        //Controller NearTouch State
//        nearTouchIndexTrigger = OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, Controller);
//        nearTouchThumbButtons = OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, Controller);

//        //Controller Trigger State
//        grip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller);
//        trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller);

//        //Controller Analogue Stick State
//        stickXYPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, Controller);
//        stickXPos = stickXYPos.x;
//        stickYPos = stickXYPos.y;


//        //Output Logs
//        //  

//    }
//}
//}