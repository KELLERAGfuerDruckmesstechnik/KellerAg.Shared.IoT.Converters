using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KellerAg.Shared.Entities.EventTrigger
{
    public class ValidatableEvent : ValidationBase, IEvent, INotifyPropertyChanged
    {
        private readonly IEventValidationMessages _validationMessages;

        public ValidatableEvent(IEventValidationMessages validationMessages)
        {
            _validationMessages = validationMessages;
            Event = new Event();
        }

        public ValidatableEvent(Event ev, IEventValidationMessages validationMessages)
        {
            Event = ev;
            _validationMessages = validationMessages;
        }

        public Event Event { get; set; }

        public RecordingEvent EventType
        {
            get => Event.EventType;
            set
            {
                Event.EventType = value;
                OnPropertyChanged(nameof(EventType));
            }
        }

        public TimeSpan DetectionInterval
        {
            get => Event.DetectionInterval;
            set
            {
                Event.DetectionInterval = value;
                OnPropertyChanged(nameof(DetectionInterval));
            }
        }

        public TimeSpan MeasureInterval
        {
            get => Event.MeasureInterval;
            set
            {
                Event.MeasureInterval = value;
                OnPropertyChanged(nameof(MeasureInterval));
            }
        }

        public double PrimaryValue
        {
            get => Event.PrimaryValue;
            set
            {
                Event.PrimaryValue = value;
                OnPropertyChanged(nameof(PrimaryValue));
            }
        }

        public double SecondaryValue
        {
            get => Event.SecondaryValue;
            set
            {
                Event.SecondaryValue = value;
                OnPropertyChanged(nameof(SecondaryValue));
            }
        }

        public ChannelInfo TriggerChannel
        {
            get => Event.TriggerChannel;
            set
            {
                Event.TriggerChannel = value;
                OnPropertyChanged(nameof(TriggerChannel));
            }
        }

        public int MeasurementsForAverage
        {
            get => Event.MeasurementsForAverage;
            set
            {
                Event.MeasurementsForAverage = value;
                OnPropertyChanged(nameof(MeasurementsForAverage));
            }
        }
        protected override void ValidateAttributes()
        {
            #region DetectionIntervalValidation

            if (!ValidationErrors.TryGetValue(nameof(DetectionInterval), out var errors))
            {
                errors = new List<string>();
            }
            else
            {
                errors.Clear();
            }

            if (EventType == RecordingEvent.OnAndOff ||
                EventType == RecordingEvent.ChBiggerThan ||
                EventType == RecordingEvent.ChSmallerThan ||
                EventType == RecordingEvent.Delta)
            {

                if (DetectionInterval.CompareTo(new TimeSpan(0, 0, 0)) <= 0)
                {
                    errors.Add(_validationMessages.DetectionIntervalMustBeBiggerThanZero);
                }

                if (DetectionInterval.CompareTo(new TimeSpan(0, 0, 65535)) > 0)
                {
                    errors.Add(_validationMessages.DetectionIntervalMustBeShorterThan);
                }
            }

            ValidationErrors[nameof(DetectionInterval)] = errors;
            RaiseErrorsChanged(nameof(DetectionInterval));

            #endregion

            #region MeasureIntervalValidation

            if (!ValidationErrors.TryGetValue(nameof(MeasureInterval), out var measureIntervalErrors))
            {
                measureIntervalErrors = new List<string>();
            }
            else
            {
                measureIntervalErrors.Clear();
            }


            if (EventType == RecordingEvent.OnAndOff ||
                EventType == RecordingEvent.ChBiggerThan ||
                EventType == RecordingEvent.ChSmallerThan)
            {

                if (MeasureInterval.CompareTo(new TimeSpan(0, 0, 0)) <= 0)
                {
                    measureIntervalErrors.Add(_validationMessages.MeasureIntervalMustBeBiggerThanZero);
                }

                if (MeasureInterval.CompareTo(new TimeSpan(0, 0, 65535)) > 0)
                {
                    measureIntervalErrors.Add(_validationMessages.MeasureIntervalMustBeShorterThan);
                }
            }

            ValidationErrors[nameof(MeasureInterval)] = measureIntervalErrors;
            RaiseErrorsChanged(nameof(MeasureInterval));

            #endregion

            #region MeasurementsForAverageValidation

            if (!ValidationErrors.TryGetValue(nameof(MeasurementsForAverage), out var measurementsForAverageErrors))
            {
                measurementsForAverageErrors = new List<string>();
            }
            else
            {
                measurementsForAverageErrors.Clear();
            }

            if (MeasurementsForAverage < 1 || MeasurementsForAverage > 255)
            {
                measurementsForAverageErrors.Add(_validationMessages.MeasurementsForAverageBetween1And255);
            }

            if (MeasurementsForAverage > MeasureInterval.TotalSeconds)
            {
                measurementsForAverageErrors.Add(_validationMessages.MeasurementsForAverageMustBeSmallerThanInterval);
            }

            ValidationErrors[nameof(MeasurementsForAverage)] = measurementsForAverageErrors;
            RaiseErrorsChanged(nameof(MeasurementsForAverage));

            #endregion
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ValidateAsync();
        }
    }
}
