using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingConveyorButBetter : MonoBehaviour
{

    [Header("Setup")]
    [SerializeField] private Transform[] points;
    [SerializeField] private float conveyorSpeed, initialDelay, minDelay, maxDelay;
    [Header("Objects that are spawned on the conveyor")]
    [SerializeField] private GameObject[] conveyorObjects;
    [SerializeField] private GameObject train;
    [Header("Visualization")]
    [SerializeField] private int resolution;

    private List<Transform> spawnedItems;
    private List<int> steps;
    private List<float> timers;

    private void Awake()
    {

        spawnedItems = new List<Transform>();
        timers = new List<float>();
        steps = new List<int>();

    }

    private void Start()
    {

        Invoke(nameof(SpawnItem), initialDelay);
        
    }

    private void SpawnItem()
    {

        Transform _tempItem = Instantiate(train, points[0].position, Quaternion.identity).transform;

        spawnedItems.Add(_tempItem);
        timers.Add(0);
        steps.Add(0);
        

        Invoke(nameof(SpawnItem), Random.Range(minDelay, maxDelay));
        
    }

    private void Update()
    {

        if(points.Length < 3 || spawnedItems.Count < 1)
            return;

        for(int i = 0; i < spawnedItems.Count; i ++)
        {

            Vector3 _newPos = Cerp(timers[i], steps[i]);

            spawnedItems[i].LookAt(_newPos, Vector3.up);
            spawnedItems[i].position = _newPos;

            timers[i] += Time.deltaTime * conveyorSpeed;
            if(timers[i] >= 1)
            {

                timers[i] = 0;
                steps[i] += 3;

                Debug.Log(steps[i] + " + " + points.Length);

                if(steps[i] >= points.Length-1)
                {

                    RemoveItemAt(i);

                }

            }

        }

    }

    private void RemoveItemAt(int _index)
    {

        Destroy(spawnedItems[_index].gameObject);
        spawnedItems.RemoveAt(_index);
        timers.RemoveAt(_index);
        steps.RemoveAt(_index);

    }

    private void OnDrawGizmos()
    {

        if(points.Length < 3 || resolution < 1)
            return;
        
        Gizmos.color = Color.green;

        int _points = points.Length * resolution;
        float _t = 0;
        Vector3 _lastPoint = points[0].position, _curPoint;

        for(int i = 0; i < points.Length-3; i += 3)
        {

            Gizmos.DrawCube(points[i].position, Vector3.one * 0.1f);
            int _pointCounter = 0;

            for(int j = 0; j < resolution; j ++)
            {

                _pointCounter++;
                _t = (float)_pointCounter / (float)resolution;

                _curPoint = Cerp(_t, i);

                Gizmos.DrawCube(_curPoint, Vector3.one * 0.05f);

                Gizmos.DrawLine(_lastPoint, _curPoint);
                
                _lastPoint = _curPoint;


            }

        }

    }

    private Vector3 Cerp(float _time, int _ps)
    {

        /*if(_time >= 1)
            return points[points.Length-1].position;*/

        //int _s = (int)(_time * (points.Length-3));

        Vector3 _pointA = points[_ps].position;
        Vector3 _pointB = points[_ps+1].position;
        Vector3 _pointC = points[_ps+2].position;
        Vector3 _pointD = points[_ps+3].position;

        Vector3 _pointAB = Vector3.Lerp(_pointA, _pointB, _time);
        Vector3 _pointBC = Vector3.Lerp(_pointB, _pointC, _time);
        Vector3 _pointCD = Vector3.Lerp(_pointC, _pointD, _time);

        Vector3 _pointABBC = Vector3.Lerp(_pointAB, _pointBC, _time);
        Vector3 _pointBCCD = Vector3.Lerp(_pointBC, _pointCD, _time);

        return Vector3.Lerp(_pointABBC, _pointBCCD, _time);

    }

}
