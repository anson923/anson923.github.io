using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TypeWritter : MonoBehaviour
{
    private Text message;
    private string Text;
    

    private void Awake()
    {
        message = GetComponent<Text>();
        Text = message.text;
        message.text = "";
    }
    public IEnumerator GoalTypeWritter()
    {
        foreach (char word in Text)
        {
            message.text += word;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

