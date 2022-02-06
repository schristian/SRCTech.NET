using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SRCTech.Common
{
    public static class Guard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNull<T>(
            T paramValue,
            string paramName)
        {
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return paramValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<T> ThrowIfAnyItemsNull<T>(
            IReadOnlyCollection<T> paramValue,
            string paramName)
        {
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (paramValue.Any(it => it == null))
            {
                throw new ArgumentException($"Parameter '{paramName}' contains one or more null items.", paramName);
            }

            return paramValue;
        }
    }
}