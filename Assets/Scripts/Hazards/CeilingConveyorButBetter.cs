using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CeilingConveyorButBetter : MonoBehaviour
{

    [Header("Setup"), Tooltip("Toggle this on to use bezier curve for interpolation")]
    [SerializeField] private bool useBezier;
    [Tooltip("Should the conveyor loop")]
    [SerializeField] private bool loopConveyor;
    [SerializeField] private Transform[] points;
    [SerializeField] private float conveyorSpeed, initialDelay, minDelay, maxDelay;
    [Tooltip("This specifies how accurate the curve interpolation will be per curve segment")]
    [SerializeField] private int curveResolution;
    [Header("Objects that are spawned on the conveyor")]
    [SerializeField] private GameObject[] conveyorObjects;
    [SerializeField] private GameObject train;
    [SerializeField] private EventReference trainSound;

    private List<Transform> spawnedItems;
    private List<int> steps;
    private List<float> timers;
    private Vector3[] arcLenghtParamedVectors;

    private void Awake()
    {

        spawnedItems = new List<Transform>();
        timers = new List<float>();
        steps = new List<int>();

#if UNITY_EDITOR

        InvokeRepeating(nameof(UpdatePoints), 0, 1f);

#else

        UpdatePoints();

#endif

    }

    private void Start()
    {

        Invoke(nameof(SpawnItem), initialDelay);
        
    }

    private void SpawnItem()
    {

        GameObject _selectedObject = train;

        if(Random.Range(0, 100) > 5)
        {

            _selectedObject = conveyorObjects[Random.Range(0, conveyorObjects.Length)];

        }
        else
            SoundManager.Instance.PlaySound(trainSound, gameObject);

        Transform _tempItem = Instantiate(_selectedObject, points[0].position, Quaternion.identity).transform;

        spawnedItems.Add(_tempItem);
        timers.Add(0);
        steps.Add(0);
        

        Invoke(nameof(SpawnItem), Random.Range(minDelay, maxDelay));
        
    }

    private void UpdatePoints()
    {

        arcLenghtParamedVectors = new Vector3[curveResolution * (points.Length / 3) + 1];

        int _index = 1;

        arcLenghtParamedVectors[0] = Cerp(0f/(float)curveResolution, 0);

        for(int j = 0; j <= points.Length - 3; j += 3)
        {

            for(int i = 1; i <= curveResolution; i++)
            {

                arcLenghtParamedVectors[_index] = Cerp((float)i/(float)curveResolution, j);
                _index++;

            }
        
        }

    }

    private void Update()
    {

        if(useBezier)
        {

            for(int i = 0; i < spawnedItems.Count; i++)
            {

                Vector3 _newPos = Vector3.Lerp(arcLenghtParamedVectors[steps[i]], arcLenghtParamedVectors[steps[i]+1], timers[i]);

                spawnedItems[i].LookAt(_newPos, Vector3.up);
                spawnedItems[i].position = _newPos;

                timers[i] += Time.deltaTime * (conveyorSpeed / Vector3.Distance(arcLenghtParamedVectors[steps[i]], arcLenghtParamedVectors[steps[i]+1]));

                if(timers[i] > 1)
                {

                    timers[i] -= 1;
                    steps[i]++;

                    if(steps[i] >= arcLenghtParamedVectors.Length - 1)
                    {

                        if(loopConveyor)
                            steps[i] = 0;
                        else
                            RemoveItemAt(i);

                    }

                }

            }

        }
        else
        {
            
            for(int i = 0; i < spawnedItems.Count; i ++)
            {

                Vector3 _newPos = Vector3.Lerp(points[steps[i]].position, points[steps[i]+1].position, timers[i]);

                spawnedItems[i].LookAt(_newPos, Vector3.up);
                spawnedItems[i].position = _newPos;

                timers[i] += Time.deltaTime * (conveyorSpeed / Vector3.Distance(points[steps[i]].position, points[steps[i]+1].position));
                if(timers[i] >= 1)
                {

                    timers[i] = 0;
                    steps[i] += 1;

                    if(steps[i] >= points.Length-1)
                    {

                        RemoveItemAt(i);

                    }

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

        if(points.Length < 3 || curveResolution < 1)
            return;
        
        Gizmos.color = Color.green;

        int _points = points.Length * curveResolution;
        float _t = 0;
        Vector3 _lastPoint = points[0].position, _curPoint;

        for(int i = 0; i < points.Length; i++)
            Gizmos.DrawCube(points[i].position, Vector3.one * 0.1f);

        for(int i = 0; i < points.Length-3; i += 3)
        {

            int _pointCounter = 0;

            for(int j = 0; j < curveResolution; j ++)
            {

                _pointCounter++;
                _t = (float)_pointCounter / (float)curveResolution;

                _curPoint = Cerp(_t, i);

                Gizmos.DrawCube(_curPoint, Vector3.one * 0.05f);

                Gizmos.DrawLine(_lastPoint, _curPoint);
                
                _lastPoint = _curPoint;


            }

        }

    }

    private Vector3 Cerp(float _time, int _ps)
    {

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
