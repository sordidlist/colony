using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Custom.Code
{
    public class CharacterKnownLocations : MonoBehaviour
    {
        public Vector3 DEV_TABLE;

        public void Start()
        {
            DEV_TABLE = GameObject.Find("DiningTable").transform.position;
        }
    }
}