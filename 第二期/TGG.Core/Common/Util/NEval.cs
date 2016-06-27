using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Common.Util
{
    /// <summary>
    /// 表达式计算器
    /// </summary>
    public class NEval
    {
        public double Eval(string expr)
        {
            try
            {
                var tmpexpr = expr.ToLower().Trim().Replace(" ", string.Empty);
                return Calc_Internal(tmpexpr);
            }
            catch
            {
                throw new Exception("表达式错误");
            }
        }
        private Random m_Random;
        private double Calc_Internal(string expr)
        {
            /*             
             * 1.    初始化一个空堆栈              
             * 2.    从左到右读入后缀表达式              
             * 3.    如果字符是一个操作数，把它压入堆栈。              
             * 4.    如果字符是个操作符，弹出两个操作数，执行恰当操作，然后把结果压入堆栈。如果您不能够弹出两个操作数，后缀表达式的语法就不正确。              
             * 5.    到后缀表达式末尾，从堆栈中弹出结果。若后缀表达式格式正确，那么堆栈应该为空。           
             */
            var post2 = ConvertExprBack(expr);
            var post = new Stack();
            while (post2.Count > 0)
                post.Push(post2.Pop());
            var stack = new Stack();
            while (post.Count > 0)
            {
                var tmpstr = post.Pop().ToString();
                var c = tmpstr[0];
                var lt = JudgeLetterType(tmpstr);
                if (lt == LetterType.Number)
                {
                    stack.Push(tmpstr);
                }
                else if (lt == LetterType.SimpleOperator)
                {
                    var d1 = double.Parse(stack.Pop().ToString());
                    var d2 = double.Parse(stack.Pop().ToString());
                    double r = 0;
                    if (c == '+')
                        r = d2 + d1;
                    else if (c == '-')
                        r = d2 - d1;
                    else if (c == '*')
                        r = d2 * d1;
                    else if (c == '/')
                        r = d2 / d1;
                    else if (c == '^')
                        r = Math.Pow(d2, d1);
                    else
                        throw new Exception("不支持操作符:" + c.ToString());
                    stack.Push(r);

                }
                else if (lt == LetterType.Function)
                //如果是函数                
                {
                    string[] p;
                    double d = 0;
                    double d1 = 0;
                    double d2 = 0;
                    var tmpos = tmpstr.IndexOf('(');
                    var funcName = tmpstr.Substring(0, tmpos);
                    switch (funcName)
                    {
                        case "asin":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Asin(d).ToString());
                            break;
                        case "acos":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Acos(d).ToString());
                            break;
                        case "atan":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Atan(d).ToString());
                            break;
                        case "acot":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push((1 / Math.Atan(d)).ToString());
                            break;
                        case "sin":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Sin(d).ToString());
                            break;
                        case "cos":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Cos(d).ToString());
                            break;
                        case "tan":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Tan(d).ToString());
                            break;
                        case "cot":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push((1 / Math.Tan(d)).ToString());
                            break;
                        case "log":
                            SplitFuncStr(tmpstr, 2, out p);
                            d1 = double.Parse(p[0]);
                            d2 = double.Parse(p[1]);
                            stack.Push(Math.Log(d1, d2).ToString());
                            break;
                        case "ln":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Log(d, Math.E).ToString());
                            break;
                        case "abs":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Abs(d).ToString());
                            break;
                        case "round":
                            SplitFuncStr(tmpstr, 2, out p);
                            d1 = double.Parse(p[0]);
                            d2 = double.Parse(p[1]);
                            stack.Push(Math.Round(d1, (int)d2).ToString());
                            break;
                        case "int":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push((int)d);
                            break;
                        case "trunc":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Truncate(d).ToString());
                            break;
                        case "floor":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Floor(d).ToString());
                            break;
                        case "ceil":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Ceiling(d).ToString());
                            break;
                        case "random":
                            if (m_Random == null)
                                m_Random = new Random();
                            d = m_Random.NextDouble();
                            stack.Push(d.ToString());
                            break;
                        case "exp":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Exp(d).ToString());
                            break;
                        case "pow":
                            SplitFuncStr(tmpstr, 2, out p);
                            d1 = double.Parse(p[0]);
                            d2 = double.Parse(p[1]);
                            stack.Push(Math.Pow(d1, d2).ToString());
                            break;
                        case "sqrt":
                            SplitFuncStr(tmpstr, 1, out p);
                            d = double.Parse(p[0]);
                            stack.Push(Math.Sqrt(d).ToString());
                            break;
                        default:
                            throw new Exception("未定义的函数：" + funcName);
                    }
                }
            }
            var obj = stack.Pop();
            return double.Parse(obj.ToString());
        }
        /// <summary>        
        /// /// 将函数括号内的字符串进行分割，获得参数列表，如果参数是嵌套的函数，用递归法计算得到它的值        
        /// /// </summary>       
        ///  /// <param name="funcstr"></param>       
        ///  /// <param name="paramCount"></param>       
        ///  /// <param name="parameters"></param>        
        private void SplitFuncStr(string funcstr, int paramCount, out string[] parameters)
        {
            parameters = new string[paramCount];
            var tmpPos = funcstr.IndexOf('(', 0);
            var str = funcstr.Substring(tmpPos + 1, funcstr.Length - tmpPos - 2);
            if (paramCount == 1)
            {
                parameters[0] = str;
            }
            else
            {
                var cpnum = 0; var startPos = 0; var paramIndex = 0;
                for (var i = 0; i <= str.Length - 1; i++)
                {
                    if (str[i] == '(') cpnum++;
                    else if (str[i] == ')') cpnum--;
                    else if (str[i] == ',')
                    {
                        if (cpnum != 0) continue;
                        var tmpstr = str.Substring(startPos, i - startPos); parameters[paramIndex] = tmpstr; paramIndex++; startPos = i + 1;
                    }
                }
                if (startPos < str.Length)
                {
                    var tmpstr = str.Substring(startPos);
                    parameters[paramIndex] = tmpstr;
                }
            }
            //如果参数是函数， 进一步采用递归的方法生成函数值           
            for (var i = 0; i <= paramCount - 1; i++)
            {
                double d;
                if (double.TryParse(parameters[i], out d)) continue;
                var calc = new NEval();
                d = calc.Eval(parameters[i]); parameters[i] = d.ToString();
            }
        }
        /// <summary>        
        /// 将中缀表达式转为后缀表达式        
        /// </summary>       
        /// <param name="expr"></param>       
        /// <returns></returns>       
        private Stack ConvertExprBack(string expr)
        {
            /*             
             * 新建一个Stack栈，用来存放运算符            
             * 新建一个post栈，用来存放最后的后缀表达式       
             * 从左到右扫描中缀表达式：             
             * 1.若读到的是操作数，直接存入post栈，以#作为数字的结束            
             * 2、若读到的是(,则直接存入stack栈             
             * 3.若读到的是），则将stack栈中(前的所有运算符出栈，存入post栈             
             * 4 若读到的是其它运算符，则将该运算符和stack栈顶运算符作比较：若高于或等于栈顶运算符， 则直接存入stack栈，             
             * 否则将栈顶运算符（所有优先级高于读到的运算符的，不包括括号）出栈，存入post栈。最后将读到的运算符入栈             
             * 当扫描完后，stack栈中还在运算符时，则将所有的运算符出栈，存入post栈             
             * */
            var post = new Stack();
            var stack = new Stack();
            string tmpstr;
            int pos;
            for (var i = 0; i <= expr.Length - 1; i++)
            {
                var c = expr[i]; var lt = JudgeLetterType(c, expr, i);
                switch (lt)
                {
                    case LetterType.Number:
                        GetCompleteNumber(expr, i, out tmpstr, out pos);
                        post.Push(tmpstr);
                        i = pos;
                        break;
                    case LetterType.OpeningParenthesis:
                        stack.Push(c);
                        break;
                    case LetterType.ClosingParenthesis:
                        while (stack.Count > 0)
                        {
                            if (stack.Peek().ToString() == "(")
                            {
                                stack.Pop(); break;
                            }
                            post.Push(stack.Pop());
                        }
                        break;
                    case LetterType.SimpleOperator:
                        if (stack.Count == 0)
                            stack.Push(c);
                        else
                        {
                            var tmpop = (char)stack.Peek();
                            if (tmpop == '(')
                            {
                                stack.Push(c);
                            }
                            else
                            {
                                if (GetPriority(c) >= GetPriority(tmpop)) { stack.Push(c); }
                                else
                                {
                                    while (stack.Count > 0)
                                    {
                                        var tmpobj = stack.Peek();
                                        if (GetPriority((char)tmpobj) > GetPriority(c))
                                        {
                                            if (tmpobj.ToString() != "(")
                                                post.Push(stack.Pop());
                                            else
                                                break;
                                        }
                                        else
                                            break;
                                    } stack.Push(c);
                                }
                            }
                        }
                        break;
                    case LetterType.Function:
                        GetCompleteFunction(expr, i, out tmpstr, out pos);
                        post.Push(tmpstr);
                        i = pos;
                        break;
                }
            } while (stack.Count > 0)
            { post.Push(stack.Pop()); }
            return post;
        }
        private LetterType JudgeLetterType(char c, string expr, int pos)
        {
            var op = "*/^";
            //操作数          
            if ((c <= '9' && c >= '0') || (c == '.'))
            {
                return LetterType.Number;
            }
            switch (c)
            {
                case '(':
                    return LetterType.OpeningParenthesis;
                case ')':
                    return LetterType.ClosingParenthesis;
                default:
                    if (op.IndexOf(c) >= 0)
                    {
                        return LetterType.SimpleOperator;
                    }
                    //要判断是减号还是负数   
                    if ((c != '-') && (c != '+')) return LetterType.Function;
                    if (pos == 0)
                        return LetterType.Number;
                    var tmpc = expr[pos - 1];
                    if (tmpc <= '9' && tmpc >= '0')
                        return LetterType.SimpleOperator;//如果前面一位是操作数 
                    if (tmpc == ')')
                        return LetterType.SimpleOperator;
                    return LetterType.Number;
            }
        }
        private LetterType JudgeLetterType(char c)
        {
            var op = "+-*/^";
            //操作数      
            if ((c <= '9' && c >= '0') || (c == '.'))
            {
                return LetterType.Number;
            }
            switch (c)
            {
                case '(':
                    return LetterType.OpeningParenthesis;
                case ')':
                    return LetterType.ClosingParenthesis;
                default:
                    return op.IndexOf(c) >= 0 ? LetterType.SimpleOperator : LetterType.Function;
            }
        }
        private LetterType JudgeLetterType(string s)
        {
            var c = s[0];
            if ((c == '-') || (c == '+'))
            {
                return s.Length > 1 ? LetterType.Number : LetterType.SimpleOperator;
            } var op = "+-*/^";
            //操作数
            if ((c <= '9' && c >= '0') || (c == '.'))
            {
                return LetterType.Number;
            }
            switch (c)
            {
                case '(':
                    return LetterType.OpeningParenthesis;
                case ')':
                    return LetterType.ClosingParenthesis;
                default:
                    return op.IndexOf(c) >= 0 ? LetterType.SimpleOperator : LetterType.Function;
            }
        }
        /// <summary>        
        /// 计算操作符的优先级        
        /// </summary>        
        /// <param name="c"></param>       
        /// <returns></returns>        
        private int GetPriority(char c)
        {
            if (c == '+' || c == '-') return 0;
            else if (c == '*') return 1;
            else if (c == '/')
                return 2;//除号优先级要设得比乘号高，否则分母可能会被先运算掉 
            else
                return 2;
        }
        /// <summary>        
        /// 获取完整的函数表达式        
        /// </summary>        
        /// <param name="expr"></param>       
        /// <param name="startPos"></param>       
        /// <param name="funcStr"></param>        
        /// <param name="endPos"></param>       
        private void GetCompleteFunction(string expr, int startPos, out string funcStr, out int endPos)
        {
            var cpnum = 0; for (var i = startPos; i <= expr.Length - 1; i++)
            {
                var c = expr[i];
                var lt = JudgeLetterType(c);
                if (lt == LetterType.OpeningParenthesis) cpnum++;
                else if (lt == LetterType.ClosingParenthesis)
                {
                    cpnum--;
                    //考虑到函数嵌套的情况，消除掉内部括号                 
                    if (cpnum != 0) continue;
                    endPos = i; funcStr = expr.Substring(startPos, endPos - startPos + 1);
                    return;
                }
            } funcStr = ""; endPos = -1;
        }

        /// <summary>        
        /// 获取到完整的数字        
        /// </summary>        
        /// <param name="expr"></param>       
        /// <param name="startPos"></param>        
        /// <param name="numberStr"></param>       
        /// <param name="endPos"></param>        
        private void GetCompleteNumber(string expr, int startPos, out string numberStr, out int endPos)
        {
            for (var i = startPos + 1; i <= expr.Length - 1; i++)
            {
                var tmpc = expr[i];
                if (JudgeLetterType(tmpc) == LetterType.Number) continue;
                endPos = i - 1;
                numberStr = expr.Substring(startPos, endPos - startPos + 1);
                return;
            } numberStr = expr.Substring(startPos); endPos = expr.Length - 1;
        }
    }

    /// <summary>    
    /// 字符类别    
    /// </summary>  
    public enum LetterType
    {
        Number,
        SimpleOperator,
        Function,
        OpeningParenthesis,
        ClosingParenthesis
    }
}
