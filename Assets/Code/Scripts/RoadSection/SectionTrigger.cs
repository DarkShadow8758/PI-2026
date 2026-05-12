using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public float moveStep;
    public float stepCount;
    public GameObject roadSection;
    public List<GameObject> sections = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            stepCount+=1;
            GameObject newSection = Instantiate(roadSection, new Vector3(0, 0, moveStep*stepCount), Quaternion.identity);
            sections.Add(newSection);
            if (sections.Count >= 3)
            {
                Destroy(sections[0]);
                sections.RemoveAt(0);
                
            }
        }
    }
}
