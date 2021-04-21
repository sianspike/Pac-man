using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSelectMenu: MonoBehaviour
{
        [SerializeField] public Text profile1Text;
        [SerializeField] public Text profile2Text;
        [SerializeField] public Text profile3Text;
        [SerializeField] public Text profile1LevelText;
        [SerializeField] public Text profile2LevelText;
        [SerializeField] public Text profile3LevelText;
        [SerializeField] public Text selector;

        private void Update()
        {
              SelectProfile();  
        }

        private void SelectProfile()
        {
            var selectorPosition = selector.transform.localPosition;

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (selectorPosition.y.Equals(profile3Text.transform.localPosition.y))
                {
                    selector.transform.localPosition = new Vector3(selectorPosition.x,
                        profile1Text.transform.localPosition.y, selectorPosition.z);
                }
                else
                {
                    selector.transform.localPosition =
                        new Vector3(selectorPosition.x, selectorPosition.y - 128, selectorPosition.z);
                }
            } else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (selectorPosition.y.Equals(profile1Text.transform.localPosition.y))
                {
                    selector.transform.localPosition = new Vector3(selectorPosition.x,
                        profile3Text.transform.localPosition.y, selectorPosition.z);
                }
                else
                {
                    selector.transform.localPosition =
                        new Vector3(selectorPosition.x, selectorPosition.y + 128, selectorPosition.z);
                }
            } else if (Input.GetKeyUp(KeyCode.Return))
            {
                if (selectorPosition.y.Equals(profile1Text.transform.localPosition.y))
                {
                    
                } else if (selectorPosition.y.Equals(profile2Text.transform.localPosition.y))
                {
                    
                } else if (selectorPosition.y.Equals(profile3Text.transform.localPosition.y))
                {
                    
                }
            }
        }
}