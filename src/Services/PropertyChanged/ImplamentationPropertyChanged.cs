using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;
using System;

namespace Services.PropertyChanged
{
    public class ImplementationPropertyChanged : INotifyPropertyChanged
    {
        #region Properties
        

        #endregion Properties

        #region Events
        
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        public void OnPropertyChanged([CallerMemberName]string PropertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));

        public virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> Property) => OnPropertyChanged(GetMemberInfo(Property).Name);

        private static MemberInfo GetMemberInfo(Expression Expression)
        {
            LambdaExpression Lambda = Expression as LambdaExpression;

            MemberExpression ME =
            (Lambda.Body is UnaryExpression)
            ?
            (Lambda.Body as UnaryExpression).Operand as MemberExpression
            :
            Lambda.Body as MemberExpression;

            return ME.Member;
        }

        #endregion Methods
    }
}