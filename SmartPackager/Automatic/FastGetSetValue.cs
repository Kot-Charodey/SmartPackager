using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartPackager.Automatic
{
    internal static class FastGetSetValue
    {
        //https://stackoverflow.com/questions/17660097/is-it-possible-to-speed-this-method-up/17669142

        public static Func<TContainer, TField> BuildUntypedGetter<TContainer, TField>(MemberInfo memberInfo)
        {
            var targetType = memberInfo.DeclaringType;
            var exInstance = Expression.Parameter(targetType, "t");

            var exMemberAccess = Expression.MakeMemberAccess(exInstance, memberInfo);       // t.PropertyName
            //var exConvertToObject = Expression.Convert(exMemberAccess, typeof(TField));     // Convert(t.PropertyName, typeof(object))
            var lambda = Expression.Lambda<Func<TContainer, TField>>(exMemberAccess, exInstance);

            var action = lambda.Compile();
            return action;
        }

        public static Action<TContainer, TField> BuildUntypedSetter<TContainer, TField>(MemberInfo memberInfo)
        {
            var targetType = memberInfo.DeclaringType;
            var exInstance = Expression.Parameter(targetType, "t");

            var exMemberAccess = Expression.MakeMemberAccess(exInstance, memberInfo);

            // t.PropertValue(Convert(p))
            var exValue = Expression.Parameter(typeof(TField), "p");
            //var exConvertedValue = Expression.Convert(exValue, GetUnderlyingType(memberInfo));
            var exBody = Expression.Assign(exMemberAccess, exValue);

            var lambda = Expression.Lambda<Action<TContainer, TField>>(exBody, exInstance, exValue);
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
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }
}
