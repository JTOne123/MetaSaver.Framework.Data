using System;

// Modified from a post by Dejan Stojanovic
// https://dejanstojanovic.net/aspnet/2016/january/sqlbulkcopy-with-model-classes-in-c/

// ReSharper disable once CheckNamespace
namespace MetaSaver.Framework.Data
{
    #region Class

    [AttributeUsage(AttributeTargets.Property)]
    public class ModelMapAttribute : Attribute
    {
        #region Locals

        public string ColumnName { get; set; }

        #endregion

        #region ModelMapAttribute

        public ModelMapAttribute(string columnName = null)
        {
            ColumnName = string.IsNullOrWhiteSpace(columnName) ? null : columnName;
        }

        #endregion
    }

    #endregion
}
