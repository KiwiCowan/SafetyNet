using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Draw : MonoBehaviour
{
    public TextMeshProUGUI _text;

    public void Show()
    {
        gameObject.SetActive(true);       
        StartCoroutine(WaitForIt());
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
