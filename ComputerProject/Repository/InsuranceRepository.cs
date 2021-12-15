using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Repository
{
    public class InsuranceRepository
    {
        public void LoadInsurance(BILL_REPAIR currentBill)
        {
            using (var db = new ComputerManagementEntities())
            {
                var bill = db.BILL_REPAIR.AsNoTracking().Include(i => i.ITEM_BILL_SERI.PRODUCT).Where(i => currentBill.id == i.id).First();

                //Customer
                currentBill.customerId = bill.customerId;
                //currentBill.CUSTOMER = bill.CUSTOMER;

                //Information Product
                currentBill.seriId = bill.seriId;
                currentBill.ITEM_BILL_SERI = bill.ITEM_BILL_SERI;
                currentBill.attachments = bill.attachments;

                //Information Insurance
                currentBill.timeDelivery = bill.timeDelivery;
                currentBill.timeReceive = bill.timeReceive;
                currentBill.status = bill.status;
                currentBill.desDetailRepair = bill.desDetailRepair;
                currentBill.desProblem = bill.desProblem;
                currentBill.desReceiveItems = bill.desReceiveItems;
                currentBill.customerMoney = bill.customerMoney;
                currentBill.price = bill.price;
            }
        }

        public CUSTOMER GetCustomer(int customerId)
        {
            using (var db = new ComputerManagementEntities())
            {
                var customer = db.CUSTOMERs.Where(i => i.id == customerId).FirstOrDefault();
                return customer;
            }
        }

        public async Task<bool> SaveAsync(BILL_REPAIR currentBill)
        {
            try
            {
                using (var db = new ComputerManagementEntities())
                {
                    if (currentBill.id == 0)
                    {
                        var warrantyTime = currentBill.ITEM_BILL_SERI?.warrantyEndTime;
                        currentBill.isWarranty = warrantyTime.HasValue && TimerService.CheckDayLarger(warrantyTime.Value, DateTime.Now);
                    }

                    //currentBill.CUSTOMER = null;
                    db.Entry(currentBill.CUSTOMER).State = EntityState.Unchanged;
                    if (currentBill.seriId.HasValue)
                    {
                        //currentBill.ITEM_BILL_SERI = null;
                        currentBill.ITEM_BILL_SERI.BILL_REPAIR = null;
                        db.Entry(currentBill.ITEM_BILL_SERI).State = EntityState.Unchanged;
                        //db.Entry(currentBill.ITEM_BILL_SERI.PRODUCT).State = EntityState.Unchanged;
                    }

                    db.Entry(currentBill).State = currentBill.id == 0 ? EntityState.Added : EntityState.Modified;


                    var rs = await db.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int billid)
        {
            try
            {
                using (var db = new ComputerManagementEntities())
                {
                    var bill = new BILL_REPAIR() { id = billid };
                    db.BILL_REPAIR.Attach(bill);
                    db.BILL_REPAIR.Remove(bill);

                    var rs = await db.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ITEM_BILL_SERI> GetBillFromSeri(string seri)
        {
            using (var db = new ComputerManagementEntities())
            {
                return db.ITEM_BILL_SERI.AsNoTracking().Include(i => i.PRODUCT).Where(i => i.seri == seri).OrderByDescending(i => i.id).ToList();
            }
        }
    }
}
