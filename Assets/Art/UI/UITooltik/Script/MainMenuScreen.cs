using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private int _lol;

    void Start()
    {
        Generate();
    }

    void OnValidate()
    {
        if (Application.isPlaying) return;
        Generate();
    }
    void Generate()
    {
        var root = _document.rootVisualElement;   
        root.Clear();

        root.styleSheets.Add(_styleSheet);   

        var titleLabel = new Label("Hello");

        root.Add(titleLabel);

        var redBoy = new VisualElement();
        redBoy.AddToClassList("red-boy");

        root.Add(redBoy);

        var blueBoy = new VisualElement();
        blueBoy.AddToClassList("blue-boy");

        root.Add(blueBoy);
    }
}
