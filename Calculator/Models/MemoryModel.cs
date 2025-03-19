using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Calculator.Models
{
    public class MemoryModel : INotifyPropertyChanged
    {
        private ObservableCollection<MemoryItem> _items;
        private bool _hasMemoryValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MemoryItem> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public bool HasMemoryValue
        {
            get => _hasMemoryValue;
            private set
            {
                _hasMemoryValue = value;
                OnPropertyChanged(nameof(HasMemoryValue));
            }
        }

        public MemoryModel()
        {
            _items = new ObservableCollection<MemoryItem>();
            _hasMemoryValue = false;
        }

        public void Clear()
        {
            Items.Clear();
            HasMemoryValue = false;
        }

        public double GetLastValue()
        {
            return Items.LastOrDefault()?.Value ?? 0;
        }

        public void AddItem(double value)
        {
            Items.Add(new MemoryItem { Value = value });
            HasMemoryValue = true;
        }

        public void RemoveItem(MemoryItem item)
        {
            Items.Remove(item);
            if (Items.Count == 0)
            {
                HasMemoryValue = false;
            }
        }

        public void AddToLastItem(double value)
        {
            if (Items.Count > 0)
            {
                Items.Last().Value += value;
            }
        }

        public void SubtractFromLastItem(double value)
        {
            if (Items.Count > 0)
            {
                Items.Last().Value -= value;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MemoryItem : INotifyPropertyChanged
    {
        private double _value;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}