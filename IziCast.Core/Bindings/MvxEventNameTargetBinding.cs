using System;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.WeakSubscription;

namespace IziCast.Core.Bindings
{
    public class MvxEventNameTargetBinding<TTarget> : MvxTargetBinding where TTarget : class
    {
        private readonly bool _useEventArgsAsCommandParameter;
        private readonly IDisposable _eventSubscription;

        private ICommand _currentCommand;

        public MvxEventNameTargetBinding(TTarget target, string targetEventName, bool useEventArgsAsCommandParameter = true) : base(target)
        {
            _useEventArgsAsCommandParameter = useEventArgsAsCommandParameter;
            _eventSubscription = target.WeakSubscribe(targetEventName, HandleEvent);
        }

        public override Type TargetType { get; } = typeof(ICommand);

        public override MvxBindingMode DefaultMode { get; } = MvxBindingMode.OneWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                _eventSubscription.Dispose();
            
            base.Dispose(isDisposing);
        }

        private void HandleEvent(object sender, EventArgs parameter)
        {
            var commandParameter = _useEventArgsAsCommandParameter ? (object)parameter : null;

            if (_currentCommand != null && _currentCommand.CanExecute(commandParameter))
                _currentCommand.Execute(commandParameter);
        }

        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _currentCommand = command;
        }
    }
}
