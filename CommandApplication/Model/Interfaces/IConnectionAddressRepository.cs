using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    public interface IConnectionAddressRepository
    {
        void AddAddress(ConnectionAddress conAddress);
        void DeleteAddress(ConnectionAddress conAddress);
        void EditAddress(ConnectionAddress conAddress);
        ConnectionAddress GetActiveConnection();
        List<ConnectionAddress> ListAllAddresses();
    }
}
