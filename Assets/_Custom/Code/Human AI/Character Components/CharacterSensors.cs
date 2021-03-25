using System.Collections.Generic;
using Drawing;
using UnityEngine;

namespace _Custom.Code
{
    public class CharacterSensors : MonoBehaviour
    {
        private List<Transform> sensorTransforms;

        public bool detects;
        public Vector3 detectHitPoint;
        public GameObject detectHitObject;

        public void Start()
        {
            sensorTransforms = new List<Transform>();
            GameObject sensorsParent = transform.Find("Sensors").gameObject;
            for (int index = 0; index < sensorsParent.transform.childCount; index++)
            {
                sensorTransforms.Add(sensorsParent.transform.GetChild(index));
            }
        }

        public void Update()
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << HumanAIConfigs.NPC_CHARACTER_LAYER;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            foreach (Transform sensorTransform in sensorTransforms)
            {
                RaycastHit hit;
                if (Physics.Raycast(sensorTransform.position, sensorTransform.TransformDirection(Vector3.forward),
                    out hit,
                    Mathf.Infinity))
                {
                    Draw.Line(sensorTransform.position, hit.point,
                        Color.red);
                    detects = true;
                    detectHitPoint = hit.point;
                    detectHitObject = hit.collider.gameObject;
                }
                else
                {
                    Draw.Line(sensorTransform.position, sensorTransform.TransformDirection(Vector3.forward) * 1000,
                        Color.white);
                    detects = false;
                }
            }
        }
    }
}