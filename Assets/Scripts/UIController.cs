using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Test test;
    [SerializeField] private GameObject gridHolder;
    [SerializeField] private TMP_InputField widthInputField;
    [SerializeField] private TMP_InputField heightInputField;
    private int width;
    private int height;
    
    
    private void Start()
    {
        GenerateGridDefault();
    }

    public void WidthInputField(string input)
    {
        int.TryParse(widthInputField.text, out var result);
        width = result;
    }
    
    public void HeightInputField(string input)
    {
        int.TryParse(heightInputField.text, out var result);
        height = result;
    }
    
    public void GenerateGrid()
    {
        foreach (Transform child in gridHolder.transform) 
        {
            Destroy(child.gameObject);
        }
        test.ResetPlayerPosition();
        grid.GenerateGrid(width, height, 10f, Vector3.zero);
    }

    public void GenerateGridDefault()
    {
        foreach (Transform child in gridHolder.transform) 
        {
            Destroy(child.gameObject);
        }
        test.ResetPlayerPosition();
        grid.GenerateGrid(10, 5, 10f, Vector3.zero);
    }
}
