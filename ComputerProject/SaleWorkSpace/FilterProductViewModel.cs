using ComputerProject.HelperService;
using ComputerProject.Model;
using System;
using System.Windows.Input;

namespace ComputerProject.SaleWorkSpace
{
    public interface IFilterProductState
    {
        Model.Category CategoryType { get; set; }
        string Supplier { get; set; }
        stateProduct StateProduct {get; }
        int PriceHighest { get; set; }
        int PriceLowest { get; set; }
        int TimeWarranty { get; set; }
        event EventHandler FilterClickedEvent;
    }
    public class FilterProductViewModel : BaseViewModel, IFilterProductState
    {
        #region Fields
        Model.Category _categoryType;
        string _supplier;
        stateProduct _stateProduct;
        int _priceHighest;
        int _priceLowest;
        int _timeWarranty;
        IFilterProductState _initState;

        ICommand _filterCommand;
        ICommand _undoCommand;
        #endregion //Fields

        #region Properties
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
        public stateProduct StateProduct
        {
            get => _stateProduct;
            set
            {
                if (value!=_stateProduct)
                {
                    _stateProduct = value;
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

        public ICommand FilterCommand
        {
            get
            {
                if (null==_filterCommand)
                {
                    _filterCommand = new RelayCommand(a => Filter());
                }
                return _filterCommand;
            }
        }
        public ICommand UndoCommand
        {
            get
            {
                if (null == _undoCommand)
                {
                    _undoCommand = new RelayCommand(a => Undo());
                }
                return _undoCommand;
            }
        }
        #endregion //Properties

        public FilterProductViewModel(IFilterProductState initstate)
        {
            this._initState = initstate;

            if (initstate != null)
            {
                this.Supplier = initstate.Supplier;
                this.PriceHighest = initstate.PriceHighest;
                this.PriceLowest = initstate.PriceLowest;
                this.TimeWarranty = initstate.TimeWarranty;
                this.StateProduct = initstate.StateProduct;
            }
        }

        /// <summary>
        /// Undo init state of filter
        /// </summary>
        /// <returns>Return true if Have an init state, otherwise return false</returns>
        public bool Undo()
        {
            if (_initState == null)
            {
                //this.CategoryType = _initState.CategoryType.Copy();
                //this.Supplier = _initState.Supplier;
                //this.PriceHighest = _initState.PriceHighest;
                //this.PriceLowest = _initState.PriceLowest;
                //this.TimeWarranty = _initState.TimeWarranty;

                //this.CategoryType = null;
                this.Supplier = null;
                this.PriceHighest = 0;
                this.PriceLowest = 0;
                this.TimeWarranty = 0;
                UndoFilterEvent?.Invoke(this, null);
                return true;
            }
            else
            {
                UndoFilterEvent?.Invoke(this, null);
                return false;
            }
        }

        public void Filter()
        {
            FilterClickedEvent?.Invoke(this, null);
        }

        public event EventHandler FilterClickedEvent;
        public event EventHandler UndoFilterEvent;
    }
}