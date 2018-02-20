using System;
using System.Collections.Generic;
using System.Data;

namespace IDataReaderExtensions
{
    public static class DataReaderExtensions
    {

        public static T Connect<T>(this T connection) where T : IDbConnection
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }

        public static T GetFieldValue<T>(this IDataReader r, string columnName, T dbNullValue = default(T))
        {
            return GetValue<T>(r, dbNullValue, columnName: columnName);
        }

        public static T GetOrdinalValue<T>(this IDataReader r, int ordinal, T dbNullValue = default(T))
        {
            return GetValue<T>(r, dbNullValue, ordinal: ordinal);
        }

        private static T GetValue<T>(IDataReader r, T dbNullValue, string columnName = null, int ordinal = 0)
        {
            try
            {
                if(columnName != null) ordinal = r.GetOrdinal(columnName);
                if (r.IsDBNull(ordinal)) return dbNullValue;
                var targetType = typeof(T);
                targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                var value = r.GetValue(ordinal);
                if (targetType.IsEnum) value = Enum.ToObject(targetType, value);
                return (T)value;
            }
            catch (Exception e)
            {
                throw new Exception($"unable to read column '{columnName}' as type '{typeof(T).FullName}'", e);
            }
        }

        public static IList<TResult> ExecuteReader<TResult>(this IDbCommand command, Func<IDataReader, TResult> fn)
        {
            using (var reader = command.ExecuteReader())
            {
                var collection = new List<TResult>();
                while (reader.Read())
                {
                    collection.Add(fn(reader));
                }
                return collection;
            }
        }
    }
}