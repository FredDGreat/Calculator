using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;
using System.Globalization;

public class CalcManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultPreview;

    private string[] arithOperators = {"+","-","x","/"};
    private List<string> operatorList = new();// the same with: new List<string>();
    private List<string> equationList = new();
    private List<string> equationOpList = new();

    private float resultHolder;
    private float previewHolder;
    private float input1;
    private float input2;

    private bool rejectDoubleOperator;
    private bool rejectDoubleDot;


    private string operation;
    private string tempValue = "";
    private bool clearTempValue;
    private bool answer;
    // Start is called before the first frame update
    void Start()
    {
        resultText.text = "";
        resultPreview.text = "";
        for (int i = 0; i < arithOperators.Length; i++) operatorList.Add(arithOperators[i]);
    }

    // Update is called once per frame
    void Update()
    {
        if (clearTempValue) tempValue = "";
    }

    public void AddNumber(string value)
    {
        if (answer)
        {
            resultText.text = "";
            answer = false;
        }
        resultText.text += value;
        tempValue += value;
        rejectDoubleOperator = false;
        if (!string.IsNullOrEmpty(operation))
        {
            equationOpList.Add(operation);
            operation = "";
        }
    }

    public void AddOperation(string value)
    {
        string result = resultText.text;
        if (!string.IsNullOrEmpty(result) && !answer)
        {
            if (!result.Equals("-") && !rejectDoubleOperator)
            {
                rejectDoubleOperator = true;
                resultText.text += value;
                operation = value;
                equationList.Add(tempValue);
                rejectDoubleDot = false;
                //Debug.Log("equationList.Count = " + equationList.Count);
                //Debug.Log("equationList item = " + equationList[0]);
                if (equationList.Count == 1) resultHolder = float.Parse(tempValue);
                if (equationList.Count > 1)
                {
                    //Debug.Log("equationOpList[equationOpList.Count - 1] item = " + equationOpList[equationOpList.Count - 1]);
                    resultHolder = SolveEquation(resultHolder, equationOpList[equationOpList.Count - 1], float.Parse(equationList[equationList.Count-1]));
                    resultPreview.text = "" + resultHolder;
                }
            }
            tempValue = "";
        }
        else
        {
            if (value.Equals("-"))
            {
                if (answer)
                {
                    resultText.text = "";
                    answer = false;
                }
                resultText.text = value;
                tempValue += value;
            }
        }
    }

    public void ShowResult()
    {

        if (equationList.Count > 0)
        {
            resultHolder = SolveEquation(resultHolder, equationOpList[equationOpList.Count - 1], float.Parse(tempValue));
            resultText.text = "" + resultHolder;
            resultPreview.text = "";
            answer = true;
            equationList.Clear();
            equationOpList.Clear();
            resultHolder = 0f;
            tempValue = "";
            operation = "";
        }
    }

    private float SolveEquation(float input1, string op, float input2)
    {
        float result = 0f;
        switch (op)
        {
            case "+":
                result = input1 + input2;
                break;
            case "-":
                result = input1 - input2;
                break;
            case "x":
                result = input1 * input2;
                break;
            case "/":
                result = input1 / input2;
                break;
        }

        return result;
    }

    public void Delete()
    {
        equationList.Clear();
        equationOpList.Clear();
        resultHolder = 0f;
        tempValue = "";
        operation = "";
        resultText.text = "";
        resultPreview.text = "";
        rejectDoubleDot = false;

        //Debug.Log("equationList size = " + equationList.Count);
        //Debug.Log("equationOpList size = " + equationOpList.Count);
    }

    public void AddDot(string value)
    {
        if (!rejectDoubleDot) {
            rejectDoubleDot = true;
            resultText.text += value;
            tempValue += value;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
