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

        int current = number;

        while (i < l)
        {
            digits[i].gameObject.SetActive(true);

            if (current <= 0)
            {
                if (hideZero)
                {
                    if (i != 0)
                    {
                        digits[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    digits[i].n = 0;
                }
            }
            else
            {
                int d = current % 10;
                current = current / 10;
                digits[i].n = d;
            }

            i++;
        }
    }

    public void OnEnable()
    {
        Change();
    }

    public void Update()
    {
        //if (oldNumber != number)
        {
            Change();
        }
    }
}
