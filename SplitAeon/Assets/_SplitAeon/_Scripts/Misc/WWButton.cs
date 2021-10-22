using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WWButton : MonoBehaviour
{

    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;

    private bool selected = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (selected)
        {
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
    }

    public void Deselected()
    {
        selected = false;
    }

    public void HoverEnter()
    {
        if (GetComponent<Button>().interactable)
        {
            anim.SetBool("Hover", true);
            itemText.text = itemName;
        }
    }

    public void HoverExit()
    {
        if (GetComponent<Button>().interactable)
        {
            anim.SetBool("Hover", false);
            itemText.text = "";
        }
    }

}
