using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{

    public float realignmentSpeed = 0.02f;
    public float realignmentCooldown = 1f;
    public float restingPosition;
    public bool showMesh;

    // Start is called before the first frame update
    void Start()
    {
        restingPosition = transform.localPosition.x;
        OnValidate();
    }

    private void OnValidate()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = showMesh;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAlignment();
    }

    public void UpdateAlignment()
    {
        if (realignmentCooldown > 0)
        {
            realignmentCooldown -= Time.deltaTime;
            return;
        }
        else
        {
            realignmentCooldown = 0;
        }

        Vector3 pos = transform.localPosition;
        float distance = Vector3.Distance(pos, Vector3.zero);

        if (pos.x < (restingPosition - realignmentSpeed)) //if it has been pushed forwards...
        {
            pos.x += realignmentSpeed;
        }
        else if (pos.x > (restingPosition + realignmentSpeed)) //if it's towards the back...
        {
            pos.x -= realignmentSpeed;
        }
        else if (pos.x >= (restingPosition + realignmentSpeed) && pos.x <= (restingPosition - realignmentSpeed)) // if it's mostly at rest in the middle...
        {
            pos.x = restingPosition;
        }

        transform.localPosition = pos;

    }

    public void OnCollisionEnter(Collision collision)
    {
        realignmentCooldown = 1f;
    }
}
