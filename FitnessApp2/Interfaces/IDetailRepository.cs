using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IDetailRepository
    {
        ICollection<Detail> GetDetails();
        Detail GetDetail(int? id);
        Detail GetDetailByEmail(string email);
        Detail GetDetailByPhone(string phone);
        public Detail GetDetailByPhoneAndEmail(string email, string phone);
        bool DetailExists(int? detailId);

        //creating
        public bool CreateDetail(Detail detail);

    }
}
