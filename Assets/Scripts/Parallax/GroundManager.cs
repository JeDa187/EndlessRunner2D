using UnityEngine;

public class GroundManager : MonoBehaviour
{
    private GameObject[] grounds;

    private void Awake()
    {
        grounds = GameObject.FindGameObjectsWithTag("Ground_Second");
    }


    public GameObject GetRightmostGroundSecond()
    {
        GameObject rightmostGround = null;
        float maxX = float.NegativeInfinity;

        foreach (GameObject ground in grounds)
        {
            if (ground.transform.position.x > maxX)
            {
                maxX = ground.transform.position.x;
                rightmostGround = ground;
            }
        }

        return rightmostGround;
    }
}
