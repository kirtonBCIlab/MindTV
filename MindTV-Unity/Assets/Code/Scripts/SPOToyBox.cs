using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BCIEssentials.Utilities;
using BCIEssentials.StimulusObjects;

public class SPOToyBox : MonoBehaviour
{
    [SerializeField]
    private IDictionary<int, GameObject> spoObjectDictionary = new Dictionary<int, GameObject>();
    [SerializeField]
    private IDictionary<int, string> spoLabelDictionary = new Dictionary<int, string>();
    
    // Start is called before the first frame update
    void Start()
    {
        SPOToyBox spoToyBox = FindObjectOfType<SPOToyBox>();
        if (spoToyBox != null && spoToyBox != this) 
        {
            Destroy(gameObject);
        }
        else    
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Each SPO has a unique ID from training and maybe a label

    // Add SPO - run this method when a new SPO is trained
    public void SetSPO(int spoID, GameObject spoObject, string spoLabel)
    {
        print("Setting SPO with ID " + spoID + " and label " + spoLabel);
        // if that spoID already exists, remove it
        if (spoObjectDictionary.ContainsKey(spoID))
        {
            spoObjectDictionary.Remove(spoID);
            spoLabelDictionary.Remove(spoID);
        }

        spoObjectDictionary.Add(spoID, spoObject);
        spoLabelDictionary.Add(spoID, spoLabel);
    }

    // Remove SPO - is this needed?

    // Get SPO 
    public GameObject GetSPO(int spoID)
    {
        // if the spoID does not exist, return null
        if (!spoObjectDictionary.ContainsKey(spoID))
        {
            print("SPO with ID " + spoID + " not found in SPOToyBox!");
            return null;
        }

        return spoObjectDictionary[spoID];
    }
}
