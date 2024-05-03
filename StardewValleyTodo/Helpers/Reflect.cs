using System;
using System.Reflection;

namespace StardewValleyTodo.Helpers {
    public static class Reflect {
        public static T GetPrivate<T>(object target, string field) {
            var getter = target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);

            if (getter == null) {
                throw new ArgumentException(
                    $"There is no '{field}' in {target.GetType().Name} or it is not non-public");
            }

            return (T) getter.GetValue(target);
        }
    }
}
