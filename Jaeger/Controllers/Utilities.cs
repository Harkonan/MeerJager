using System;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Controllers
{
    class Utilities
    {

        public static char ConvertNumberToChar(int Input)
        {
            char CharResult;

            if (Input < 10)
            {
                CharResult =  Convert.ToChar(Input.ToString());
            }
            else if (Input < 40)
            {
                CharResult = (char)(Input + 55);
            }
            else
            {
                throw new Exception("Too Many Menu options to automate!");
            }
            

            return CharResult;
        }
    }
}
