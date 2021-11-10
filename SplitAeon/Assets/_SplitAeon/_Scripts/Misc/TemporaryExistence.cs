using System.Collections;
using UnityEngine;

public class TemporaryExistence : MonoBehaviour
{
    public IEnumerator LiveThenDieScaled(float period)
    {
        yield return new WaitForSeconds(period);
        Destroy(gameObject);
    }

    //private IEnumerator LiveThenDieRealtime(float period)
    //{
    //    yield return new WaitForSecondsRealtime(period);
    //}
}