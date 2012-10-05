//Windows Auto Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//My Imports
using System.IO;

//3rd Party Imports

namespace Restart
{
    /**
     * Configuration Manager.  Stores the data of the configuration.  Requires "config.txt" to exist
     */
    public class Config
    {
        private static Trikey options = null;

        /**
         * Initializes the Cofig Manager 
         */
        public static void init()
        {
            if (options != null)
            {
                Console.WriteLine("ERROR: Config.init() : Config has already been initialized..");
                return;
            }

            int lineNumber = 0;
            StreamReader reader = null;
            string temp = null;

            try
            {
                reader = new StreamReader("config.txt");
            }
            catch(FileNotFoundException fnfe)
            {
                MainMethod.die("Error: Config.init() : Unable to find Config file. Exception Method: " + fnfe.Message);
            }

            options = new Trikey();

            while (reader.Peek() > -1)
            {
                temp = getNewLine(reader, ref lineNumber);

                if (String.IsNullOrWhiteSpace(temp) || temp.StartsWith("//") || temp.StartsWith("#"))
                {
                    continue;
                }

                if(!temp.Contains("section"))
                {
                    reader.Close();
                    MainMethod.die("Error: Config.init() : Line number: \"" + lineNumber + "\" is out of place. It does not contain a comment or section start tag. Line Contents:\n" + temp);
                }

                string[] pieces = temp.Split(' ');

                if (!pieces[0].Equals("section"))
                {
                    reader.Close();
                    MainMethod.die("Error: Config.init : Line + \"" + lineNumber + "\" is out of order. \"section\" should be first string a in line. Line Contents:\n" + temp);
                }

                if (pieces.Length == 2)
                {
                    options.addSection(pieces[1]);
                    parseSection(reader, pieces[1], ref lineNumber);
                }
                else
                {
                    if (pieces.Length == 1)
                    {
                        reader.Close();
                        MainMethod.die("Error: Config.init() : Section must be named.");
                    }
                    else
                    {
                        reader.Close();
                        MainMethod.die("Error: Config.init() : This line has to many words on it. Line Number: \"" + lineNumber + "\".");
                    }

                }
            }//End WHile loop

            reader.Close();
        
        }//End Method

        /**
         * Get the string value of the given option from the given section
         * 
         * @param sectionName - string - Name of the Section to pull from.
         * @param optionName - string - Name of the option to retrieve.
         * 
         * @return - string - The string value of the given option
         */
        public static string getValue(string sectionName, string optionName)
        {
            //Trikey handles the null checks for us
            return options.getValue(sectionName, optionName);
        }

        /**
         * Gets the given option from the given section and tries to convert it a bool
         * 
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The option to retrieve
         * 
         * @return - bool - The option converted to a bool value
         */
        public static bool convertSettingToBool(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);
            temp = temp.ToLower();

            if(temp.Equals("true") || temp.Equals("yes") || temp.Equals("t"))
            {
                return true;
            }
            else if (temp.Equals("false") || temp.Equals("no") || temp.Equals("f"))
            {
                return false;
            }
            else
            {
                MainMethod.die("Error : convertSettingToBool : Could not convert: " + temp + " to bool.  Section: " + sectionName + " Option: " + optionName + ".");
            }

            return false;
        }

        /**
         * Gets the given option from the given section and tries to convert it a char
         * 
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The option to retrieve
         * 
         * @return - char - The option converted to a char value
         */
        public static char convertSettingToChar(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);

            if (temp.Length != 1)
            {
                MainMethod.die("Error : convertSettingToChar() : Could not convert " + temp + " to char. has to many letters. Section: " + sectionName + " Option: " + optionName);
            }

