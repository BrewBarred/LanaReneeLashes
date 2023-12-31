﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static LanaReneeLashes.Tools.Logger;

namespace LanaReneeLashes
{
    /// <summary>
    /// Tools (Custom methods) that could be useful in other applications too
    /// </summary>
    public static class Tools
    {
        #region Class Scope Variables

        ///////
        // Defaults
        ////

        // default inactive color
        static Color defaultInactiveColor = Color.LightGray;
        // default active textbox color
        static Color defaultActiveTextBoxColor = SystemColors.InactiveCaption;
        /// <summary>
        /// Defines the regular expression to represent a digit
        /// </summary>
        static private Regex currencyPatternString = new Regex(@"^0$|^(\$?[1-9]){1}([\d]{0,3}([\,]{1})?[\d]{1,3}?)*(\d*|(\.{1}[\d]{1,2}))$", RegexOptions.Compiled);
        // variable used to parse valid keys for textboxes
        static private string currencyPattern = currencyPatternString.ToString();
        // regex pattern to match non-currency number values
        static private Regex numberPatternString = new Regex(@"^0$|^([1-9]){1}([\d]{0,3}([\,]{1})?[\d]{1,3}?)*(\d*|(\.{1}[\d]{1,2}))$", RegexOptions.Compiled);
        // variable used to parse text regex
        static private string numberPattern = numberPatternString.ToString();

        #endregion

        #region Disable(TextBoxBase textbox)
        /// <summary>
        /// Extends a textBoxBase to disable it
        /// </summary>
        /// <param name="textBox">Textbox to disable</param>
        public static void Disable(this TextBoxBase textBox)
        {
            // greys out textbox
            textBox.BackColor = defaultInactiveColor;
            // disables textbox
            textBox.Enabled = false;

        } // end void
        #endregion

        #region Enable(this TextBoxBase textbox, string defaultValue)
        /// <summary>
        /// Extends a textBoxBase to enable it and inserts the passed default value
        /// </summary>
        /// <param name="textBox">Textbox to enable</param>
        /// <param name="defaultValue">Default value to insert into textbox</param>
        public static void Enable(this TextBoxBase textBox, string defaultValue)
        {
            // disables textbox
            textBox.Enabled = true;
            // sets backcolor back to normal
            textBox.BackColor = defaultActiveTextBoxColor;
            // sets textbox text to 0
            textBox.Text = defaultValue;

        } // end void
        #endregion

        #region Enable(this TextBoxBase textbox)
        /// <summary>
        /// Enables a passed textbox and sets it's default value to zero
        /// </summary>
        /// <param name="textBox">Textbox to enable</param>
        public static void Enable(this TextBoxBase textBox)
        {
            // enables the textbox with passed default value
            textBox.Enable("0");

        } // end void
        #endregion

        #region IsNumeric()
        /// <summary>
        /// Extends a string and returns true if the whole string is a valid numeric string
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string thisString)
        {
            // if the extended key event matches the number pattern regex
            if (Regex.IsMatch(thisString, numberPattern))
            {
                return true;

            } // end if

            // writes error to console
            Log("Invalid input detected in string: \"" + thisString + "\"");
            return false;

        } // end bool
        #endregion

        #region IsCurrency()
        /// <summary>
        /// Extends a string and returns true if the whole string is a valid currency string
        /// </summary>
        /// <param name="thisString">String to validate</param>
        /// <returns></returns>
        public static bool IsCurrency(this string thisString)
        {
            // if the extended key event matches the currency pattern regex
            if (Regex.IsMatch(thisString, currencyPattern))
            {
                return true;

            } // end if

            // writes error to console
            Log("Invalid input detected in string: \"" + thisString + "\"");
            return false;

        } // end bool
        #endregion

        #region ToInt(this string thisString)
        /// <summary>
        /// Extends a string to return an integer value stripped of unnecessary symbols/words
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static int ToInt(this string thisString)
        {
            // uses the ToDecimal() method to strip back the passed string
            decimal decimalValue = thisString.ToDecimal();
            // converts the decimal value to an int and returns the result
            return (int)decimalValue;

        } // end int
        #endregion

