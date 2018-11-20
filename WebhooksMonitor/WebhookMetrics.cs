using App.Metrics;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using System;

namespace WebhooksMonitor
{
    public sealed class WebhookMetrics
    {
        public static MeterOptions EventMeter { get; } = new MeterOptions
        {
            Name = "Event Rate",
            MeasurementUnit = Unit.Events,
            RateUnit = TimeUnit.Minutes
        };

        public static HistogramOptions EventHistogram { get; } = new HistogramOptions
        {
            Name = "Event Delay",
            MeasurementUnit = Unit.Custom("delay (ms)"),
        };


        private readonly IMeasureMetrics _measure;

        public WebhookMetrics(IMetrics metrics)
        {
            _measure = metrics.Measure;
        }

        public void Mark(WebhookEvent e)
        {
            var tags = new MetricTags("name", e.Type);

            _measure.Meter.Mark(EventMeter);
            _measure.Meter.Mark(EventMeter, tags);

            var delay = (DateTime.UtcNow - e.Timestamp).Ticks / TimeSpan.TicksPerMillisecond;

            _measure.Histogram.Update(EventHistogram, delay);
            _measure.Histogram.Update(EventHistogram, tags, delay);
        }
    }
}
