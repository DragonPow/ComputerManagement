using ComputerProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Repository
{
    public class StoreRepository
    {
        public static StoreInformation GetStoreInformation()
        {
            using (var db = new ComputerManagementEntities())
            {
                StoreInformation storeInfor = new StoreInformation();

                storeInfor.Name = db.REGULATIONs.Where(i => i.name == "StoreName").Select(i=>i.value).First();
                storeInfor.Phone = db.REGULATIONs.Where(i => i.name == "StorePhone").Select(i => i.value).First();
                storeInfor.Address = db.REGULATIONs.Where(i => i.name == "StoreAdress").Select(i => i.value).First();

                return storeInfor;
            }
        }
    }
}