        #region ToDecimal(this string thisString)
        /// <summary>
        /// Extends a formatted string to return a decimal value stripped of unnecessary symbols/words
        /// </summary>
        /// <param name="thisString">String to convert to a stripped decimal value</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string thisString)
        {
            try
            {
                // if the passed string is empty
                if (thisString is "")
                {
                    // sets the passed string to "0"
                    thisString = "0";

                } // end if

                // removes any "$" symbols from the string
                string newString = thisString.Replace("$", "");
                // removes "cents" from the text (found in boxing cost)
                newString = newString.Replace("cents", "");
                // removes " hours" from the text (found in est. hours to box)
                newString = newString.Replace("hrs", ".");
                // removes " minutes" from the text (found in est. hours to box)
                newString = newString.Replace("mins", "");
                // removes any white spaces from the string
                newString = newString.Replace(" ", "");
                // removes any commas from the string and parses the stripped value into a decimal
                decimal newDecimal = decimal.Parse(newString.Replace(",", ""));

                // returns the stripped decimal
                return newDecimal;
            }
            catch (Exception ex)
            {
                LogError("Failed to parse \"" + thisString + "\" into a decimal value", ex.Message);
                return -1;

            } // end try

        } // end decimal
        #endregion

        #region ToDouble(this string thisString)
        /// <summary>
        /// Extends a formatted string to return a double value stripped of unnecessary symbols/words
        /// </summary>
        /// <param name="thisString">String to strip and convert to a double</param>
        /// <returns></returns>
        public static double ToDouble(this string thisString)
        {
            // converts passed string to a stripped decimal value
            decimal decimalValue = thisString.ToDecimal();
            // converts the decimal to a double to 2 d.p. and returns the result
            return double.Parse(decimalValue.ToString("0.##"));

        } // end double
        #endregion

        #region FormatHours(this double thisDouble)
        /// <summary>
        /// Extends a double value, splits it into hours and minutes and returns the result as a string
        /// </summary>
        /// <param name="thisDouble">Double value to be formatted</param>
        /// <returns></returns>
        public static string FormatHours(this double thisDouble)
        {
            try
            {
                // uses the ToDecimal() method to strip any unnecessary symbols/words
                thisDouble = (double)thisDouble.ToString().ToDecimal();

                // if the passed double value has a decimal point
                if (thisDouble.ToString().Contains("."))
                {
                    // splits the string by decimal point
                    string[] splitString = thisDouble.ToString().Split('.');
                    // if the double value had more than 1 decimal point
                    if (splitString.Length < 1)
                    {
                        // writes error to console
                        LogError("Failed to convert the value \"" + thisDouble + "\" to hours and minutes");
                        return "Error!";
                    }
                    // else if the value after the decimal point is equal to zero
                    if (double.Parse(splitString[1]) == 0)
                    {
                        // returns the estimated hours as a string
                        return splitString[0] + " hrs";

                    }
                    else if (splitString.Length >= 2)
                    {
                        // stores the hours from the split string and formats it into an "hours" string
                        string hours = splitString[0] + " hrs ";
                        // converts value after decimal point to a to a 2 digit number
                        string minutes = splitString[1];

                        // if minutes is a single digit number
                        if (minutes.Count() == 1)
                        {
                            // inserts a zero to the end of the number
                            minutes = minutes.Insert(1, "0");

                        }
                        // else if minutes length is greater than two
                        else if (minutes.Count() > 2)
                        {
                            // takes only the first two characters of the string
                            minutes = minutes.Substring(0, 2);

                        }// end if

                        // converts 2 digit decimal value into a formatted "minutes" string
                        minutes = Math.Ceiling((decimal.Parse(minutes) / 100 * 60)).ToString() + " mins";

                        // returns the passed double value as a string in hours and minutes format
                        return hours + minutes;

                    } // end if

                } // end if

                // returns the passed double as a string formatted to hours
                return thisDouble.ToString() + " hrs";
            }
            catch (Exception ex)
            {
                // writes error to console
                LogError("Couldn't convert \"" + thisDouble + "\" to hours format!", ex.Message);
                return "Error!";

            } // end try

        } // end string
        #endregion

        #region FormatCurrency(this decimal thisDecimal)
        /// <summary>
        /// Extends a decimal value to return it as a string formatted to a currency value
        /// </summary>
        /// <param name="thisDecimal">Decimal to convert to a string formatted as a currency value</param>
        /// <returns></returns>
        public static string FormatCurrency(this decimal thisDecimal)
        {
            try
            {

                // stores the decimal value formatted as a currency string
                string newCurrency;

                // if thisDecimal is equal to 0
                if (thisDecimal == 0)
                {
                    // sets newCurrency to $0
                    newCurrency = "$0";
                }
                // if thisDecimal is a negative value
                else if (thisDecimal < 0)
                {
                    // formats passed decimal value to 2 d.p. with a 0 in front and inserts the dollar sign 'after' the negative sign
                    newCurrency = thisDecimal.ToString("#.00").Insert(1, " $");
                }
                // if thisDecimal is less than 1
                else if (thisDecimal < 1)
                {
                    // formats passed decimal value to 2 d.p. with a 0 in front
                    newCurrency = "$0" + thisDecimal.ToString("#.00");
                }
                // else if thisDecimal is greater than 1
                else
                {
                    // formats passed decimal value to 2 d.p.
                    newCurrency = "$" + thisDecimal.ToString("#.00");

                } // end if

                // returns the formatted decimal value as a string
                return newCurrency;
            }
            catch (Exception ex)
            {
                // logs error message
                LogError("Failed to parse " + thisDecimal + " into a decimal value", ex.Message);
                return null;

            } // end try

        } // end string
        #endregion

