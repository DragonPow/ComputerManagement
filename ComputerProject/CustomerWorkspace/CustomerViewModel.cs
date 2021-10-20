using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerViewModel
    {
        private CUSTOMER _model;
        public CUSTOMER Model => _model;

        public CustomerViewModel()
        {
            this._model = new CUSTOMER();
            this._model.id = -1;
        }

        public CustomerViewModel(CUSTOMER model)
        {
            this._model = model;
        }

        public String FullName
        {
            get => _model.name;
            set => _model.name = value.ToString();
        }

        public String Address
        {
            get => _model.address;
            set => _model.address = value.ToString();
        }

        public String PhoneNumber
        {
            get => _model.phone;
            set => _model.phone = value.ToString();
        }

        public String Birthday
        {
            get => _model.birthday;
            set => _model.birthday = value.ToString();
        }

        public int Point
        {
            get => _model.point;
            set => _model.point = value;
        }

        public String Point_String
        {
            get => _model.point.ToString();
            set => _model.point = int.Parse(value.ToString());
        }

        public void SaveSync()
        {
            this.Save().Wait();
        }

        public Task Save()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                if (_model.id == -1)
                {
                    // Add new row to db
                    this._model = db.CUSTOMERs.Add(this._model);
                }
                else
                {
                    // Update from old one
                    var old = db.CUSTOMERs.Where(c => c.id == _model.id).First();
                    this.CopyTo(old);
                }
                return db.SaveChangesAsync();
            }
        }

        public void DeleteSync()
        {
            this.Delete().Wait();
        }

        public Task Delete()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                if (_model.id == -1)
                {
                    return Task.CompletedTask;
    }
                else
                {
                    // Update from old one
                    var old = db.CUSTOMERs.Where(c => c.id == _model.id).First();
                    if (old != null)
                    {
                        db.CUSTOMERs.Remove(old);
                    }
                }
                return db.SaveChangesAsync();
            }
        }

        public void CopyTo(CUSTOMER other)
        {
            other.name = this._model.name;
            other.address = this._model.address;
            other.phone = this._model.phone;
            other.birthday = this._model.birthday;
            other.point = this._model.point;
        }
    }
}
