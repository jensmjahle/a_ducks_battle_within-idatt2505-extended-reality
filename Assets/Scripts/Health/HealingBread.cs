﻿using UnityEngine;


    public class HealingBread : MonoBehaviour
    {
        public float bobHeight = 0.2f;
        public float bobSpeed = 3f;

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * bobSpeed) * bobHeight, 0);
        }
    }
