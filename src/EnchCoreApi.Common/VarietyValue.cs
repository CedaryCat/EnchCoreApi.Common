using Microsoft.Xna.Framework;

namespace EnchCoreApi.Common
{
    public struct VarietyValue : IEquatable<int>, IEquatable<float>, IEquatable<double>, IEquatable<Vector2>, IEquatable<bool>
    {
        private enum ValueType
        {
            num,
            vector
        }
        public static VarietyValue Zero = new VarietyValue(0);
        private readonly double Value;
        private Vector2 Vector2;
        private readonly ValueType type;
        public VarietyValue(bool value) {
            Value = value.GetHashCode();
            Vector2 = Vector2.Zero;
            type = ValueType.num;
        }
        public VarietyValue(double value) {
            Value = value;
            Vector2 = Vector2.Zero;
            type = ValueType.num;
        }
        public VarietyValue(float value) {
            Value = value;
            Vector2 = Vector2.Zero;
            type = ValueType.num;
        }
        public VarietyValue(Vector2 vector) {
            Value = 0;
            Vector2 = vector;
            type = ValueType.num;
        }
        public bool Equals(int other) {
            return other == Value;
        }

        public bool Equals(float other) {
            return other == Value;
        }

        public bool Equals(double other) {
            return other == Value;
        }

        public bool Equals(Vector2 other) {
            return Vector2 == other;
        }

        public bool Equals(bool other) {
            return other == this;
        }

        public static VarietyValue operator -(VarietyValue me) {
            if (me.type == ValueType.num)
                return new VarietyValue(-me.Value);
            else
                return new VarietyValue(-me.Vector2);
        }
        public static VarietyValue operator +(VarietyValue me) {
            if (me.type == ValueType.num)
                return new VarietyValue(me.Value);
            else
                return new VarietyValue(me.Vector2);
        }
        public static VarietyValue operator +(VarietyValue right, VarietyValue left) {
            if (right.type != left.type)
                throw new Exception();
            if (right.type == ValueType.num)
                return new VarietyValue(right.Value + left.Value);
            else
                return new VarietyValue(right.Vector2 + left.Vector2);
        }
        public static VarietyValue operator -(VarietyValue right, VarietyValue left) {
            if (right.type != left.type)
                throw new Exception();
            if (right.type == ValueType.num)
                return new VarietyValue(right.Value - left.Value);
            else
                return new VarietyValue(right.Vector2 - left.Vector2);
        }
        public static VarietyValue operator *(VarietyValue right, VarietyValue left) {
            if (right.type == left.type) {
                if (right.type == ValueType.vector)
                    return new VarietyValue(right.Vector2 * left.Vector2);
                else
                    return new VarietyValue(right.Value * left.Value);
            }
            else {
                if (right.type == ValueType.vector)
                    return new VarietyValue(right.Vector2 * (float)left.Value);
                else
                    return new VarietyValue((float)right.Value * left.Vector2);
            }
        }
        public static VarietyValue operator /(VarietyValue right, VarietyValue left) {
            if (right.type == left.type) {
                if (right.type == ValueType.vector)
                    throw new Exception();
                else
                    return new VarietyValue(right.Value / left.Value);
            }
            else {
                if (right.type == ValueType.vector)
                    return new VarietyValue(right.Vector2 / (float)left.Value);
                else
                    throw new Exception();
            }
        }
        public static VarietyValue operator %(VarietyValue right, VarietyValue left) {
            if (right.type != left.type)
                throw new Exception();
            if (right.type == ValueType.num)
                return new VarietyValue(right.Value % left.Value);
            else
                throw new Exception();
        }

        public static implicit operator double(VarietyValue input) {
            return input.Value;
        }
        public static implicit operator int(VarietyValue input) {
            return (int)input.Value;
        }
        public static implicit operator float(VarietyValue input) {
            return (float)input.Value;
        }
        public static implicit operator bool(VarietyValue input) {
            return input.Value != 0;
        }

        public static implicit operator VarietyValue(int input) {
            return new VarietyValue(input);
        }
        public static implicit operator VarietyValue(float input) {
            return new VarietyValue(input);
        }
        public static implicit operator VarietyValue(double input) {
            return new VarietyValue(input);
        }
        public static implicit operator VarietyValue(bool input) {
            return new VarietyValue(input);
        }

        public static implicit operator Vector2(VarietyValue input) {
            return input.Vector2;
        }

        public static implicit operator VarietyValue(Vector2 input) {
            return new VarietyValue(input);
        }
    }
}
