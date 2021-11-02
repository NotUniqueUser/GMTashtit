using System;
namespace MODEL
{
    public class VaccineSideEffect : BaseEntity
    {
        private string sideEfectNo;
        private string vaccineNo;
        private string remarks;
        public VaccineSideEffect()
        {
        }

        public VaccineSideEffect(string sideEfectNo, string vaccineNo, string remarks)
        {
            this.sideEfectNo = sideEfectNo;
            this.vaccineNo = vaccineNo;
            this.remarks = remarks;
        }

        public string SideEfectNo { get => sideEfectNo; set => sideEfectNo = value; }
        public string VaccineNo { get => vaccineNo; set => vaccineNo = value; }
        public string Remarks { get => remarks; set => remarks = value; }

        public override bool Equals(object obj)
        {
            return obj is VaccineSideEffect efect &&
                   base.Equals(obj) &&
                   sideEfectNo == efect.sideEfectNo &&
                   vaccineNo == efect.vaccineNo &&
                   remarks == efect.remarks;
        }

        public override bool Validate()
        {
            return !(string.IsNullOrEmpty(sideEfectNo) || string.IsNullOrEmpty(vaccineNo) || string.IsNullOrEmpty(remarks));
        }
    }
}
