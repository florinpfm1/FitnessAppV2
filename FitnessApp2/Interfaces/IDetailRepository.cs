using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IDetailRepository
    {
        //retrieving
        public ICollection<Detail> GetDetails();
        public Detail GetDetail(int? id);
        public Detail GetDetailByEmail(string email);
        public Detail GetDetailByPhone(string phone);
        public Detail GetDetailByPhoneAndEmail(string email, string phone);
        //checking
        public bool DetailExists(int? detailId);

        //creating
        public bool CreateDetail(Detail detail);

    }
}
