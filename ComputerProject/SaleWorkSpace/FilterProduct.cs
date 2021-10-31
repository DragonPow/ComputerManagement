using ComputerProject.HelperService;
using ComputerProject.Model;
using System;

namespace ComputerProject.SaleWorkSpace
{
    public interface IFilterProductState
    {
        Model.Category CategoryType { get; set; }
        string Supplier { get; set; }
        int PriceHighest { get; set; }
        int PriceLowest { get; set; }
        int TimeWarranty { get; set; }
    }
    public class FilterProduct : BaseViewModel, IFilterProductState
    {
        Model.Category _categoryType;
        string _supplier;
        int _priceHighest;
        int _priceLowest;
        int _timeWarranty;
        IFilterProductState _initState;

        public Category CategoryType
        {
            get => _categoryType;
            set
            {
                if (value != _categoryType)
                {
                    _categoryType = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Supplier
        {
            get => _supplier;
            set
            {
                if (value != _supplier)
                {
                    _supplier = value;
                    OnPropertyChanged();
                }
            }
        }
        public int PriceHighest
        {
            get => _priceHighest;
            set
            {
                if (value != _priceHighest)
                {
                    _priceHighest = value;
                    OnPropertyChanged();
                }
            }
        }
        public int PriceLowest
        {
            get => _priceLowest;
            set
            {
                if (value != _priceLowest)
                {
                    _priceLowest = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TimeWarranty
        {
            get => _timeWarranty;
            set
            {
                if (value != _timeWarranty)
                {
                    _timeWarranty = value;
                    OnPropertyChanged();
                }
            }
        }

        public FilterProduct(IFilterProductState initstate)
        {
            this._initState = initstate;
        }

        /// <summary>
        /// Undo init state of filter
        /// </summary>
        /// <returns>Return true if Have an init state, otherwise return false</returns>
        public bool Undo()
        {
            if (_initState != null)
            {
                this.CategoryType = _initState.CategoryType.Copy();
                this.Supplier = _initState.Supplier;
                this.PriceHighest = _initState.PriceHighest;
                this.PriceLowest = _initState.PriceLowest;
                this.TimeWarranty = _initState.TimeWarranty;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}