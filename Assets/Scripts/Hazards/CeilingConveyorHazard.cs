using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingConveyorHazard : MonoBehaviour
{

    public float SpawnTimeMin, SpawnTimeMax, ConveyorSpeed, DistFromPoint;
    public GameObject ConveyorArmPrefab, warningFX;
    public Transform[] RailPoints;
    public List<Transform> arms;
    public List<int> curPoint;
    private List<float> armTravelTimer, curDist;
    public float RolledTime, WarningTime;
    //EnableThis one if it's more cosmetic
    public bool noWarnings;
    private void Start()
    {

        arms = new List<Transform>();
        curPoint = new List<int>();
        armTravelTimer = new List<float>();
        curDist = new List<float>();
        Invoke(nameof(SpawnArm), 1);

    }

    private void Update()
    {if(noWarnings == true)
        {
            goto SkipWarning;
        }
        WarningTime -= Time.deltaTime;
        if(WarningTime < 0)
        {
            warningFX.SetActive(true);
        }
    SkipWarning:;
        if(arms.Count > 0)
        {

            for(int i = 0; i < arms.Count; i++)
            {

                armTravelTimer[i] += Time.deltaTime * ConveyorSpeed;

                if(Vector3.Distance(arms[i].position, RailPoints[curPoint[i]].position) < DistFromPoint)
                {

                    curPoint[i]++;
                    if(curPoint[i] == RailPoints.Length)
                    {

                        RemoveArm(i);

                    }
                    else
                    {

                        curDist[i] = Vector3.Distance(RailPoints[curPoint[i] - 1].position, RailPoints[curPoint[i]].position);
                        armTravelTimer[i] = 0;

                    }

                }
                else
                {

                    arms[i].position = Vector3.Lerp(RailPoints[curPoint[i]-1].position, RailPoints[curPoint[i]].position, armTravelTimer[i] / curDist[i]);

                }

            }

        }

    }

    private void RemoveArm(int _index)
    {

        Destroy(arms[_index].gameObject);
        arms.RemoveAt(_index);
        curPoint.RemoveAt(_index);
        armTravelTimer.RemoveAt(_index);
        curDist.RemoveAt(_index);

    }

    private void SpawnArm()
    {

        GameObject _tempGO = Instantiate(ConveyorArmPrefab, RailPoints[0].position, Quaternion.identity);
        _tempGO.GetComponent<ConveyorArm>().MovePower = ConveyorSpeed;

        arms.Add(_tempGO.transform);
        curPoint.Add(1);
        armTravelTimer.Add(0);
        curDist.Add(Vector3.Distance(RailPoints[0].position, RailPoints[1].position));
        RolledTime = Random.Range(SpawnTimeMin, SpawnTimeMax);
        WarningTime = RolledTime- 1;
        if(noWarnings== false)
        {
            warningFX.SetActive(false);
        }
        
        Invoke(nameof(SpawnArm), RolledTime);
        
    }

}
