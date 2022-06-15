using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartPackager.Automatic
{
    internal static class FastGetSetValue
    {
        public delegate void Setter<TContainer, TField>(ref TContainer destination, TField getValue);
        public delegate TField Getter<TContainer, TField>(TContainer destination);

        //https://stackoverflow.com/questions/17660097/is-it-possible-to-speed-this-method-up/17669142

        public static Getter<TContainer, TField> BuildUntypedGetter<TContainer, TField>(MemberInfo memberInfo)
        {
            var targetType = memberInfo.DeclaringType;
            var exInstance = Expression.Parameter(targetType, "t");

            var exMemberAccess = Expression.MakeMemberAccess(exInstance, memberInfo);       // t.PropertyName
            //var exConvertToObject = Expression.Convert(exMemberAccess, typeof(TField));     // Convert(t.PropertyName, typeof(object))
            var lambda = Expression.Lambda<Getter<TContainer, TField>>(exMemberAccess, exInstance);

            var action = lambda.Compile();
            return action;
        }

        //можно ускорить с помошью   (хз почему но работать стало медленнее в 10 раз)
        //s.GetType().GetField("Field").SetValueDirect(__makeref(s), 5);
        //https://stackoverflow.com/questions/6280506/is-there-a-way-to-set-properties-on-struct-instances-using-reflection

        public static Setter<TContainer, TField> BuildUntypedSetter<TContainer, TField>(MemberInfo memberInfo)
        {
            var targetType = memberInfo.DeclaringType;
            var exInstance = Expression.Parameter(targetType.MakeByRefType(), "t");

            var exMemberAccess = Expression.MakeMemberAccess(exInstance, memberInfo);

            // t.PropertValue(Convert(p))
            var exValue = Expression.Parameter(typeof(TField), "p");
            //var exConvertedValue = Expression.Convert(exValue, GetUnderlyingType(memberInfo));
            var exBody = Expression.Assign(exMemberAccess, exValue);

            var lambda = Expression.Lambda<Setter<TContainer, TField>>(exBody, exInstance, exValue);
            var action = lambda.Compile();
            return action;
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    ("Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
            }
        }
    }
}
