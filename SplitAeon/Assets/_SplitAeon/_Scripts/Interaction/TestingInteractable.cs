
using UnityEngine;

public class TestingInteractable : Interactable
{
    public override string GetDescription()
    {
        return "Press <color=green>[F]</color> to rotate the object .";
    }

    public override void Interact()
    {
        this.transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }

}
