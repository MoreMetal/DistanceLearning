namespace DistanceLearning.Services
{
    public class CalculatingService
    {
        // Универсальная операция
        private abstract class Operation
        {
            public abstract float Eval();
        }

        // Просто число
        private class Number : Operation
        {
            public Number(float f) { value = f; }
            public override float Eval() { return value; }

            private float value;
        }

        // Один операнд
        private abstract class Unary : Operation
        {
            public Unary(Operation op) { one = op; }

            protected Operation one;
        }

        // Два операнда
        private abstract class Binary : Operation
        {
            public Binary(Operation l, Operation r) { left = l; right = r; }

            protected Operation left, right;
        }

        // Одинокий минус
        private class Negation : Unary
        {
            public Negation(Operation n) : base(n) { }
            public override float Eval() { return -one.Eval(); }
        }

        // Сложение
        private class Plus : Binary
        {
            public Plus(Operation l, Operation r) : base(l, r) { }
            public override float Eval() { return left.Eval() + right.Eval(); }
        }

        // Вычитание
        private class Minus : Binary
        {
            public Minus(Operation l, Operation r) : base(l, r) { }
            public override float Eval() { return left.Eval() - right.Eval(); }
        }

        // Умножение
        private class Multiply : Binary
        {
            public Multiply(Operation l, Operation r) : base(l, r) { }
            public override float Eval() { return left.Eval() * right.Eval(); }
        }

        // Деление
        private class Divide : Binary
        {
            public Divide(Operation l, Operation r) : base(l, r) { }
            public override float Eval()
            {
                float right_eval = right.Eval();
                if (right_eval == 0.0f)
                    System.Console.WriteLine("Devide by zero");
                return (right_eval != 0.0f) ? (left.Eval() / right_eval) : float.MaxValue;
            }
        }

        public class Expression
        {
            public Expression(string s) { source = s; }

            public float CalculatingService()
            {
                pos = 0;
                Operation root = Parse0();
                return (root != null) ? root.Eval() : 0.0f;
            }

            // Низкий приоритет: сложение, вычитание
            private Operation Parse0()
            {
                Operation result = Parse1();

                for (; ; )
                {
                    if (Match('+')) result = new Plus(result, Parse1());
                    else if (Match('-')) result = new Minus(result, Parse1());
                    else return result;
                }
            }

            // Средний приоритет: умножение, деление
            private Operation Parse1()
            {
                Operation result = Parse2();
                for (; ; )
                {
                    if (Match('*')) result = new Multiply(result, Parse2());
                    else if (Match('/')) result = new Divide(result, Parse2());
                    else return result;
                }
            }

            // Высокий приоритет: одинокий минус, скобки, число
            private Operation Parse2()
            {
                Operation result = null;

                if (Match('-'))
                {
                    result = new Negation(Parse0());
                }
                else if (Match('('))
                {
                    result = Parse0();
                    if (!Match(')'))
                        System.Console.WriteLine("Missing ')'");
                }
                else
                {
                    // распарсим число
                    float val = 0.0f;
                    int start = pos;
                    while (pos < source.Length && (char.IsDigit(source[pos]) || source[pos] == '.' || source[pos] == ',' || source[pos] == 'e')) ++pos;

                    try { val = float.Parse(source.Substring(start, pos - start)); }
                    catch { System.Console.WriteLine("Can't parse '" + source.Substring(start) + "'"); }
                    result = new Number(val);

                }
                return result;
            }

            // Поищем символ в строке
            private bool Match(char ch)
            {
                if (pos >= source.Length) return false;
                while (source[pos] == ' ') ++pos;             // пропустим пробелы

                if (source[pos] == ch) { ++pos; return true; } // нашли что искали?
                else return false;
            }

            private string source;     // исходная строчка
            private int pos;        // текущая позиция
        }

        public static double Result(string str)
        {
            Expression expr = new Expression(str);

            return expr.CalculatingService();
        }
    }
}
