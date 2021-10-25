using System;
namespace MODEL
{
    public class VaccineSideEffect : BaseEntity
    {
        private int sideEfectNo;
        private int vaccineNo;
        private string remarks;
        public VaccineSideEffect()
        {
        }

        public VaccineSideEffect(int sideEfectNo, int vaccineNo, string remarks)
        {
            this.sideEfectNo = sideEfectNo;
            this.vaccineNo = vaccineNo;
            this.remarks = remarks;
        }

        public int SideEfectNo { get => sideEfectNo; set => sideEfectNo = value; }
        public int VaccineNo { get => vaccineNo; set => vaccineNo = value; }
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
            return sideEfectNo > 0 && vaccineNo > 0 && !string.IsNullOrEmpty(remarks);
        }
    }
}
