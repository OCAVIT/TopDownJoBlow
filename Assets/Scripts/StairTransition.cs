using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public GameObject HighFloor;
    public GameObject DownFloor;
    public GameObject HighStairs;
    public GameObject DownStairs;

    private Vector3 highTriggerEntrySide;
    private Vector3 midTriggerEntrySide;
    private Vector3 downTriggerEntrySide;

    private bool highTriggerTouched = false;
    private bool midTriggerTouched = false;
    private bool downTriggerTouched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 entrySide = DetermineEntrySide(other.transform.position, transform.position);

            switch (gameObject.name)
            {
                case "High":
                    HandleHighTrigger(entrySide);
                    break;
                case "Mid":
                    HandleMidTrigger(entrySide);
                    break;
                case "Down":
                    HandleDownTrigger(entrySide);
                    break;
            }
        }
    }

    private Vector3 DetermineEntrySide(Vector3 playerPosition, Vector3 triggerPosition)
    {
        return (playerPosition - triggerPosition).normalized;
    }

    private void HandleHighTrigger(Vector3 entrySide)
    {
        if (!highTriggerTouched)
        {
            highTriggerEntrySide = entrySide;
            HighFloor.SetActive(false);
            highTriggerTouched = true;
        }
        else if (Vector3.Dot(highTriggerEntrySide, entrySide) < 0)
        {
            HighFloor.SetActive(true);
        }
    }

    private void HandleMidTrigger(Vector3 entrySide)
    {
        if (!midTriggerTouched)
        {
            midTriggerEntrySide = entrySide;
            DownFloor.SetActive(true);
            HighStairs.SetActive(false);
            midTriggerTouched = true;
        }
        else if (Vector3.Dot(midTriggerEntrySide, entrySide) < 0)
        {
            DownFloor.SetActive(false);
            HighStairs.SetActive(true);
        }
    }

    private void HandleDownTrigger(Vector3 entrySide)
    {
        if (!downTriggerTouched)
        {
            downTriggerEntrySide = entrySide;
            DownStairs.SetActive(false);
            downTriggerTouched = true;
        }
        else if (Vector3.Dot(downTriggerEntrySide, entrySide) < 0)
        {
            DownStairs.SetActive(true);
        }
    }
}