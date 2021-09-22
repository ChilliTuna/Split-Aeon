using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{


    public LayerMask mask;

    public List<ImpactSurface> surfaces = new List<ImpactSurface>();

    public GameObject fallbackImpact;

    private void OnEnable()
    {
        if (Physics.CheckSphere(gameObject.transform.position, 0.2f, mask))
        {
            GameObject hitObject = Physics.OverlapSphere(gameObject.transform.position, 0.2f, mask)[0].gameObject;

            foreach (ImpactSurface s in surfaces)
            {
                if (hitObject.transform.tag == s.name)
                {
                    //Debug.LogWarning("Impact created on type " + s.name);

                    s.impactEffect.SetActive(true);

                    return;
                }

            }

            fallbackImpact.SetActive(true);
        }
    }

    private void OnDisable()
    {

        transform.parent = null;

        foreach (ImpactSurface s in surfaces)
        {
            s.impactEffect.SetActive(false);
        }
    }

}

[System.Serializable]
public class ImpactSurface
{
    public string name;
    public GameObject impactEffect;
}
