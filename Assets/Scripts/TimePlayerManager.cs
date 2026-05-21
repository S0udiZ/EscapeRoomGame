using System.Collections;
using UnityEngine;

public class TimePlayerManager : MonoBehaviour
{
    //The hand that gets checked first
    int handprioty = 0;
    Player player;
    Controller rhand, lhand;

    Controller[] hands;

    [SerializeField]
    float ActivateDistance;

    [SerializeField]
    TimeManager timeManager;

    bool timeActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
        rhand = player.r_hand;
        lhand = player.l_hand;
        hands = new Controller[2];
        hands[0] = rhand;
        hands[1] = lhand;
    }

    // Update is called once per frame
    void Update()
    {
        if (handprioty == 0)
        {
            if (IsHandTimeThing(rhand))
            {
                ActivateTimeThing();
                return;
            }
            else if (IsHandTimeThing(lhand))
            {
                timeActivated = false;
                handprioty = 1;
                ActivateTimeThing();
                return;
            }
        }
        else
        {
            if (IsHandTimeThing(lhand))
            {
                ActivateTimeThing();
                return;
            }
            else if (IsHandTimeThing(rhand))
            {
                timeActivated = false;
                handprioty = 0;
                ActivateTimeThing();
                return;
            }
        }

        if (timeActivated)
        {
            timeActivated = false;
            print("Neigh");
        }

    }

    void ActivateTimeThing()
    {
        if (timeActivated == false)
        {
            timeActivated = true;
            print("Yay");
            StopAllCoroutines();
            StartCoroutine(TimeControl());

        }
    }

    bool IsHandTimeThing(Controller hand)
    {
        if (hand.grabbing && hand.trigger)
        {
            return true;
        }
        return false;
    }

    Vector3 startPos;

    IEnumerator TimeControl()
    {
        Controller curHand = hands[handprioty];
        GameObject handObj = curHand.controller;
        startPos = handObj.transform.position;

        bool timeReversed = false;

        float distance;

        while (timeActivated)
        {
            yield return null;

            Debug.DrawLine(startPos, handObj.transform.position, Color.green);

            distance = Vector3.Distance(startPos, handObj.transform.position);
            print($"Distance: {distance} | {Time.time}");

            if (!timeReversed && distance > ActivateDistance)
            {
                timeReversed = true;
                timeManager.StartTimeReverse();
            }
            else if (timeReversed && distance < ActivateDistance)
            {
                timeReversed = false;
                timeManager.StopTimeReverse();
            }

        }


    }
}
