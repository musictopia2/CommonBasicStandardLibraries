﻿using System;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public static class DatabaseNullables
    {
        public static bool WhatBoolean(bool? thisBoolean)
        {
            if (thisBoolean.HasValue == false)
                return false;
            return thisBoolean!.Value;
        }
        public static bool WhatBoolean(bool thisBoolean)
        {
            return thisBoolean;
        }
        public static bool WhatBoolean(string thisString)
        {
            if (thisString == null)
                return false;
            if (thisString.Trim().Length == 0)
                return false;
            thisString = WhatString(thisString);
            if (thisString.ToLower() == "false")
                return false;
            if (thisString.ToLower() == "true")
                return true;
            if (thisString.Length > 1)
                throw new Exception("Boolean strings has to be 1 character long; not " + thisString.Length + " long");
            if (thisString.ToLower() == "y")
                return true;
            if (thisString.ToLower() == "n")
                return false;
            if (thisString == "1")
                return true;
            if (thisString == "0")
                return false;
            if (thisString == "-1")
                return true;
            bool.TryParse(thisString, out bool NewValue);
            return NewValue;
        }
        public static bool WhatBoolean(int thisInt)
        {
            if (thisInt == 0)
                return false;
            if (thisInt == 1)
                return true;
            if (thisInt == -1)
                return true;// i think -1 should be true as well
            throw new Exception("The Integer has to be 0 or 1 or -1 for booleans; not " + thisInt);
        }
        public static bool WhatBoolean(int? thisInt)
        {
            if (thisInt.HasValue == false)
                return false;
            return WhatBoolean(thisInt!.Value);
        }
        public static bool WhatBoolean(char? thisChar)
        {
            if (thisChar.HasValue == false)
                return false;
            return WhatBoolean(thisChar!.Value);
        }
        public static bool WhatBoolean(char thisChar)
        {
            if (thisChar.ToString().ToLower() == "y")
                return true;
            if (thisChar.ToString().ToLower() == "n")
                return false;
            if (thisChar.ToString() == "0")
                return false;
            if (thisChar.ToString() == "1")
                return true;
            if (thisChar.ToString() == "-1")
                return true;
            throw new Exception("Cannot find out whether true or false for " + thisChar);
        }
        public static string WhatString(int? thisInt)
        {
            if (thisInt.HasValue == false)
                return "";
            return thisInt.ToString();
        }
        public static string WhatString(int thisInt)
        {
            return thisInt.ToString();
        }
        public static string WhatString(decimal? thisDec)
        {
            if (thisDec.HasValue == false)
                return "";
            return thisDec.ToString();
        }
        public static string WhatString(decimal thisDec)
        {
            return thisDec.ToString();
        }
        public static string WhatString(string thisString)
        {
            if (thisString == null)
                return "";
            return thisString.Trim();
        }
        public static string WhatString(char thisString)
        {
            return thisString.ToString();
        }
        public static string WhatString(char? thisString)
        {
            if (thisString.HasValue == false)
                return "";
            return thisString.ToString();
        }
        public static decimal WhatDecimal(decimal? thisDec)
        {
            if (thisDec.HasValue == false)
                return decimal.Parse(string.Format("$0.00", 0));
            return Math.Round(thisDec!.Value, 2);
        }
        public static decimal WhatDecimal(decimal thisDec)
        {
            return Math.Round(thisDec, 2);
        }
        public static decimal WhatDecimal(string thisDec)
        {
            thisDec = WhatString(thisDec);
            decimal.TryParse(thisDec, out decimal NewValue);
            return Math.Round(NewValue, 2);
        }
        public static int WhatInteger(int? thisInt)
        {
            if (thisInt.HasValue == false)
                return 0;
            return thisInt!.Value;
        }
        public static int WhatInteger(int thisInt)
        {
            return thisInt;
        }
        public static int WhatInteger(string thisInt)
        {
            thisInt = WhatString(thisInt); // to take out the spaces
            try
            {
                int.TryParse(thisInt, out int NewValue);
                return NewValue;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}