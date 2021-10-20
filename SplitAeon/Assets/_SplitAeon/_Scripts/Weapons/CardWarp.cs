using UnityEngine;

public class CardWarp : MonoBehaviour
{
    private bool thisCardInPast = true;
    private bool timePlayThisCard = false;
    private bool hasWarped = false;

    public int damage;

    private Vector3 velocity;

    private Timewarp tw;

    private float timeUntilWarp = 0.1f;

    private CustomTimer timer;

    // Start is called before the first frame update
    private void OnEnable()
    {
        timer = new CustomTimer();
        timer.Start();
        velocity = new Vector3();
        tw = FindObjectOfType<Timewarp>();
        thisCardInPast = Globals.isInPast;
    }

    // Update is called once per frame
    private void Update()
    {
        if (hasWarped)
        {
            if (Globals.isInPast & thisCardInPast)
            {
                ResumeCard();
            }
        }
        else
        {
            timer.Count();
            if (timer.GetCurrentTime() >= timeUntilWarp)
            {
                WarpToOtherTime();
            }
        }
    }

    #region Generic card stuff

    private void OnCollisionEnter(Collision collision)
    {
        var health = collision.collider.gameObject.GetComponentInParent<Health>();
        if (health)
        {
            StickToSurface(collision);

            health.Hit(damage, collision.collider);

            GetComponent<BoxCollider>().enabled = false;
        }
        else if (collision.collider.gameObject.GetComponent<Target>())
        {
            StickToSurface(collision);

            collision.collider.gameObject.GetComponent<Target>().Hit();

            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            StickToSurface(collision);
        }
    }

    private void StickToSurface(Collision collision)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        transform.parent = collision.transform;

        transform.position = collision.GetContact(0).point;
    }

    #endregion Generic card stuff

    public void WarpToOtherTime()
    {
        thisCardInPast = !timePlayThisCard;
        velocity = gameObject.GetComponent<Rigidbody>().velocity;
        Vector3 temp = transform.position;
        temp.y -= tw.offsetAmount;
        transform.position = temp;
        hasWarped = true;
    }

    public void ResumeCard()
    {
        timePlayThisCard = true;
        gameObject.GetComponent<Rigidbody>().velocity = velocity;
    }
}