using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSController : MonoBehaviour
{
    public GameObject EntitiesFolder;
    public Entity[] Entities;
    public Transform MouseView;
    public Transform MoveToView;

    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        Entities = EntitiesFolder.GetComponentsInChildren<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!MouseView) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit raycastHit)) return;
        targetPosition = raycastHit.point;
        MouseView.transform.position = new Vector3(targetPosition.x, 0, targetPosition.z);

        if (Input.GetMouseButtonUp(1))
        {
            //foreach entity entity[i].bounds/2 + entity[i + 1].bound/2
            List<Vector3> targetPositionList = GetPositionListAround(targetPosition, new float[] { 1f, 2f, 3f }, new int[] { 5, 10, 20 });

            int targetPositionListIndex = 0;

            foreach (Entity entity in Entities)
            {
                entity.SetMove(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    //STROKED WALL FORMATION
    List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            dir.z = dir.y;
            dir.y = 0;
            Vector3 position = startPosition + dir * distance;
            // position.z = position.y;
            // position.y = 0;
            positionList.Add(position);
        }
        return positionList;
    }

    Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

}