using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Victory : MonoBehaviour
{
    public TextMeshProUGUI _text;

    public void Show(string token)
    {
        gameObject.SetActive(true);
        _text.text = token;
        StartCoroutine(WaitForIt());
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
