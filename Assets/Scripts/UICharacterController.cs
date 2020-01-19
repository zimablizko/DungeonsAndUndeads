using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterController : MonoBehaviour
{
    [SerializeField] private PressedButton left;
    [SerializeField] private PressedButton right;
    [SerializeField] private Button jump;
    [SerializeField] private Button fire;

    public PressedButton Left => left;
    public PressedButton Right => right;
    public Button Jump => jump;
    public Button Fire => fire;

    void Start()
    {
        Player.Instance.InitUIController(this);
    }
    
}
