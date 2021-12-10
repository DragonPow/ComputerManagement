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
                var bill = db.BILL_REPAIR.AsNoTracking().Include(i=>i.ITEM_BILL_SERI).Where(i => currentBill.id == i.id).First();

                //Customer
                currentBill.customerId = bill.customerId;
                //currentBill.CUSTOMER = bill.CUSTOMER;

                //Information Product
                currentBill.nameProduct = bill.nameProduct;
                currentBill.seriId = bill.seriId;
                currentBill.ITEM_BILL_SERI = bill.ITEM_BILL_SERI;
                currentBill.attachments = bill.attachments;

                //Information Insurance
                currentBill.timeDelivery = bill.timeDelivery;
                currentBill.timeReceive = bill.timeReceive;
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
                    db.Entry(currentBill).State = EntityState.Added;
                    db.Entry(currentBill.CUSTOMER).State = EntityState.Unchanged;

                    var rs = await db.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
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

        public ITEM_BILL_SERI GetBillFromSeri(string seri)
        {
            using (var db = new ComputerManagementEntities())
            {
                return db.ITEM_BILL_SERI.Where(i => i.seri == seri).OrderByDescending(i=>i.id).FirstOrDefault();
            }
        }
    }
}
