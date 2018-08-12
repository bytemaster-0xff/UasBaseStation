using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.UWP
{
    public class BindingHelper<TDest> : IDisposable  where TDest : class
    {
        TDest _dest;
        public BindingHelper(TDest dest)
        {
            _dest = dest;
        }

        private List<IDisposable> _bindings = new List<IDisposable>();

        public Binding<TSource, TDest> Add<TSource>(TSource model)  where TSource : INotifyPropertyChanged
        {
            var binding = new Binding<TSource, TDest>(model, _dest);
            _bindings.Add(binding);
            return binding;
        }
        
        public void Dispose()
        {
            foreach(var prop in _bindings)
            {
                prop.Dispose();
            }
        }

       
        public class Binding<TSource, TBindingDestination> : IDisposable where TBindingDestination : class where TSource : INotifyPropertyChanged
        {
            Func<TSource, object> _sourceProperty;
            TSource _dataContext;
            TBindingDestination _bindingDestination;
            PropertyInfo _destPropertyInfo;

            public Binding(TSource model, TBindingDestination bindingDestination)
            {
                _bindingDestination = bindingDestination;
                _dataContext = model;
                _dataContext.PropertyChanged += _dataContext_PropertyChanged;
             }

            private void _dataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if(e.PropertyName == PropertyName)
                {
                    Update();
                }
            }

            public void Dispose()
            {
                _dataContext.PropertyChanged -= _dataContext_PropertyChanged;
            }

            public void Update()
            {
                Object result = _sourceProperty.Invoke(_dataContext);
                _destPropertyInfo.SetValue(_bindingDestination, result);
            }

            public void For(Expression<Func<TSource, object>> source, string propertyName)
            {
                PropertyName = propertyName;
                _sourceProperty = source.Compile();
                
                var destType = typeof(TBindingDestination);
                _destPropertyInfo = destType.GetProperty(propertyName);
                Update();
            }

            public void For(Expression<Func<TSource, object>> source, Expression<Func<TBindingDestination, object>> dest)
            {
                //TODO: This is is VERY hacky...but body is something like clt.PropertyName 
                var regEx = new Regex(@"^Convert\(\w+\.(\w+),.+$");
                var match = regEx.Match(dest.Body.ToString());
                if(match.Success)
                {
                    PropertyName = match.Groups[1].Value;
                }
                else
                {
                    PropertyName = dest.Body.ToString().Split(".")[1];
                }

                if(String.IsNullOrEmpty(PropertyName))
                {
                    throw new Exception($"Could not parse property name from {dest.Body.ToString()}");
                }
                
                _sourceProperty = source.Compile();

                var destType = typeof(TBindingDestination);
                _destPropertyInfo = destType.GetProperty(PropertyName);
                Update();
            }

            public string PropertyName { get; private set; }
        }

    }
}
