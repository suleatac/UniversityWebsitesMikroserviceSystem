namespace Mikroservice.Personel.Api
{
    public static class DateTimeOffsetHelper
    {
        public static void ConvertToUtc<T>(T obj)
        {
            if (obj == null)
                return;

            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(DateTimeOffset?) ||
                    prop.PropertyType == typeof(DateTimeOffset))
                {
                    var value = prop.GetValue(obj);

                    if (value != null)
                    {
                        var dto = (DateTimeOffset)value;
                        prop.SetValue(obj, dto.ToUniversalTime());
                    }
                }
            }
        }
    }
}
