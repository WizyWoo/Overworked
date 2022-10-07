using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingConveyorHazard : MonoBehaviour
{

    public float SpawnTimeMin, SpawnTimeMax, ConveyorSpeed, DistFromPoint;
    public GameObject ConveyorArmPrefab;
    public Transform[] RailPoints;
    public List<Transform> Arms;
    public List<int> CurPoint;
    public List<float> ArmTravelTimer, CurDist;

    //spawn arm, make it follow rail, test if knock off player

    private void Start()
    {

        Arms = new List<Transform>();
        CurPoint = new List<int>();
        ArmTravelTimer = new List<float>();
        CurDist = new List<float>();
        Invoke(nameof(SpawnArm), 1);

    }

    private void Update()
    {

        if(Arms.Count > 0)
        {

            for(int i = 0; i < Arms.Count; i++)
            {

                ArmTravelTimer[i] += Time.deltaTime * ConveyorSpeed;

                if(Vector3.Distance(Arms[i].position, RailPoints[CurPoint[i]].position) < DistFromPoint)
                {

                    CurPoint[i]++;
                    if(CurPoint[i] == RailPoints.Length)
                    {

                        RemoveArm(i);

                    }
                    else
                    {

                        CurDist[i] = Vector3.Distance(RailPoints[CurPoint[i] - 1].position, RailPoints[CurPoint[i]].position);
                        ArmTravelTimer[i] = 0;

                    }

                }
                else
                {

                    Arms[i].position = Vector3.Lerp(RailPoints[CurPoint[i]-1].position, RailPoints[CurPoint[i]].position, ArmTravelTimer[i] / CurDist[i]);

                }

            }

        }

    }

    private void RemoveArm(int _index)
    {

        Destroy(Arms[_index].gameObject);
        Arms.RemoveAt(_index);
        CurPoint.RemoveAt(_index);
        ArmTravelTimer.RemoveAt(_index);
        CurDist.RemoveAt(_index);

    }

    private void SpawnArm()
    {

        GameObject _tempGO = Instantiate(ConveyorArmPrefab, RailPoints[0].position, Quaternion.identity);
        _tempGO.GetComponent<ConveyorArm>().MovePower = ConveyorSpeed;

        Arms.Add(_tempGO.transform);
        CurPoint.Add(1);
        ArmTravelTimer.Add(0);
        CurDist.Add(Vector3.Distance(RailPoints[0].position, RailPoints[1].position));
        Invoke(nameof(SpawnArm), Random.Range(SpawnTimeMin, SpawnTimeMax));

    }

}