            return temp[0];
        }

        /**
         * Gets the given option from the given section and tries to convert it a byte
         * 
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The option to retrieve
         * 
         * @return - byte - The option converted to a byte value
         */
        public static byte convertSettingToByte(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);
            byte value = 0;

            try
            {
                value = Convert.ToByte(temp);
            }
            catch (FormatException fe)
            {
                MainMethod.die("Error: Config.ConvertSettingToShort() : A Format exception has occured. Exception Message: " + fe.Message);
            }

            return value;
        }

        /**
         * Gets the given option from the given section and tries to convert it a short
         * 
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The option to retrieve
         * 
         * @return - short - The option converted to a short value
         */
        public static short convertSettingToShort(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);
            short value = 0;

            try
            {
                value = Convert.ToInt16(temp);
            }
            catch (FormatException fe)
            {
                MainMethod.die("Error: Config.ConvertSettingToShort() : A Format exception has occured. Exception Message: " + fe.Message); 
            }

            return value;
        }

        /**
         * Gets the given option from the given section and tries to convert it a int.
         * 
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The option to retrieve
         * 
         * @return - int - The option converted to a int value
         */
        public static int convertSettingToInt(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);
            int value = 0;

            try
            {
                value = Convert.ToInt32(temp);
            }
            catch (FormatException fe)
            {
                MainMethod.die("Error: Config.ConvertSettingToInt() : A Format exception has occured. Exception Message: " + fe.Message);
            }

            return value;
        }

        /**
         * Gets the given option from the given section and tries to convert it a long
         * 
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The option to retrieve
         * 
         * @return - long - The option converted to a long value
         */
        public static long convertSettingToLong(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);
            long value = 0;

            try
            {
                value = Convert.ToInt64(temp);
            }
            catch (FormatException fe)
            {
                MainMethod.die("Error: Config.ConvertSettingToLong() : A Format exception has occured. Exception Message: " + fe.Message);
            }

            return value;
        }

        /**
         * Gets the given option from the given section and tries to convert it a decimal
         *  
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The option to retrieve
         * 
         * @return - decimal - The option converted to a decimal value
         */
        public static decimal convertSettingToDecimal(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);
            decimal value = 0;

            try
            {
                value = Convert.ToDecimal(temp);
            }
            catch (FormatException fe)
            {
                MainMethod.die("Error: Config.ConvertSettingToShort() : A Format exception has occured. Exception Message: " + fe.Message);
            }

            return value;
        }

        public static float convertSettingToFloat(string sectionName, string optionName)
        {
            string temp = getValue(sectionName, optionName);
            float value = 0;

            try
            {
                value = float.Parse(temp);
            }
            catch (FormatException fe)
            {
                MainMethod.die("Error: Config.ConvertSettingToShort() : A Format exception has occured. Exception Message: " + fe.Message);
            }

            return value;
        }

        /**
         * Uninitializes the Config. Currently Does nothing
         */
        public static void uninit()
        {//Nothing to do here for now
        }

        /**
         * Parse a section of the config file.
         * 
         * @param reader - StreamReader - Reader for the Config file
         * @param sectionName - string - Name of the section that is about to parsed
         * @param lineNumber - ref-int - The Current line number.
         */
        private static void parseSection(StreamReader reader, string sectionName, ref int lineNumber)
        {
            string temp = getNewLine(reader, ref lineNumber);

            if (temp == "{")
            {
                temp = getNewLine(reader, ref lineNumber);
                while (temp != null && temp != "}")
                {
                    if (String.IsNullOrWhiteSpace(temp) || temp.StartsWith("//") || temp.StartsWith("#"))
                    {
                        temp = getNewLine(reader, ref lineNumber);
                        continue;
                    }

                    string[] pieces = temp.Split(' ');

                    if (pieces.Length == 2)
                    {
                        options.addOption(sectionName, pieces[0], pieces[1]);
                    }
                    else
                    {
                        reader.Close();
                        MainMethod.die("Error: parseSection : To many words on line: \"" + lineNumber + "\". Line looks like: " + temp);
                    }

                    temp = getNewLine(reader, ref lineNumber);
                }
                if (temp == null)
                {
                    MainMethod.die("Error: Config.parseSection() : Reached the end of the file while parsing section " + sectionName);
                }
            }
            else
            {
                reader.Close();
                MainMethod.die("Error: parseSection() : Section heeaders must be followed by a \"{\" on the following line. Section Name: \"" + sectionName + "\" and on Line: \"" + (lineNumber - 1) + "\"."); 
            }
        }

        private static string getNewLine(StreamReader reader, ref int lineNumber)
        {
            string temp = reader.ReadLine();
            temp = temp.Trim();
            lineNumber++;
            return temp;
        }

        public static void DebugPrint()
        {
            options.DebugPrintElements();
        }
    }
}
