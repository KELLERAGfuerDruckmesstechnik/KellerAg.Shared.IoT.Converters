using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KellerAg.Shared.Entities.Validation
{
    public abstract class ValidationBase : INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public Dictionary<string, List<string>> ValidationErrors = new Dictionary<string, List<string>>();

        private readonly object _lock = new object();

        public IEnumerable GetErrors(string propertyName)
        {
            ValidationErrors.TryGetValue(propertyName, out var errorsForName);
            return errorsForName;
        }

        public bool HasErrors
        {
            get { return ValidationErrors.Any(e => e.Value != null && e.Value.Count > 0); }
        }
        public void RaiseErrorsChanged(string propertyName)
        {
            EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
            if (handler == null) return;
            var arg = new DataErrorsChangedEventArgs(propertyName);
            handler.Invoke(this, arg);
        }

        public Task ValidateAsync()
        {
            return Task.Run(() => RealValidation());
        }
        protected abstract void ValidateAttributes();

        private void RealValidation()
        {
            lock (_lock)
            {
                ValidateAttributes();
            }
        }
    }
}
