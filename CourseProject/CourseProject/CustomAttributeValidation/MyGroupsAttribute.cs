using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.CustomAttributeValidation
{
    public class MyGroupsAttribute : ValidationAttribute
    {
        private static string[] myGroups;

        public MyGroupsAttribute(string[] Groups)
        {
            myGroups = Groups;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string strval = value.ToString();
                for (int i = 0; i < myGroups.Length; i++)
                {
                    if (strval == myGroups[i])
                        return true;
                }
            }
            return false;
        }
    }
}