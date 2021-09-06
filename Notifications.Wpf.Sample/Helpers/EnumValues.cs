using System;
using System.Linq;
using System.Windows.Markup;
using Notifications.Wpf.Annotations;

namespace Notification.Wpf.Sample.Helpers
{ 
    class EnumValues : MarkupExtension
    {
        private Type _Type;
        public Type Type
        {
            get => _Type;
            set
            {
                if (value != null && !value.IsEnum) throw new ArgumentException("Тип не является перечислением", nameof(value));
                _Type = value;
            }
        }

        public enum NullValueLocation { None, First, Last }

        public NullValueLocation NullValue { get; set; } = NullValueLocation.None;

        public EnumValues() { }

        public EnumValues([NotNull] Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (!type.IsEnum) throw new ArgumentException("Тип не является перечислением", nameof(type));
            Type = type;
        }

        public override object ProvideValue(IServiceProvider sp)
        {
            var values = _Type?.GetEnumValues();
            if (values is null) return null;
            switch (NullValue)
            {
                case NullValueLocation.None: return values;
                case NullValueLocation.First: return values.Cast<object>().AppendFirst(null).ToArray();
                case NullValueLocation.Last: return values.Cast<object>().AppendLast(null).ToArray();
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
