namespace MODEL
{
    public class VaccineSideEffect : BaseEntity
    {
        public VaccineSideEffect()
        {
        }

        public VaccineSideEffect(string sideEfectNo, string vaccineNo, string remarks)
        {
            this.SideEfectNo = sideEfectNo;
            this.VaccineNo = vaccineNo;
            this.Remarks = remarks;
        }

        public string SideEfectNo { get; set; }

        public string VaccineNo { get; set; }

        public string Remarks { get; set; }

        public override bool Equals(object obj)
        {
            return obj is VaccineSideEffect efect &&
                   base.Equals(obj) &&
                   SideEfectNo == efect.SideEfectNo &&
                   VaccineNo == efect.VaccineNo &&
                   Remarks == efect.Remarks;
        }

        public override bool Validate()
        {
            return !(string.IsNullOrEmpty(SideEfectNo) || string.IsNullOrEmpty(VaccineNo) ||
                     string.IsNullOrEmpty(Remarks));
        }
    }
}