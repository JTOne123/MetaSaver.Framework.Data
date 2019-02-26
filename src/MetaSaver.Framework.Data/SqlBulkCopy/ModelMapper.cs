using System.Collections.Generic;
using System.Data;
using System.Linq;

// Modified from a post by Dejan Stojanovic
// https://dejanstojanovic.net/aspnet/2016/january/sqlbulkcopy-with-model-classes-in-c/

// ReSharper disable once CheckNamespace
namespace MetaSaver.Framework.Data
{
    public static class ModelMapper
    {
        public static DataTable MapModel<T>(IEnumerable<T> models, string tableName) where T : class, new()
        {
            var enumerable = models as IList<T> ?? models.ToList();
            if (!enumerable.Any())
                return null;

            var result = new DataTable(tableName);
            var propertyInfos = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ModelMapAttribute), true)
                    .Any())
                .ToList();

            //Create columns
            foreach (var propertyInfo in propertyInfos)
            {
                var attribute = propertyInfo.GetCustomAttributes(typeof(ModelMapAttribute), true)
                    .First() as ModelMapAttribute;

                result.Columns.Add(!string.IsNullOrWhiteSpace(attribute?.ColumnName)
                    ? attribute.ColumnName
                    : propertyInfo.Name);
            }

            //Fill the data
            foreach (var model in enumerable)
            {
                var matchCount = 0;
                var row = result.NewRow();

                foreach (var propertyInfo in propertyInfos)
                {
                    var attribute = propertyInfo.GetCustomAttributes(typeof(ModelMapAttribute), true)
                        .First() as ModelMapAttribute;

                    var value = propertyInfo.GetValue(model);

                    row[!string.IsNullOrWhiteSpace(attribute?.ColumnName)
                        ? attribute.ColumnName
                        : propertyInfo.Name] = value;

                    if (value != null)
                        matchCount++;
                }

                //Skip empty models
                if (matchCount > 0)
                    result.Rows.Add(row);
            }

            result.AcceptChanges();

            return result;
        }
    }
}