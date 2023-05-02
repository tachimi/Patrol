using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _point;
    [SerializeField] private GameObject _cubePrefab;

    [SerializeField] private Vector3[] _pointPositions;

    [SerializeField] private float _speed;
    [SerializeField] private float _waitingTime;

    private GameObject _cube;

    private float _currentTime;
    private int _index;
    private bool _isUpdating = true;
    private float _timer;

    private void OnValidate()
    {
        for (var i = 0; i < _pointPositions.Length; i++)
        {
            _pointPositions[i].x = Mathf.Clamp(_pointPositions[i].x, -19.5f, 19.5f);
            _pointPositions[i].y = Mathf.Clamp(_pointPositions[i].y, 0f, 0f);
            _pointPositions[i].z = Mathf.Clamp(_pointPositions[i].z, -10.5f, 10.5f);
        }
    }

    private void Awake()
    {
        _timer = _waitingTime;
        if (_pointPositions == null) return;
        foreach (var point in _pointPositions)
        {
            Instantiate(_point);
            _point.transform.position = point;
        }

        _cube = Instantiate(_cubePrefab);
        _cube.transform.position = _pointPositions[0];
    }


    private void Update()
    {
        if (_isUpdating)
        {
            _currentTime += Time.deltaTime;

            IsOnPoint();
            IsOnLastPoint();


            var distance = Vector3.Distance(_cube.transform.position, _pointPositions[_index]);

            var travelTime = distance / _speed;


            var newPosition =
                Vector3.Lerp(_cube.transform.position, _pointPositions[_index], _currentTime / travelTime * Time.deltaTime);
            _cube.transform.position = newPosition;
        }
        else
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _isUpdating = !_isUpdating;
                _timer = _waitingTime;
            }
        }
    }

    private void IsOnPoint()
    {
        if (_cube.transform.position != _pointPositions[_index]) return;
        _index++;
        _currentTime = 0f;
        StopUpdating();
    }

    private void IsOnLastPoint()
    {
        if (_cube.transform.position == _pointPositions[^1])
        {
            _index = 0;
        }
    }

    private void StopUpdating()
    {
        _isUpdating = !_isUpdating;
    }
}