﻿using UnityEngine;
using Yashlan.bird;

namespace Yashlan.shoot
{
    public class SlingShooter : MonoBehaviour
    {
        [SerializeField]
        private CircleCollider2D _circleCollider2D;
        [SerializeField]
        private LineRenderer _trajectory;
        [SerializeField]
        private float _radius = 0.75f;
        [SerializeField]
        private float _throwSpeed = 30f;

        private Bird _bird;
        private Vector2 _startPos;

        void Start()
        {
            _startPos = transform.position;
        }

        void OnMouseUp()
        {
            _circleCollider2D.enabled = false;
            Vector2 velocity = _startPos - (Vector2)transform.position;
            float distance = Vector2.Distance(_startPos, transform.position);
            _bird.Shoot(velocity, distance, _throwSpeed);
            gameObject.transform.position = _startPos;
            _trajectory.enabled = false;
        }

        void OnMouseDrag()
        {
            Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = p - _startPos;

            if (dir.sqrMagnitude > _radius)
                dir = dir.normalized * _radius;

            transform.position = _startPos + dir;

            float distance = Vector2.Distance(_startPos, transform.position);

            if (!_trajectory.enabled)
                _trajectory.enabled = true;

            DisplayTrajectory(distance);
        }

        public void InitiateBird(Bird bird)
        {
            _bird = bird;
            _bird.MoveTo(gameObject.transform.position, gameObject);
            _circleCollider2D.enabled = true;
        }

        void DisplayTrajectory(float distance)
        {
            if (_bird == null) return;

            Vector2 velocity = _startPos - (Vector2)transform.position;
            int segmentCount = 5;
            Vector2[] segments = new Vector2[segmentCount];

            segments[0] = transform.position;
            Vector2 segVelocity = velocity * _throwSpeed * distance;

            for (int i = 1; i < segmentCount; i++)
            {
                float elapsedTime = i * Time.fixedDeltaTime * 5;
                segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
            }

            _trajectory.positionCount = segmentCount;

            for (int i = 0; i < segmentCount; i++)
            {
                _trajectory.SetPosition(i, segments[i]);
            }
        }
    }
}
