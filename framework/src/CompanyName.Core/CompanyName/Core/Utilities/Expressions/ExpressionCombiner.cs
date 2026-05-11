using System.Linq.Expressions;


namespace CompanyName.Core.Utilities.Expressions
{
    public static class ExpressionCombiner
    {
        public static Expression<Func<T, bool>> And<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return CombineExpressions(left, right, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return CombineExpressions(left, right, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> Not<T>(
            Expression<Func<T, bool>> expression)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var visitor = new ReplaceParameterVisitor(expression.Parameters[0], parameter);
            var body = visitor.Visit(expression.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(body!), parameter);
        }

        private static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right,
            Func<Expression, Expression, BinaryExpression> combiner)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var leftVisitor = new ReplaceParameterVisitor(left.Parameters[0], parameter);
            var rightVisitor = new ReplaceParameterVisitor(right.Parameters[0], parameter);

            var leftBody = leftVisitor.Visit(left.Body);
            var rightBody = rightVisitor.Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(
                combiner(leftBody!, rightBody!), parameter);
        }
    }
}

