﻿using ComputerProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Repository
{
    public class BillRepository
    {
        private IQueryable<BILL> CreateQuery(ComputerManagementEntities db, string text, DateTime? timeFrom, DateTime? timeTo)
        {
            IQueryable<BILL> query = db.BILLs.AsQueryable();

            if (!String.IsNullOrWhiteSpace(text))
            {
                text.Trim();
                int number;
                if (int.TryParse(text, out number))
                {
                    query = query.Where(i => i.id == number || i.CUSTOMER.phone.Contains(text));
                }
                else
                {
                    query = query.Where(i => i.CUSTOMER.phone.Contains(text));
                }
            }
            if (timeFrom.HasValue) query = query.Where(i => i.createTime.Year >= timeFrom.Value.Year && i.createTime.Month >= timeFrom.Value.Month && i.createTime.Day >= timeFrom.Value.Day);
            if (timeTo.HasValue) query = query.Where(i => i.createTime.Year <= timeTo.Value.Year && i.createTime.Month <= timeTo.Value.Month && i.createTime.Day <= timeTo.Value.Day);

            return query.OrderByDescending(i => i.createTime).ThenBy(i => i.id);
        }
        /// <summary>
        /// Load bill from database
        /// </summary>
        /// <param name="maxNumberInPage">Max number of bill in one page, if this equal 0, get all bill</param>
        /// <param name="numberPage">Index of page, start from 1</param>
        /// <param name="text">Search page by name</param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns>Collection bill with filter</returns>
        public Collection<BILL> LoadBills(int maxNumberInPage, int numberPage = 1, string text = null, DateTime? timeFrom = null, DateTime? timeTo = null)
        {
            if (maxNumberInPage < 0 || numberPage < 0) throw new ArgumentException("The number is larger than 0");
            Collection<BILL> bills = null;

            using (var db = new ComputerManagementEntities())
            {
                var query = CreateQuery(db, text, timeFrom, timeTo);
                if (maxNumberInPage > 0) query = query.Skip((numberPage - 1) * maxNumberInPage).Take(maxNumberInPage);

                var list = query.Include(i => i.CUSTOMER).AsEnumerable();

                bills = new ObservableCollection<BILL>(list);
            }

            return bills;
        }

        public void Delete(Bill currentBill)
        {
            using (var db = new ComputerManagementEntities())
            {
                db.Database.Log = Console.WriteLine;
                BILL b = db.BILLs.Include(i => i.ITEM_BILL).Include(i => i.ITEM_BILL_SERI).Where(i => i.id == currentBill.Id).First();

                foreach (var i in b.ITEM_BILL.ToList())
                {
                    db.ITEM_BILL.Remove(i);
                }
                foreach (var i in b.ITEM_BILL_SERI.ToList())
                {
                    db.ITEM_BILL_SERI.Remove(i);
                }
                db.BILLs.Remove(b);
                db.SaveChanges();
            }
        }

        public Collection<BILL> LoadBills(int maxNumberInPage, int numberPage = 1)
        {
            return LoadBills(maxNumberInPage, numberPage, null, null, null);
        }

        public Collection<BILL> LoadBills(int maxNumberInPage, DateTime? timeFrom, DateTime? timeTo, int numberPage = 1)
        {
            return LoadBills(maxNumberInPage, numberPage, null, timeFrom, timeTo);
        }

        public int LoadNumberPages(int maxNumberInPage, string text = null, DateTime? timeFrom = null, DateTime? timeTo = null)
        {
            using (var db = new ComputerManagementEntities())
            {
                var query = CreateQuery(db, text, timeFrom, timeTo);
                return (query.Count() - 1) / maxNumberInPage + 1;
            }
        }

        public int LoadNumberPages(int maxNumberInPage, DateTime? timeFrom, DateTime? timeTo)
        {
            return LoadNumberPages(maxNumberInPage, null, timeFrom, timeTo);
        }

        public void RemoveAsync(BILL bill)
        {
            using (var db = new ComputerManagementEntities())
            {
                db.Database.Log = Console.WriteLine;
                var b = db.BILLs.Include(i => i.ITEM_BILL).Include(i => i.ITEM_BILL_SERI).Where(i => i.id == bill.id).First();
                db.BILLs.Remove(b);
                //db.SaveChangesAsync();
            }
        }

        public BILL LoadDetailBill(int billId)
        {
            BILL bill = null;
            try
            {
                using (var db = new ComputerManagementEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    bill = db.BILLs.Include(i => i.CUSTOMER).Include(i => i.ITEM_BILL).Include(i => i.ITEM_BILL_SERI.Select(j => j.PRODUCT)).Where(i => i.id == billId).First();
                }
            }
            catch (ArgumentNullException e)
            {
                throw e;
            }
            return bill;
        }
    }
}