using System.Diagnostics.Metrics;

namespace ShortenerURLService.Infrastructures.Metrics
{
    public sealed class ShortenerUrlDiagnostics
    {
        public const string MeterName = "ShortenerService";
        public const string ShortenerCode = "ShortenerService.ShortenerCode";
        public const string Redirect = "ShortenerService.Redirect";
        public const string ExceptionDirect = "ShortenerService.ExceptionDirect";

        private readonly Counter<long> _shortenerCode;
        private readonly Counter<long> _redirect;
        private readonly Counter<long> _exceptionDirect;

        public ShortenerUrlDiagnostics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create(MeterName);
            _shortenerCode = meter.CreateCounter<long>(ShortenerCode);
            _redirect = meter.CreateCounter<long>(Redirect);
            _exceptionDirect = meter.CreateCounter<long>(ExceptionDirect);
        }

        public void AddShortenerCode() => _shortenerCode.Add(1);
        public void AddRedirect() => _redirect.Add(1);
        public void AddExceptionDirect()=> _exceptionDirect.Add(1);

    }
}
