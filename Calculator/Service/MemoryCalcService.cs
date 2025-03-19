using Calculator.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Calculator.ViewModels;

namespace Calculator.Service
{
    public class MemoryCalcService
    {
        private readonly DisplayModel _displayModel;
        private readonly MemoryModel _memoryModel;

        public MemoryCalcService(DisplayModel displayModel)
        {
            _displayModel = displayModel;
            _memoryModel = new MemoryModel();
        }

        public bool HasMemoryValue => _memoryModel.HasMemoryValue;

        public ObservableCollection<MemoryItem> MemoryItems => _memoryModel.Items;

        public void MemoryClear()
        {
            _memoryModel.Clear();
        }

        public void MemoryRecall()
        {
            if (_memoryModel.HasMemoryValue)
            {
                double lastValue = _memoryModel.GetLastValue();
                _displayModel.MainDisplayText = lastValue.ToString();
            }
        }

        public void MemoryAdd()
        {
            if (double.TryParse(_displayModel.MainDisplayText, out double value))
            {
                if (_memoryModel.HasMemoryValue)
                {
                    _memoryModel.AddToLastItem(value);
                }
                else
                {
                    _memoryModel.AddItem(value);
                }
            }
        }

        public void MemorySubtract()
        {
            if (double.TryParse(_displayModel.MainDisplayText, out double value))
            {
                if (_memoryModel.HasMemoryValue)
                {
                    _memoryModel.SubtractFromLastItem(value);
                }
                else
                {
                    _memoryModel.AddItem(-value);
                }
            }
        }

        public void MemoryStore()
        {
            if (double.TryParse(_displayModel.MainDisplayText, out double value))
            {
                _memoryModel.AddItem(value);
            }
        }

        public void MemoryItemAdd(MemoryItem item)
        {
            if (double.TryParse(_displayModel.MainDisplayText, out double value))
            {
                item.Value += value;
            }
        }

        public void MemoryItemSubtract(MemoryItem item)
        {
            if (double.TryParse(_displayModel.MainDisplayText, out double value))
            {
                item.Value -= value;
            }
        }

        public void MemoryItemRemove(MemoryItem item)
        {
            _memoryModel.RemoveItem(item);
        }

        public void MemoryItemRecall(MemoryItem item)
        {
            _displayModel.MainDisplayText = item.Value.ToString();
        }
    }
}