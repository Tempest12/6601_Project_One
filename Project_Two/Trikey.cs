//Windows Auto Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//My Imports
using System.Collections;

//3rd Party Libraries

namespace Restart
{

    /**
     * Data Structure for the configuration manager. Essentially a double Hash table. (Hash table inside of a Hash table.
     */
    public class Trikey
    {
        private Hashtable table;

        /**
         * Constructor. Takes in nothing
         */
        public Trikey()
        {
            table = new Hashtable();
        }

        /**
         * Adds a section(Hash Table).
         * 
         * @param sectionName - string - The name of the new section to add
         */ 
        public void addSection(string sectionName)
        {
            if (sectionName == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                table.Add(sectionName, new Hashtable());
            }
        }

        /**
         * Adds an option to the named section.
         * 
         * @param sectionName - string - The section to look for.
         * @param optionName - string - The name of the Option to add
         * @param value - string - The value to give the option
         */
        public void addOption(string sectionName, string optionName, string value)
        {
            if (sectionName == null)
            {
                throw new NullReferenceException();
            }
            if(optionName == null)
            {
                throw new NullReferenceException();
            }
            if(value == null)
            {
                throw new NullReferenceException();
            }

            Hashtable temp = (Hashtable)table[sectionName];

            if (temp == null)
            {
                MainMethod.die("Error: Trikey.addOption() : section: " + sectionName + " does not exist.");
            }

            temp.Add(optionName, value);
        }

        /**
         * Gets a (string) value from hashtables.
         * 
         * @param sectionName - string - Section to look for.
         * @param optionName - string - Option to look for.
         * 
         * @return string - The string value of the given option
         */
        public string getValue(string sectionName, string optionName)
        {
            if(sectionName == null)
            {
                throw new NullReferenceException();
            }
            if (optionName == null)
            {
                throw new NullReferenceException();
            }

            Hashtable temp = (Hashtable)table[sectionName];

            if (temp == null)
            {
                MainMethod.die("Error : Trikey.getValue : Section \"" + sectionName + "\" does not exist.");
            }

            string value = (string)temp[optionName];

            if (value == null)
            {
                MainMethod.die("Error : Trikey.getValue : Option: \"" + optionName + "\" from section: \"" + sectionName + "\" does not exist.");
            }

            return value;
        }


        /**
         * Debug method that prints the content of the Trikey.
         */
        public void DebugPrintElements()
        {
            foreach (string outerKey in table.Keys)
            {
                Hashtable temp = (Hashtable)table[outerKey];
                Console.WriteLine("section " + outerKey);

                foreach (string innerKey in temp.Keys)
                {
                    Console.WriteLine("\t" + innerKey + " " + (string)temp[innerKey]);
                }
            }
        }
    }
}
