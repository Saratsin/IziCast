namespace IziCast.Core.Models
{
    public class Try
    {
        protected Try(bool operationSucceeded)
        {
            OperationSucceeded = operationSucceeded;
        }

		public bool OperationSucceeded { get; }

        public static Try Create(bool operationSucceed) => new Try(operationSucceed);

        public static Try Succeed() => new Try(true);

        public static Try Unsucceed() => new Try(false);
    }
}