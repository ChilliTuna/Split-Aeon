using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour
{
    public Transform doorTransform;

    public float openDegree = 90.0f;
    public float openTime = 1.0f;

    float openProgress = 1.0f;
    float timeToAction = 0.0f;
    Vector3 rotStart = Vector3.zero;
    Vector3 targetRot = Vector3.zero;

    bool isClosed = true;

    delegate void VoidAction();
    VoidAction actionDelegate;

    public AnimationCurve openCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve closeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    AnimationCurve actionCurve;

    // Start is called before the first frame update
    void Start()
    {
        actionDelegate = () => { };
    }

    // Update is called once per frame
    void Update()
    {        
        actionDelegate.Invoke();
    }

    public void Open()
    {
        if(!isClosed)
        {
            return;
        }
        isClosed = false;

        rotStart = doorTransform.localEulerAngles;
        timeToAction = openTime * openProgress;
        targetRot = Vector3.up * openDegree;
        openProgress = 0.0f;
        actionCurve = openCurve;
        actionDelegate = OpenAction;
    }

    void OpenAction()
    {
        openProgress += Time.deltaTime / timeToAction;
        doorTransform.localEulerAngles = Vector3.Slerp(rotStart, targetRot, actionCurve.Evaluate(openProgress));

        if(openProgress > 1.0f)
        {
            openProgress = 1.0f;
            actionDelegate = () => { };
        }
        else if(openProgress < 0.0f)
        {
            openProgress = 0.0f;
            actionDelegate = () => { };
        }
    }

    public void Close()
    {
        if (isClosed)
        {
            return;
        }
        isClosed = true;

        rotStart = doorTransform.localEulerAngles;
        if(rotStart.y == 0.0f)
        {
            return;
        }
        timeToAction = openTime * openProgress;
        targetRot = Vector3.zero;
        openProgress = 0.0f;
        actionCurve = closeCurve;
        actionDelegate = OpenAction;
    }

    void CloseAction()
    {

    }
}
