using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostCode : MonoBehaviour
{
    private Text Text;
    // Start is called before the first frame update
    private void Start()
    {
        Text = GetComponent<Text>();
    }

    void Update()
    {
        Text.text = TestRelay.JoinCode;
    }
}
