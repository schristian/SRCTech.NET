using System;
using System.Threading.Tasks;

namespace SRCTech.Common.Functional
{
    public readonly struct Unit : IEquatable<Unit>
    {
        public static Unit Default { get; } = default;

        public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(Default);

        public bool Equals(Unit other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "()";
        }

        public static bool operator ==(Unit first, Unit second)
        {
            return true;
        }

        public static bool operator !=(Unit first, Unit second)
        {
            return false;
        }
    }
}
