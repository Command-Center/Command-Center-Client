using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    public class ConnectionAddressRepository : IConnectionAddressRepository
    {
        private DataContext db = new DataContext();

        public void AddAddress(ConnectionAddress conAddress)
        {
            db.ConnectionAddresses.Add(conAddress);
            if (conAddress.isActive == true)
            {
                ChangeActiveConnection();
            }
            db.SaveChanges();
        }

        private void ChangeActiveConnection()
        {
            ConnectionAddress conAdr = GetActiveConnection();
            conAdr.isActive = false;
            EditAddress(conAdr);
        }

        public void DeleteAddress(ConnectionAddress conAddress)
        {
            db.ConnectionAddresses.Remove(conAddress);
            db.SaveChanges();
        }
        public List<ConnectionAddress> ListAllAddresses()
        {
            return db.ConnectionAddresses.ToList();
        }
        public void EditAddress(ConnectionAddress conAddress) //Edited incomming
        {
            ConnectionAddress originalConAddress = ListAllAddresses().Where(ca => ca.Id == conAddress.Id).Single();
            var temp = originalConAddress.isActive;
            originalConAddress.Address = conAddress.Address;
            originalConAddress.Comment = conAddress.Comment;
            originalConAddress.isActive = conAddress.isActive;
            if (conAddress.isActive == true && temp == false)
            {
                ChangeActiveConnection();
            }
            db.Entry(originalConAddress).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public ConnectionAddress GetActiveConnection()
        {
            return db.ConnectionAddresses.ToList().Where(ca => ca.isActive == true).Single();
        }
    }
}
