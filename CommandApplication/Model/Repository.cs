using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    public class Repository : Irepository
    {
        private DataContext db = new DataContext();

        public void AddAddress(ConnectionAddress conAddress)
        {
            db.ConnectionAddresses.Add(conAddress);
        }
        public void DeleteAddress(ConnectionAddress conAddress)
        {

        }
        public List<ConnectionAddress> ListAllAddresses()
        {
            return null;
        }
    }
}
