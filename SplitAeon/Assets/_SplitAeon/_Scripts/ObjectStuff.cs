using UnityEngine;

public class ObjectStuff : MonoBehaviour
{
    public void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

    public void ActivateGameObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void TurnOnWatch(Timewarp tw)
    {
        tw.enabled = true;
    }


}