        #region Count(this string thisString)
        /// <summary>
        /// Extends a string and returns the number of characters in it excluding white spaces
        /// </summary>
        /// <returns></returns>
        public static int Count(this string thisString)
        {
            // trims all white spaces from the string
            thisString = thisString.Trim();
            // splits this string into a character array
            char[] charArray = thisString.ToCharArray();

            // returns the length of the character array as an integer value
            return charArray.Length;

        } // end int
        #endregion

        #region ShowUsYaTips(text)
        /// <summary>
        /// Extends a control to display a tooltip on it with the passed text
        /// </summary>
        /// <param name="control">Extended control</param>
        /// <param name="text">Tool tip text</param>
        /// <returns></returns>
        public static void ShowUsYaTips(this Control control, string text)
        {
            // creates a new tool tip object
            ToolTip toolTip = new ToolTip
            {
                // sets the tool tip shape to a rectangle instead of a balloon
                IsBalloon = false,
                // enables tool tip even if parent control is not active
                ShowAlways = true,
                // sets the period of time that the tool tip is shown for
                AutoPopDelay = 2500,
                // sets the period of time that the mouse pointer must remain stationary for before the tool tip is shown
                InitialDelay = 0,
                // enables tool tip animation effect
                UseAnimation = true,
                // enables tool tip fading effect
                UseFading = true
            };

            // creates a tooltip with the above settings at the passed control with the passed text
            toolTip.SetToolTip(control, text);

        } // end void
        #endregion

        #region Logger
        /// <summary>
        /// Logs errors to report lists or console window
        /// </summary>
        public static class Logger
        {
            /// <summary>
            /// Stores errors to be written to a log file later
            /// </summary>
            public static List<string> errorReportList = new List<string>();

            #region Log(string message)
            /// <summary>
            /// Writes passed message to the console
            /// </summary>
            /// <param name="message">Message to write to console</param>
            public static void Log(string message)
            {
                // writes passed message to the console
                Console.WriteLine(message);
            }
            #endregion

            #region Log(message)
            /// <summary>
            /// Writes passed error message to the console along with passed exception message
            /// </summary>
            /// <param name="errorMessage">Error message to write to the console</param>
            /// <param name="exMessage">Exception message to write to the console</param>
            public static void Log(string errorMessage, string exMessage)
            {
                // writes error to console window along with exception message
                Console.WriteLine("Error: " + errorMessage + ". Exception: " + exMessage);

            } // end void
            #endregion

            #region LogError(string errorMessage)
            /// <summary>
            /// Writes error message to console window and error report list
            /// </summary>
            public static void LogError(string errorMessage)
            {
                try
                {
                    // writes error to error report
                    errorReportList.Add("[" + DateTime.Now + "] " + errorMessage);
                    // writes line to console window on its own line
                    Console.WriteLine("Error: " + errorMessage);
                }
                catch (Exception ex)
                {
                    // write error to console
                    Log(ex.Message);

                } // end try

            } // end void
            #endregion

            #region LogError(string errorMessage, string exMessage)
            /// <summary>
            /// Writes error message to error report list and console window along with passed exception message
            /// </summary>
            /// <param name="errorMessage">Error message to write to the console</param>
            /// <param name="exMessage">Exception message to write to the console</param>
            public static void LogError(string errorMessage, string exMessage)
            {
                try
                {
                    // writes error to error report along with exception message
                    errorReportList.Add("[" + DateTime.Now + "] " + errorMessage + ". Exception: " + exMessage);
                    // writes error to console window along with exception message
                    Console.WriteLine("Error: " + errorMessage + ". Exception: " + exMessage);
                }
                catch (Exception ex)
                {
                    // write error to console
                    Log("Failed to log error!", ex.Message);

                } // end try

            } // end void
            #endregion

        } // end class

        #endregion

    } // end class

} // end namespace
