using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SP_Color_Wheel.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
    public class AsyncRelayCommand<T> : IAsyncCommand
    {
        bool _isExecuting;
        object _errorHandler;
        Func<T, Task> _ExecuteMethod;
        Func<T, bool> _CanExecuteMethod;

        public AsyncRelayCommand(Func<T, Task> executeMethod, object errorHandler = null)
        {
            _errorHandler = errorHandler;
            _ExecuteMethod = executeMethod;
        }
        public AsyncRelayCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod, object errorHandler = null)
        {
            _errorHandler = errorHandler;
            _ExecuteMethod = executeMethod;
            _CanExecuteMethod = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged;
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter = null)
        {
            return !_isExecuting && (_CanExecuteMethod?.Invoke((T)parameter) ?? true);
        }

        public async Task ExecuteAsync(object parameter = null)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _ExecuteMethod((T)parameter).ConfigureAwait(false);
                }
                finally
                {
                    _isExecuting = false;
                }
            }
            OnCanExecuteChanged();
        }


        /// <summary>
        /// Empty Method 
        /// Don't use.
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return true;
        }
        /// <summary>
        /// Empty Method 
        /// Don't use.
        /// </summary>
        /// <returns></returns>
        void ICommand.Execute(object parameter )
        {
            ExecuteAsync((T)parameter).ConfigureAwait(false);
        }
    }
}
