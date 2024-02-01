using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TrainingPageSO", menuName = "Scriptable Objects/TrainingPageSO", order = 1)]
public class TrainingPageSO : ScriptableObject
{
    public TabGroup trainingTabGroup;
    public List<GameObject> trainingPages;

    public void AddTrainingPage(GameObject trainingPage)
    {
        trainingPages.Add(trainingPage);
    }

    public void RemoveTrainingPage(GameObject trainingPage)
    {
        trainingPages.Remove(trainingPage);
    }

    public void RemoveTrainingPage(int index)
    {
        trainingPages.RemoveAt(index);
    }

    public void ClearTrainingPages()
    {
        trainingPages.Clear();
    }

    public Dictionary<string, Color> colors = new Dictionary<string, Color>
    {
        {"Red", Color.red },
        {"Blue", Color.blue },
        {"Green", Color.green },
        {"White", Color.white },
        {"Yellow", Color.yellow },
        {"Black", Color.black },
        {"Magenta", Color.magenta},
        {"Grey", Color.grey},
        {"Cyan", Color.cyan},
        {"Blue (Theme)", new Color(0.34117647f, 0.72156863f, 1.0f, 1.0f)},
        {"Green (Theme)", new Color(0.26277451f, 0.6666667f, 0.54509804f, 1.0f)},
        {"Purple (Theme)", new Color(0.3254902f, 0.21960784f, 0.57254902f, 1.0f)}
    };
    
}
