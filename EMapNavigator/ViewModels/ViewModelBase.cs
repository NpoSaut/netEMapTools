using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace EMapNavigator.ViewModels
{
    public class ViewModelBase : DispatcherObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String PropertyName)
        {
            VerifyPropertyName(PropertyName);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        [System.Diagnostics.Conditional("DEBUG")]
        [System.Diagnostics.DebuggerStepThrough]
        protected void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real, 
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                throw new Exception(msg);
            }
        }

        protected void OnPropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> property)
            where T : ViewModelBase
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var expression = property.Body as System.Linq.Expressions.MemberExpression;
                if (expression == null)
                {
                    throw new NotSupportedException("Invalid expression passed. Only property member should be selected.");
                }

                handler(this, new PropertyChangedEventArgs(expression.Member.Name));
            }
        }
    }
}
