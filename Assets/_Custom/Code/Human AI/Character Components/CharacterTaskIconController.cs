using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Custom.Code
{
    public class CharacterTaskIconController : MonoBehaviour
    {
        private Camera currentCamera;
        private List<CharacterAI> humanCharacters;

        public void Start()
        {
            humanCharacters = new List<CharacterAI>();
            AddCharacterIfPresent("Graham");
            AddCharacterIfPresent("Lorraine");
            currentCamera = Camera.main;
        }

        public void Update()
        {
            UpdateAllTaskIcons();
        }

        private void AddCharacterIfPresent(string characterName)
        {
            try
            {
                humanCharacters.Add(GameObject.Find(characterName).GetComponent<CharacterAI>());
                transform.Find(characterName + "TaskIcon").gameObject.SetActive(true);
            }
            catch (NullReferenceException e)
            {
            }
        }

        private void UpdateAllTaskIcons()
        {
            foreach (CharacterAI character in humanCharacters)
            {
                float characterDistanceFromCamera =
                    Vector3.Distance(character.transform.position, currentCamera.transform.position);
                float taskIconAdjustmentForDistance = characterDistanceFromCamera / 100f;
                character.taskIcon.position =
                    currentCamera.WorldToScreenPoint(character.taskIconTrackingTransform.position -
                                                     new Vector3(0f, taskIconAdjustmentForDistance, 0f));
            }
        }
    }
}