using UnityEngine;
using System.Collections.Generic;

/**
  * @class NumberManager
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class NumberManager : MonoBehaviour
{
    public DigitSprite[] digits;
    public int number;

    public bool hideZero;
    private int oldNumber;

    public void Change()
    {
        oldNumber = number;

        int l = digits.Length;
        int i = 0;

        if (number == 0)
        {
            if (hideZero)
            {
                digits[i++].gameObject.SetActive(true);
            }

            digits[i].n = 0;


            while (i < l)
            {
                if (hideZero)
                {
                    digits[i++].gameObject.SetActive(false);
                }
                else
                {
                    digits[i++].n = 0;
                }
            }
            return;
        }

        int current = number;

        while (current > 0)
        {
            int d = current % 10;
            current = current / 10;
            if (hideZero)
            {
                digits[i].gameObject.SetActive(true);
            }
            digits[i++].n = d;
        }

        while (i < l)
        {
            if (hideZero)
            {
                digits[i++].gameObject.SetActive(false);
            }
            else
            {
                digits[i++].n = 0;
            }
        }
    }

    public void OnEnable()
    {
        Change();
    }

    public void Update()
    {
        if (oldNumber != number)
        {
            Change();
        }
    }
}
