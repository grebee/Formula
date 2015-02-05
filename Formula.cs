// Skeleton written by Joe Zachary for CS 3500, January 2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        private String formula_var; 
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using standard C# syntax for double/int literals), 
        /// variable symbols (one or more letters followed by one or more digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// An example of a valid parameter to this constructor is "2.5e9 + x5 / 17".
        /// Examples of invalid parameters are "x", "-5.3", and "2 5 + 3";
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            formula_var = formula;
            IEnumerable<String> formula_array = new String[formula_var.Length];
             formula_array = GetTokens(formula_var);
            if (!checking(formula_array))
            {
                Console.WriteLine("Error");
                Console.ReadLine();
            }
        }

        private bool checking(IEnumerable<String> formula_array)
        {
            // Second requirement
            int left_par = 0;
            int right_par = 0;
            String[] for_array = formula_array.ToArray();
            if (for_array.Length < 1)
                return false;
            // Third & Fourth requirement
            foreach (string s in for_array)
            {
                if (right_par < left_par)
                    return false;
                if (s == '('.ToString())
                    left_par++;
                if (s == ')'.ToString())
                    right_par++;
            }
            if (right_par != left_par)
                return false;
            // Fifth requirement

            int number;
            if (!(int.TryParse(for_array[0], out number) || for_array[0] == '('.ToString() || isvariable(for_array[0])))
                return false;

            // Sixth requirement
            if (!(int.TryParse(for_array[for_array.Length-1], out number) || for_array[for_array.Length-1] == ')'.ToString() || isvariable(for_array[for_array.Length-1])))
                return false;
            
            // Seventh and Eigth requirement
            String opPattern = @"[\+\-*/]";
            Regex opRegex = new Regex(opPattern);
            for (int i = 0; i <for_array.Length; i++)
            {
                if(for_array[i] == '('.ToString() || opRegex.IsMatch(for_array[i]) )
                    if (!(int.TryParse(for_array[i+1], out number) || for_array[i+1] == '('.ToString() || isvariable(for_array[i+1])))
                        return false;
                
                if (int.TryParse(for_array[i], out number) || for_array[i] == ')'.ToString() || isvariable(for_array[i]))
                    if(!(opRegex.IsMatch(for_array[i+1]) || for_array[i+1] == ')'.ToString()))
                        return false;    
            }
        }
        private bool isvariable(String variable)
        {
            String varPattern = @"[a-zA-Z]+\d+";
            Regex varRegex = new Regex(varPattern);
            if (!varRegex.IsMatch(variable))
                return false;
            return true;
        }
  
        

        /// <summary>
        /// A Lookup function is one that maps some strings to double values.  Given a string,
        /// such a function can either return a double (meaning that the string maps to the
        /// double) or throw an ArgumentException (meaning that the string is unmapped.
        /// Exactly how a Lookup function decides which strings map to doubles and which
        /// don't is up to the implementation of that function.
        /// </summary>
        public delegate double Lookup(string s);

        /// <summary>
        /// Evaluates this Formula, using lookup to determine the values of variables.  
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, throw a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            return 0;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of one or more
        /// letters followed by one or more digits, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z]+\d+";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message)
            : base(message)
        {
        }
    }
}
